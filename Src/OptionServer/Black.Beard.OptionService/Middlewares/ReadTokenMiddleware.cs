using Bb.Security.Jwt;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Bb.Middlewares
{
    public class ReadTokenMiddleware
    {

        public ReadTokenMiddleware(RequestDelegate next, JwtTokenConfiguration tokenConfiguration)
        {
            _next = next;
            _jwtTokenManager = new JwtTokenManager(tokenConfiguration);
        }

        public async Task Invoke(HttpContext context)
        {

            if (context.Request.Headers.ContainsKey("authorization"))
            {
                var tokenText = context.Request.Headers["authorization"];
                if (!string.IsNullOrEmpty(tokenText))
                    context.User = _jwtTokenManager.ValidToken(tokenText);
            }

            await _next(context);

        }

        private readonly RequestDelegate _next;
        private readonly JwtTokenManager _jwtTokenManager;

    }

}
