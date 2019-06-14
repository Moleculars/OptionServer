using Bb.OptionServer;
using Bb.OptionServer.Exceptions;
using Bb.OptionService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationController : ControllerBase
    {

        public ApplicationController(OptionServices service)
        {
            _service = service;
        }

        [HttpGet("add/{applicationName}", Name = "application.create")]
        public ActionResult<RootResultModel<ApplicationModel>> Create(string groupName, string applicationName)
        {

            ApplicationModel execute(ControllerBase self, string username)
            {

                var user = _service.User(username);
                var application = _service.AddApplication(user, groupName, applicationName);

                return new ApplicationModel()
                {
                    ApplicationName = applicationName,
                    GroupName = application.Group.FullName,
                };

            }

            return this.Execute(execute, true);

        }

        [HttpGet("list/{groupName}", Name = "application.list")]
        public ActionResult<RootResultModel<List<ApplicationModel>>> List(string groupName)
        {

            List<ApplicationModel> execute(ControllerBase self, string username)
            {

                var user = _service.User(username);

                var appli = _service.GetApplications(user, groupName);
                var result = appli.Select(c => new ApplicationModel() { GroupName = c.Group.FullName, ApplicationName = c.Name }).ToList();
                return result;
            }

            return this.Execute(execute, true);

        }

        private readonly OptionServices _service;

    }



}
