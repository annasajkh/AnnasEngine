namespace AnnasEngine.Scripts.Utils.Exceptions.GameObject
{
    public class ParentNotFoundException : Exception
    {
        public ParentNotFoundException() : base()
        {

        }

        public ParentNotFoundException(string? message) : base(message)
        {

        }

        public ParentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
