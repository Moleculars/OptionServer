using Bb.OptionServer;
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

            EnviromnentModel execute(ControllerBase self, string username)
            {
                var user = _service.User(username);
                _service.AddEnvironment(user, model.Groupname, model.EnvironmentName);
                return model;
            }

            return this.Execute(execute, true);

        }

        [HttpGet("list/{groupName}", Name = "environment.list")]
        public ActionResult<RootResultModel<List<EnviromnentModel>>> List(string groupName)
        {

            List<EnviromnentModel> execute(ControllerBase self, string username)
            {
                var user = _service.User(username);
                var group = _service.LoadEnvironments(user, groupName);
                var result = group.GetEnvironments().Select(c => 
                new EnviromnentModel()
                {
                    Groupname = group.FullName,
                    EnvironmentName = c.EnvironmentName
                }).ToList();
                return result;
            }

            return this.Execute(execute, true);

        }

        private readonly OptionServices _service;

    }



}
