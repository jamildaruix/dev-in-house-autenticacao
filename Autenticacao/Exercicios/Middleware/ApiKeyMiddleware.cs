namespace Exercicios.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HEADER_API_KEY = "ApiKey";
        private const string SECRETS_API_KEY = "DD988170-8ED8-40F6-B278-1CC193FCEEDE";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var pathUrl = context?.Request?.Path.Value?.ToLower();

            if (pathUrl!.Contains("apikeyexemplo"))
            {
                if (!context!.Request.Headers.TryGetValue(HEADER_API_KEY, out var apiKeyRequest))
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Header da api key não encontrado");
                    return;
                }

                if (!SECRETS_API_KEY.Equals(apiKeyRequest))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Autorização negada");
                    return;
                }
            }

            await _next(context!);
        }
    }
}
