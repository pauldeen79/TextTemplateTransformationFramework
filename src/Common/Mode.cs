namespace TextTemplateTransformationFramework.Common
{
    public static class Mode
    {
        public const int Unknown = 4999;
        public const int StartBlock = 1;
        public const int Directive = 3;
        public const int ExpressionRangeStart = 1000;
        public const int ExpressionRender = 1001;
        public const int ExpressionClassFeature = 1002;
        public const int ExpressionBaseClassFeature = 1003;
        public const int ExpressionNamespaceFeature = 1004;
        public const int ExpressionInitialize = 1005;
        public const int ExpressionRangeEnd = 1999;
        public const int CodeRangeStart = 2000;
        public const int CodeRender = 2001;
        public const int CodeClassFeature = 2002;
        public const int CodeBaseClassFeature = 2003;
        public const int CodeNamespaceFeature = 2004;
        public const int CodeInitialize = 2005;
        public const int CodeCompositionRootInitialize = 2006;
        public const int CodeCompositionRootFeature = 2007;
        public const int CodeRangeEnd = 2999;
        public const int TextRangeStart = 3000;
        public const int TextRender = 3001;
        public const int TextClassFeature = 3002;
        public const int TextBaseClassFeature = 3003;
        public const int TextNamespaceFeature = 3004;
        public const int TextInitialize = 3005;
        public const int TextRangeEnd = 3999;
        public const int UnknownRangeStart = 4000;
        public const int UnknownRender = 4001;
        public const int UnknownClassFeature = 4002;
        public const int UnknownBaseClassFeature = 4003;
        public const int UnknownNamespaceFeature = 4004;
        public const int UnknownInitialize = 4005;
        public const int UnknownRangeEnd = 4999;
    }
}
