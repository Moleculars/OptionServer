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
    public class DocumentController : ControllerBase
    {

        public DocumentController(OptionServices service)
        {
            _service = service;
        }

        [HttpPost("add", Name = "configuration.create")]
        public ActionResult<RootResultModel<DocumentModel>> Create([FromBody]DocumentModel model)
        {

            DocumentModel execute(ControllerBase self, string username)
            {
                var user = _service.User(username);
                var applis = user.Applications(model.GroupName, model.ApplicationName).ToList();
                if (applis.Count == 0)
                    throw new Bb.OptionServer.Exceptions.InvalidNameException(model.ApplicationName);
                else if (applis.Count > 1)
                    throw new Bb.OptionServer.Exceptions.AmbigiousNameException(model.ApplicationName);

                var modelResult = _service.SetDocument(user, applis[0], model.EnvironmentName, model.TypeName, model.Name, model.Content);
                return new DocumentModel()
                {
                    GroupName = modelResult.Application.Group.FullName,
                    ApplicationName = modelResult.Application.Name,
                    Name = modelResult.ConfigurationName,
                    EnvironmentName = modelResult.Environment.EnvironmentName,
                    TypeName = modelResult.Type?.TypeName,
                };
            }

            return this.Execute(execute, true);

        }

        [HttpGet("list/{groupName}/{application}", Name = "configuration.list")]
        public ActionResult<RootResultModel<List<DocumentModel>>> List(string groupName, string applicationName)
        {

            List<DocumentModel> execute(ControllerBase self, string username)
            {
                var user = _service.User(username);
                var applis = user.Applications(groupName, applicationName).ToList();

                if (applis.Count == 0)
                    throw new Bb.OptionServer.Exceptions.InvalidNameException(applicationName);
                else if (applis.Count > 1)
                    throw new Bb.OptionServer.Exceptions.AmbigiousNameException(applicationName);

                _service.LoadDocuments(applis[0], user, false);
                var result = applis[0].Documents.Select(c =>
                new DocumentModel()
                {
                    GroupName = c.Application.Group.FullName,
                    ApplicationName = c.Application.Name,
                    Name = c.ConfigurationName,
                    EnvironmentName = c.Environment.EnvironmentName,
                    TypeName = c.Type?.TypeName,
                }).ToList();
                return result;
            }

            return this.Execute(execute, true);

        }

        private readonly OptionServices _service;

    }



}
