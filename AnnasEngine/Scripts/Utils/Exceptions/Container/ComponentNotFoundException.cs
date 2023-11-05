namespace AnnasEngine.Scripts.Utils.Exceptions.Container
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException() : base()
        {

        }

        public ComponentNotFoundException(string? message) : base(message)
        {

        }

        public ComponentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
