using System.IdentityModel.Tokens.Jwt;

namespace Cart.Api.Api.Middlewares
{
    public class IdentityTokenLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IdentityTokenLoggerMiddleware> _logger;

        public IdentityTokenLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<IdentityTokenLoggerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var bearerAuthHeader = (string)context.Request.Headers["Authorization"];

            if (bearerAuthHeader != null)
            {
                var indexOfBearerFieldEnd = bearerAuthHeader.IndexOf(' ');
                var token = bearerAuthHeader.Remove(0, indexOfBearerFieldEnd + 1);

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var id = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "sub").Value;
                var role = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

                _logger.LogInformation("Incoming authenticated [{httpMethod}]-[{requestPath}] request with id: {id} and role: {role}",
                    context.Request.Method, context.Request.Path, id, role);
            }
            else
            {
                _logger.LogInformation("Incoming not authenticated request.");
            }

            await _next(context);
        }
    }
}
