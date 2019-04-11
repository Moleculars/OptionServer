using Bb.Exceptions;
using Bb.OptionService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class EnvironmentController : ControllerBase
    {

        public EnvironmentController(OptionServices service)
        {
            _service = service;
        }

        [HttpPost("add", Name = "environment.create")]
        public ActionResult<RootResultModel<EnviromnentModel>> Create([FromBody]EnviromnentModel model)
        {

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var env = _service.AddEnvironment(username, model.Groupname, model.EnvironmentName);

                if (env != null)
                    return Ok(new RootResultModel<EnviromnentModel>()
                    {
                        Valid = true,
                        Datas = model
                    });

                else
                    return BadRequest(
                        new RootResultModel<EnviromnentModel>()
                        {
                            Valid = false,
                            Message = "failed to create environment.",
                        });

            }
            catch (AllreadyExistException)
            {
                return BadRequest(
                    new RootResultModel<EnviromnentModel>()
                    {
                        Valid = false,
                        Message = "environment allready exist.",
                    }
                    );

            }

        }

        [HttpGet("list/{groupName}", Name = "environment.list")]
        public ActionResult<RootResultModel<List<EnviromnentModel>>> List(string groupName)
        {

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var env = _service.GetEnvironments(username, groupName);

                if (env != null)
                    return Ok(new RootResultModel<List<EnviromnentModel>>()
                    {
                        Valid = true,
                        Datas = env.Select(c => new EnviromnentModel() { Groupname = groupName, EnvironmentName = c.Name }).ToList()
                    });

                else
                    return BadRequest(
                        new RootResultModel<List<EnviromnentModel>>()
                        {
                            Valid = false,
                            Message = "failed to get environments.",
                        });

            }
            catch (AllreadyExistException)
            {
                return BadRequest
                    (
                        new RootResultModel<List<EnviromnentModel>>()
                        {
                            Valid = false,
                            Message = "failed to get environments.",
                        }
                    );

            }



        }


        private string GetUsername()
        {
            return User?.Identity.Name;
        }

        private readonly OptionServices _service;

    }



}
