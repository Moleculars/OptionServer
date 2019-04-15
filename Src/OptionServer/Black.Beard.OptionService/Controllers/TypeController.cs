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

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var env = _service.AddType(username, model.Groupname, model.TypeName, model.Extension, model.Validator);

                if (env != null)
                    return Ok(new RootResultModel<TypeModel>()
                    {
                        Valid = true,
                        Datas = new TypeModel()
                        {
                            Groupname = model.Groupname,
                            TypeName = env.Name,
                            Extension = env.Extension,
                            Validator = env.Version.Contract,
                            Sha256 = env.Version.Sha256,
                            Version = env.Version.Version,
                        }
                    });

                else
                    return BadRequest(
                        new RootResultModel<TypeModel>()
                        {
                            Valid = false,
                            Message = "failed to create environment.",
                        });

            }
            catch (AllreadyExistException)
            {
                return BadRequest(
                    new RootResultModel<TypeModel>()
                    {
                        Valid = false,
                        Message = "environment allready exist.",
                    }
                    );

            }

        }

        [HttpPost("updateContract", Name = "type.updateContract")]
        public ActionResult<RootResultModel<TypeModel>> UpdateContract([FromBody]TypeModel model)
        {

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var env = _service.UpdateContract(username, model.Groupname, model.TypeName, model.Validator);

                if (env != null)
                    return Ok(new RootResultModel<TypeModel>()
                    {
                        Valid = true,
                        Datas = new TypeModel()
                        {
                            Groupname = model.Groupname,
                            TypeName = env.Name,
                            Extension = env.Extension,
                            Validator = env.Version.Contract,
                            Version = env.Version.Version,
                        }
                    });

                else
                    return BadRequest(
                        new RootResultModel<TypeModel>()
                        {
                            Valid = false,
                            Message = "failed to create environment.",
                        });

            }
            catch (AllreadyExistException)
            {
                return BadRequest(
                    new RootResultModel<TypeModel>()
                    {
                        Valid = false,
                        Message = "environment allready exist.",
                    }
                    );

            }

        }

        [HttpPost("updateExtension", Name = "type.updateExtension")]
        public ActionResult<RootResultModel<TypeModel>> UpdateExtension([FromBody]TypeModel model)
        {

            model.Extension = model.Extension.Trim('.').ToLower();

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var env = _service.UpdateExtension(username, model.Groupname, model.TypeName, model.Extension);

                if (env != null)
                    return Ok(new RootResultModel<TypeModel>()
                    {
                        Valid = true,
                        Datas = new TypeModel()
                        {
                            Groupname = model.Groupname,
                            TypeName = env.Name,
                            Extension = env.Extension,
                            Validator = env.Version.Contract,
                            Version = env.Version.Version,
                        }
                    });

                else
                    return BadRequest(
                        new RootResultModel<TypeModel>()
                        {
                            Valid = false,
                            Message = "failed to create environment.",
                        });

            }
            catch (AllreadyExistException)
            {
                return BadRequest(
                    new RootResultModel<TypeModel>()
                    {
                        Valid = false,
                        Message = "environment allready exist.",
                    }
                    );

            }

        }

        [HttpGet("list/{groupName}", Name = "type.list")]
        public ActionResult<RootResultModel<List<TypeModel>>> List(string groupName)
        {

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var env = _service.GetTypes(username, groupName);

                if (env != null)
                    return Ok(new RootResultModel<List<TypeModel>>()
                    {
                        Valid = true,
                        Datas = env.Select(c => new TypeModel()
                        {
                            Groupname = groupName,
                            TypeName = c.Name,
                            Extension = c.Extension,
                            Validator = c.Version.Contract,
                            Version = c.Version.Version,
                            Sha256 = c.Version.Sha256,
                        }).ToList()
                    });

                else
                    return BadRequest(
                        new RootResultModel<List<TypeModel>>()
                        {
                            Valid = false,
                            Message = "failed to get environments.",
                        });

            }
            catch (AllreadyExistException)
            {
                return BadRequest
                    (
                        new RootResultModel<List<TypeModel>>()
                        {
                            Valid = false,
                            Message = "failed to get environments.",
                        }
                    );

            }



        }

        [HttpGet("get/{groupName}/{typeName}", Name = "type.get")]
        public ActionResult<RootResultModel<TypeModel>> Get(string groupName, string typeName)
        {

            if (!ModelState.IsValid)
                return base.BadRequest(base.ModelState);

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var type = _service.GetType(username, groupName, typeName);

                if (type != null)
                    return Ok(new RootResultModel<TypeModel>()
                    {
                        Valid = true,
                        Datas =  new TypeModel()
                        {
                            Groupname = groupName,
                            TypeName = type.Name,
                            Extension = type.Extension,
                            Validator = type.Version.Contract,
                            Version = type.Version.Version,
                        }
                    });

                else
                    return BadRequest(
                        new RootResultModel<TypeModel>()
                        {
                            Valid = false,
                            Message = "failed to get environments.",
                        });

            }
            catch (AllreadyExistException)
            {
                return BadRequest
                    (
                        new RootResultModel<TypeModel>()
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
