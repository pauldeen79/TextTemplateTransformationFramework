using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4
{
    public sealed class TokenParserState
    {
        private List<ITemplateToken<TokenParserState>> _tokens;
        private List<ITemplateSectionProcessor<TokenParserState>> _customSectionProcessors;
        private ITextTemplateProcessorContext<TokenParserState> _context;

        public string InitialFileName { get; private set; }
        public string FileName { get; private set; }
        public int CurrentMode { get; private set; }
        public int PreviousMode { get; private set; }
        public int LastMode { get; private set; }
        public int? NewMode { get; private set; }
        public int PrependNewLines { get; private set; }
        public int LineCounter { get; private set; }
        public int PositionCounter { get; private set; }
        public int Skip { get; private set; }
        public StringBuilder CurrentSectionBuilder { get; private set; }
        public int Position { get; private set; }
        public char? PreviousPosition { get; private set; }
        public char CurrentPosition { get; private set; }
        public char? NextPosition { get; private set; }
        public int? PreviousLine { get; private set; }

        public IEnumerable<ITemplateToken<TokenParserState>> Tokens => _tokens;
        public IEnumerable<ITemplateSectionProcessor<TokenParserState>> CustomSectionProcessors => _customSectionProcessors;
        public ITextTemplateProcessorContext<TokenParserState> Context => _context;
        public TemplateParameter[] Parameters => _context.Parameters;
        public IEnumerable<ITemplateToken<TokenParserState>> ExistingTokens => _context.ParentContext?.ExistingTokens ?? Enumerable.Empty<ITemplateToken<TokenParserState>>();

        public static TokenParserState Initial(ITextTemplateProcessorContext<TokenParserState> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            return new TokenParserState
            {
                _tokens = new List<ITemplateToken<TokenParserState>>(),
                _customSectionProcessors = new List<ITemplateSectionProcessor<TokenParserState>>(),
                _context = context,
                InitialFileName = context.TextTemplate.FileName,
                FileName = context.TextTemplate.FileName,
                CurrentMode = Mode.UnknownRender,
                PreviousMode = Mode.Unknown,
                LastMode = Mode.Unknown,
                LineCounter = 1,
                CurrentSectionBuilder = new StringBuilder(),
            };
        }

        public TokenParserState SetPosition(char currentPosition, char? nextPosition)
        {
            PreviousPosition = CurrentPosition;
            CurrentPosition = currentPosition;
            NextPosition = nextPosition;

            PositionCounter++;
            Position++;
            return this;
        }

        public TokenParserState ProcessSkip()
        {
            Skip--;
            return this;
        }

        public TokenParserState NewLine()
        {
            LineCounter++;
            PositionCounter = 1;

            var sectionContext = SectionContext.FromCurrentMode(CurrentMode, this);

            if (CurrentMode.IsTextRange())
            {
                CurrentSectionBuilder.AppendLine();
            }
            else if (!PreviousMode.IsUnknownRange() && !CurrentMode.IsCodeRange() && !CurrentMode.IsExpressionRange())
            {
                if (PrependNewLines == 0)
                {
                    PrependNewLines++;
                }
                else if (CurrentMode != Mode.Directive)
                {
                    _tokens.Add(new SourceSectionToken<TokenParserState>(sectionContext, null, false, new[] { sectionContext.CreateTextToken(Environment.NewLine) }));
                }
            }
            else
            {
                CurrentSectionBuilder.AppendLine();
            }

            if (Environment.NewLine.Length == 2)
            {
                Skip++; //skip next position
            }

            return this;
        }

        public TokenParserState StartSection(Func<TokenParserState, ProcessSectionResult<TokenParserState>> processStateDelegate)
        {
            if (processStateDelegate is null)
            {
                throw new ArgumentNullException(nameof(processStateDelegate));
            }

            if (CurrentMode == Mode.Directive)
            {
                //nested tags, not supported
                var sectionContext = SectionContext.FromCurrentMode(CurrentMode, this);
                _tokens.Add(new SourceSectionToken<TokenParserState>(sectionContext, null, false, new[] { new InitializeErrorToken<TokenParserState>(sectionContext, string.Format("Nested directive found at line {0}, position {1}, this is not supported", LineCounter, PositionCounter)) } ));
            }
            else
            {
                //first, process previous section
                if (CurrentSectionBuilder.Length > 0)
                {
                    processStateDelegate(this).Apply(PostProcess);
                }
                else
                {
                    NewMode = null;
                }

                //clear the section
                CurrentSectionBuilder.Clear();

                PreviousMode = CurrentMode;
                CurrentMode = Mode.StartBlock;
                PreviousLine = LineCounter;
            }
            Skip++; //skip next position

            return this;
        }

        public TokenParserState EndSection(Func<TokenParserState, ProcessSectionResult<TokenParserState>> processStateDelegate)
        {
            if (processStateDelegate is null)
            {
                throw new ArgumentNullException(nameof(processStateDelegate));
            }
            
            if (CurrentMode != Mode.Directive
                && !CurrentMode.IsCodeRange()
                && !CurrentMode.IsExpressionRange()
                && !CurrentMode.IsUnknownRange())
            {
                //missing start tag, not supported
                var sectionContext = SectionContext.FromCurrentMode(CurrentMode, this);
                _tokens.Add(new SourceSectionToken<TokenParserState>(sectionContext, null, false, new[] { new InitializeErrorToken<TokenParserState>(sectionContext, string.Format("Missing open tag for close tag at line {0}, position {1}, this is not supported", LineCounter, PositionCounter)) } ));
            }
            else
            {
                //first, process section
                processStateDelegate(this).Apply(PostProcess);

                //clear the section
                CurrentSectionBuilder.Clear();

                PreviousMode = CurrentMode;
                CurrentMode = GetUnknownRange(NewMode.GetValueOrDefault());
                NewMode = null;
                PreviousLine = null;
            }
            Skip++; //skip next position

            return this;
        }

        public TokenParserState ProcessLastSection(Func<TokenParserState, ProcessSectionResult<TokenParserState>> processStateDelegate)
        {
            if (processStateDelegate is null)
            {
                throw new ArgumentNullException(nameof(processStateDelegate));
            }

            if (CurrentMode == Mode.Directive)
            {
                var sectionContext = SectionContext.FromCurrentMode(CurrentMode, this);
                _tokens.Add(new SourceSectionToken<TokenParserState>(sectionContext, null, false, new[] { new InitializeErrorToken<TokenParserState>(sectionContext, "Missing close tag, this is not supported") }));
            }

            processStateDelegate(this).Apply(PostProcess);
            return this;
        }

        public TokenParserState ExpressionSection()
        {
            CurrentMode = GetExpressionRange(PreviousMode);
            _tokens.AddRange(GetPrependNewLines());
            CurrentSectionBuilder.Append(CurrentPosition);

            LastMode = CurrentMode;
            return this;
        }

        public TokenParserState Directive()
        {
            CurrentMode = Mode.Directive;
            LastMode = CurrentMode;
            PrependNewLines = 0;

            CurrentSectionBuilder.Append(CurrentPosition);
            return this;
        }

        public TokenParserState StartCodeBlock()
        {
            _tokens.AddRange(GetPrependNewLines());
            CurrentMode = GetCodeRange(PreviousMode);
            LastMode = CurrentMode;
            PrependNewLines = 0;

            CurrentSectionBuilder.Append(CurrentPosition);
            return this;
        }

        public TokenParserState AddToCurrentSection(Func<TokenParserState, ProcessSectionResult<TokenParserState>> processStateDelegate)
        {
            if (processStateDelegate is null)
            {
                throw new ArgumentNullException(nameof(processStateDelegate));
            }

            if (CurrentMode.IsUnknownRange())
            {
                CurrentMode = GetTextRange(CurrentMode);
                LastMode = CurrentMode;
                processStateDelegate(this).Apply(PostProcess);
            }

            CurrentSectionBuilder.Append(CurrentPosition);
            return this;
        }

        private ProcessSectionResult<TokenParserState> PostProcess(ProcessSectionResult<TokenParserState> arg)
        {
            _tokens.AddRange(arg.TemplateTokensSections);
            _tokens.AddRange(arg.AdditionalSections);
            _customSectionProcessors.AddRange(arg.SectionProcessors);
            NewMode = arg.SwitchToMode;
            if (CurrentMode.IsTextRange() && (PreviousMode.IsTextRange() || PreviousMode.IsExpressionRange()))
            {
                Enumerable.Range(0, PrependNewLines).ForEach(_ => CurrentSectionBuilder.Append(Environment.NewLine));
            }
            PrependNewLines = 0;
            return arg;
        }

        private IEnumerable<ITemplateToken<TokenParserState>> GetPrependNewLines()
            => LastMode.IsExpressionRange()
                ? Enumerable.Range(0, PrependNewLines).Select
                    (_ => new SourceSectionToken<TokenParserState>
                        (
                                SectionContext.FromCurrentMode(CurrentMode, this),
                                null,
                                false,
                                new[]
                                {
                                    SectionContext.FromCurrentMode(CurrentMode, this).CreateTextToken(Environment.NewLine)
                                }
                            )
                        )
                : Enumerable.Empty<ITemplateToken<TokenParserState>>();

        private int GetCodeRange(int lastMode)
            => GetRange(Mode.CodeRangeStart, lastMode);

        private int GetExpressionRange(int lastMode)
            => GetRange(Mode.ExpressionRangeStart, lastMode);

        private int GetTextRange(int lastMode)
            => GetRange(Mode.TextRangeStart, lastMode);

        private int GetUnknownRange(int lastMode)
            => GetRange(Mode.UnknownRangeStart, lastMode);

        private int GetRange(int rangeStart, int lastMode)
            => lastMode < 1000
                ? rangeStart + 1
                : rangeStart + int.Parse(lastMode.ToString().Right(1));
    }
}
