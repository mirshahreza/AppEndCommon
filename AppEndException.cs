namespace AppEnd
{
    public class AppEndException : Exception
    {
        public List<KeyValuePair<string, object>> ErrorMetadata = [];

        public AppEndException()
        {
        }

        public AppEndException(string message) : base(message)
        {
        }

        public AppEndException(string message, Exception inner) : base(message, inner)
        {
        }


        public AppEndException AddParam(string name, object value)
        {
            ErrorMetadata.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public AppEndException SetMetaData(List<KeyValuePair<string, object>> errorMetadata)
        {
            ErrorMetadata = errorMetadata;
            return this;
        }
    }

}
