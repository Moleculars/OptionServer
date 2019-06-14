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

                var user = _service.User(username);

                var type = _service.AddType(user, model.Groupname, model.TypeName, model.Extension, model.Validator);

                var result = new TypeModel()
                {
                    Groupname = type.Group.FullName,
                    TypeName = type.TypeName,
                    Extension = type.Extension,
                    Validator = type.CurrentVersion.Contract,
                    Sha256 = type.CurrentVersion.Sha256,
                    Version = type.CurrentVersion.Version,
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

                var user = _service.User(username);

                var type = _service.UpdateContract(user, model.Groupname, model.TypeName, model.Validator);
                var result = new TypeModel()
                {
                    Groupname = type.Group.FullName,
                    TypeName = type.TypeName,
                    Extension = type.Extension,
                    Validator = type.CurrentVersion.Contract,
                    Version = type.CurrentVersion.Version,
                    Sha256 = type.CurrentVersion.Sha256,
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

                var user = _service.User(username);

                var type = _service.UpdateExtension(user, model.Groupname, model.TypeName, model.Extension);
                var result = new TypeModel()
                {
                    Groupname = type.Group.FullName,
                    TypeName = type.TypeName,
                    Extension = type.Extension,
                    Validator = type.CurrentVersion.Contract,
                    Version = type.CurrentVersion.Version,
                    Sha256 = type.CurrentVersion.Sha256,
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

                var user = _service.User(username);
                var group = user.ResolveGroup(groupName).Group.Infos;
                var types = group.GetTypes();
                var result = types.Select(type => new TypeModel()
                {
                    Groupname = type.Group.FullName,
                    TypeName = type.TypeName,
                    Extension = type.Extension,
                    Validator = type.CurrentVersion.Contract,
                    Version = type.CurrentVersion.Version,
                    Sha256 = type.CurrentVersion.Sha256,
                }).ToList();

                return result;

            }

            return this.Execute(execute, true);

        }

        [HttpGet("get/{groupName}/{typeName}", Name = "type.getByName")]
        public ActionResult<RootResultModel<TypeModel>> Get(string groupName, string typeName)
        {

            TypeModel execute(ControllerBase self, string username)
            {

                var user = _service.User(username);

                var type = _service.GetType(user, groupName, typeName);
                var result = new TypeModel()
                {
                    Groupname = type.Group.FullName,
                    TypeName = type.TypeName,
                    Extension = type.Extension,
                    Validator = type.CurrentVersion.Contract,
                    Version = type.CurrentVersion.Version,
                    Sha256 = type.CurrentVersion.Sha256,
                };

                return result;
            }

            return this.Execute(execute, true);

        }

        [HttpGet("extension/{groupName}/{typeName}", Name = "type.getByExtension")]
        public ActionResult<RootResultModel<List<TypeModel>>> GetByExtension(string groupName, string extension)
        {

            if (!extension.StartsWith('.'))
                extension = "." + extension;

            List<TypeModel> execute(ControllerBase self, string username)
            {

                var user = _service.User(username);
                var group = user.ResolveGroup(groupName).Group.Infos;
                var types = group.GetTypes();
                
                var result = types
                    .Where(c => c.Extension == extension)
                    .Select(type => new TypeModel()
                    {
                        Groupname = type.Group.FullName,
                        TypeName = type.TypeName,
                        Extension = type.Extension,
                        Validator = type.CurrentVersion.Contract,
                        Version = type.CurrentVersion.Version,
                        Sha256 = type.CurrentVersion.Sha256,
                    }).ToList();

                return result;

            }

            return this.Execute(execute, true);

        }

        private readonly OptionServices _service;

    }



}
