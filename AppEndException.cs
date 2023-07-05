namespace AppEnd
{
    public class AppEndException : Exception
    {
        List<KeyValuePair<string, string>> ErrorMetadata = new();

        public AppEndException()
        {
        }

        public AppEndException(string message) : base(message)
        {
        }

        public AppEndException(string message, Exception inner) : base(message, inner)
        {
        }


        public AppEndException AddParam(string name, string value)
        {
            ErrorMetadata.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public AppEndException SetMetaData(List<KeyValuePair<string, string>> errorMetadata)
        {
            ErrorMetadata = errorMetadata;
            return this;
        }

    }
    
}
