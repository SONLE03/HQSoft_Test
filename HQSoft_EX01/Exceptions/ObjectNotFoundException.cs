using System.Net;

namespace HQSoft_EX01.Exceptions
{
    public class ObjectNotFoundException : BusinessException
    {
        public ObjectNotFoundException(string message = "Not found") : base(message)
        {
            this.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
