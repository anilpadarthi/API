using SIMAPI.Repository.Interfaces;
using System.Net;

namespace SIMAPI.Data.Models
{
    public class CommonResponse
    {
        public bool status { get; set; }
        public int responseCode { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
        public object data { get; set; }

        public CommonResponse HandleException(Exception exception, IRepository commRepository)
        {
            commRepository.LogError(exception);
            var response = new CommonResponse();
            var errorMessage = "error found: " + exception?.Message + "<br/>" + exception?.StackTrace;
            response.status = false;
            response.message = errorMessage;
            response.data = errorMessage;
            response.statusCode = HttpStatusCode.InternalServerError;
            return response;
        }
    }
}
