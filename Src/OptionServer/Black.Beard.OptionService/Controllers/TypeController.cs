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
    public class TypeController : ControllerBase
    {

        public TypeController(OptionServices service)
        {
            _service = service;
        }

        [HttpPost("add", Name = "type.add")]
        public ActionResult<RootResultModel<TypeModel>> Create([FromBody]TypeModel model)
        {

            model.Extension = model.Extension.Trim('.').ToLower();

            TypeModel execute(ControllerBase self, string username)
            {

                var env = _service.AddType(username, model.Groupname, model.TypeName, model.Extension, model.Validator);

                var result = new TypeModel()
                {
                    Groupname = model.Groupname,
                    TypeName = env.Name,
                    Extension = env.Extension,
                    Validator = env.Version.Contract,
                    Sha256 = env.Version.Sha256,
                    Version = env.Version.Version,
                };

                return result;

            }

            return this.Execute(execute, true);
            
        }

        [HttpPost("updateContract", Name = "type.updateContract")]
        public ActionResult<RootResultModel<TypeModel>> UpdateContract([FromBody]TypeModel model)
        {

            TypeModel execute(ControllerBase self, string username)
            {

                var env = _service.UpdateContract(username, model.Groupname, model.TypeName, model.Validator);
                var result = new TypeModel()
                {
                    Groupname = model.Groupname,
                    TypeName = env.Name,
                    Extension = env.Extension,
                    Validator = env.Version.Contract,
                    Version = env.Version.Version,
                };

                return result;

            }

            return this.Execute(execute, true);

        }

        [HttpPost("updateExtension", Name = "type.updateExtension")]
        public ActionResult<RootResultModel<TypeModel>> UpdateExtension([FromBody]TypeModel model)
        {

            TypeModel execute(ControllerBase self, string username)
            {
                var env = _service.UpdateExtension(username, model.Groupname, model.TypeName, model.Extension);
                var result = new TypeModel()
                {
                    Groupname = model.Groupname,
                    TypeName = env.Name,
                    Extension = env.Extension,
                    Validator = env.Version.Contract,
                    Version = env.Version.Version,
                };

                return result;

            }

            return this.Execute(execute, true);

        }

        [HttpGet("list/{groupName}", Name = "type.list")]
        public ActionResult<RootResultModel<List<TypeModel>>> List(string groupName)
        {

            List<TypeModel> execute(ControllerBase self, string username)
            {

                var env = _service.GetTypes(username, groupName);
                var result = env.Select(c => new TypeModel()
                {
                    Groupname = groupName,
                    TypeName = c.Name,
                    Extension = c.Extension,
                    Validator = c.Version.Contract,
                    Version = c.Version.Version,
                    Sha256 = c.Version.Sha256,
                }).ToList();

                return result;

            }

            return this.Execute(execute, true);

        }

        [HttpGet("get/{groupName}/{typeName}", Name = "type.get")]
        public ActionResult<RootResultModel<TypeModel>> Get(string groupName, string typeName)
        {

            TypeModel execute(ControllerBase self, string username)
            {
                var type = _service.GetType(username, groupName, typeName);
                var result = new TypeModel()
                {
                    Groupname = groupName,
                    TypeName = type.Name,
                    Extension = type.Extension,
                    Validator = type.Version.Contract,
                    Version = type.Version.Version,
                };

                return result;
            }

            return this.Execute(execute, true);

        }

        private readonly OptionServices _service;

    }



}
