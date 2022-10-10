namespace ProjectDK.Middleware
{
    public class RequestMethodMiddleware : Exception
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestMethodMiddleware> logger;

        public RequestMethodMiddleware(RequestDelegate next, ILogger<RequestMethodMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "GET")
            {
                logger.LogInformation("This is a GET method request");
                await next(httpContext);
                logger.LogInformation("Status code of response:" + httpContext.Response.StatusCode.ToString());
            }
            else if (httpContext.Request.Method == "POST")
            {
                logger.LogInformation("This is a POST method request");
                await next(httpContext);
                logger.LogInformation("Status code of response:" + httpContext.Response.StatusCode.ToString());
            }
        }
    }
}
