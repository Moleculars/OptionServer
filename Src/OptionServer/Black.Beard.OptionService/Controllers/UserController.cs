using Bb.Exceptions;
using Bb.OptionService.Models;
using Bb.Security.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            try
            {
                UserEntity auth = _service.AddUser(user.Login, user.Password, user.Mail, user.Pseudo);
                if (auth != null)
                    return Ok(new RootResultModel<UserCreatedResultModel>()
                    {

                        Valid = true,

                        Datas = new UserCreatedResultModel()
                        {
                            Id = auth.Id,
                            Username = auth.Username,
                        }

                    });

                else
                    return Ok(
                        new RootResultModel<UserCreatedResultModel>()
                        {
                            Valid = false,
                            Message = "failed to create user.",
                        });

            }
            catch (AllreadyExistException)
            {
                return new RootResultModel<UserCreatedResultModel>()
                {
                    Valid = false,
                    Message = "user allready exist.",
                };

            }

        }

        [HttpPost("connect", Name = "user.Login")]
        public ActionResult<string> UserConnect([FromBody]LoginInputModel user)
        {

            if (!string.IsNullOrEmpty(user.Login))
            {

                UserEntity auth = _service.User(user.Login);

                if (auth == null)
                    return Unauthorized();

                var hash = UserEntity.Hash(user.Password);
                if (auth.HashPassword != hash)
                    return Unauthorized();

                var token = new JwtTokenManager(_tokenConfiguration)
                                    .AddMail(auth.Email)
                                    .AddPseudo(auth.Pseudo)
                                    .AddSubject(user.Login)
                                    .AddExpiry(60)
                                    .Build();

                return Ok(token);

            }

            return Unauthorized();

        }

        private readonly OptionServices _service;
        private readonly JwtTokenConfiguration _tokenConfiguration;
    }



}
