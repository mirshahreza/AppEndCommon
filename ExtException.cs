﻿using System.Reflection;
using System.Runtime.CompilerServices;

namespace AppEndCommon
{
    public class ExtException : Exception
    {
	    private List<KeyValuePair<string, object>> _errorMetadata = [];

        public ExtException(string message, MethodBase? methodBase) : base(message)
        {
            AddParam("Site", methodBase.GetPlaceInfo());
        }

		public ExtException AddParam(string name, object value)
		{
			_errorMetadata.Add(new KeyValuePair<string, object>(name, value));
			return this;
		}

		public List<KeyValuePair<string, object>> GetParams()
		{
			return _errorMetadata;
		}

		public string GetMetadata()
        {
            return string.Join(", ", _errorMetadata);
        }
    }

    public static class ExtensionsForAppEndException
	{
		public static Exception GetEx(this ExtException appEndException)
		{
            var ex = new Exception($"{appEndException.Message}");
            foreach(var p in appEndException.GetParams()) ex.Data.Add(p.Key, p.Value);
			return ex;
		}

	}

}
