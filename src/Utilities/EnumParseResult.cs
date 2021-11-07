using System;

namespace Utilities
{
    public class EnumParseResult<T>
        where T : struct
    {
        public bool Success
        {
            get
            {
                FillResult();
                return _parseSuccess;
            }
        }

        public T Result
        {
            get
            {
                FillResult();
                return _result.Value;
            }
        }

        private void FillResult()
        {
            if (_result != null)
            {
                return;
            }

            _parseSuccess = Enum.TryParse(_value, _ignoreCase, out T result);
            _result = result;
        }

        private readonly bool _ignoreCase;
        private readonly string _value;
        private T? _result;
        private bool _parseSuccess;

        public EnumParseResult(string value, bool ignoreCase)
        {
            _value = value;
            _ignoreCase = ignoreCase;
        }
    }
}