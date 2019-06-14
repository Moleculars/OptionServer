using Bb.OptionServer;
using Bb.OptionServer.Repositories.Tables;
using Bb.OptionService.Models;
using Bb.Security.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Bb.Controllers
{

    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {

        public UserController(OptionServices service, JwtTokenConfiguration tokenConfiguration)
        {
            _service = service;
            _tokenConfiguration = tokenConfiguration;
        }

        [HttpPost("add", Name = "user.create")]
        public ActionResult<RootResultModel<UserCreatedResultModel>> Create([FromBody]CreateUserInputModel user)
        {

            UserCreatedResultModel execute(ControllerBase self, string username)
            {

                UsersTable auth = _service.AddUser(user.Login, user.Password, user.Mail, user.Pseudo);
                var result = new UserCreatedResultModel()
                {
                    Id = auth.Id,
                    Username = auth.Username,
                };

                return result;

            }

            return this.Execute(execute, false);

        }

        [HttpPost("connect", Name = "user.Login")]
        public ActionResult<RootResultModel<string>> UserConnect([FromBody]LoginInputModel user)
        {

            string execute(ControllerBase self, string username)
            {

                if (!string.IsNullOrEmpty(user.Login))
                {

                    var u = _service.Authenticate(user.Login, user.Password);
                    string token = _service.GetToken(u, _tokenConfiguration);

                    return token;

                }

                throw new AuthenticationException();

            }

            return this.Execute(execute, false);

        }



        private readonly OptionServices _service;
        private readonly JwtTokenConfiguration _tokenConfiguration;
    }



}
