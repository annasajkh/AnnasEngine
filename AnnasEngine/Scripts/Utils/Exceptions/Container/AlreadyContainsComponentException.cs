namespace AnnasEngine.Scripts.Utils.Exceptions.Container
{
    public class AlreadyContainsComponentException : Exception
    {
        public AlreadyContainsComponentException() : base()
        {

        }

        public AlreadyContainsComponentException(string? message) : base(message)
        {

        }

        public AlreadyContainsComponentException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
