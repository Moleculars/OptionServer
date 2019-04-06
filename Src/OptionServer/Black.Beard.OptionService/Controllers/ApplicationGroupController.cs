using Bb.OptionServer;
using Bb.OptionService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bb.OptionService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationGroupController : ControllerBase
    {

        public ApplicationGroupController(OptionServices service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new group for logged user.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        [HttpGet("add/{groupName}", Name = "applicationGroup.create")]
        public IActionResult Create(string groupName)
        {

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var groups = _service.CreateGroupApplication(username, groupName);
            List<GroupApplicationResult> _dic = Converts(groups);

            return Ok
                (
                    new RootResultModel<GroupApplicationResult>()
                    {
                        Valid = true,
                        Datas = _dic.FirstOrDefault(),
                    }
                );


        }

        /// <summary>
        /// Gets the specified group name with information that user can access.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        [HttpGet("get/{groupName}", Name = "applicationGroup.get")]
        public IActionResult Get(string groupName)
        {

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var groups = _service.GroupApplication(username, groupName);
            List<GroupApplicationResult> _items = Converts(groups);

            return Ok
            (
                new RootResultModel<GroupApplicationResult>()
                {
                    Valid = true,
                    Datas = _items.FirstOrDefault(),
                }
            );

        }

        /// <summary>
        /// return the List of group that user can access 
        /// </summary>
        /// <returns></returns>
        [HttpGet("list", Name = "applicationGroup.list")]
        public IActionResult List()
        {

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var groups = _service.GetGroupApplicationsForUser(username);
            List<GroupApplicationResult> items = Converts(groups);

            return Ok
            (
                new RootResultModel<List<GroupApplicationResult>>()
                {
                    Valid = true,
                    Datas = items,
                }
            );

        }

        /// <summary>
        /// set grant or revoke access to application for specified user.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        [HttpPost("access", Name = "applicationGroup.access")]
        public IActionResult SetAccess(GrantModel model)
        {

            string username = GetUsername();
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            try
            {

                var groups = _service.SetAccess(
                    username,
                    model.User,
                    model.GroupName,
                    (AccessEntityEnum)(int)model.AccessApplication,
                    (AccessEntityEnum)(int)model.AccessType,
                    (AccessEntityEnum)(int)model.AccessEnvironment
                );

                GroupApplicationResult _items = Convert(groups);

                return Ok
                (
                    new RootResultModel<GroupApplicationResult>()
                    {
                        Valid = true,
                        Datas = _items,
                    }
                );


            }
            catch (System.Exception e)
            {

                var error = new RootResultModel<GroupApplicationResult>()
                {
                    Valid = true,
                    Message = e.Message,
                    Datas = null,
                };

                return BadRequest(error);

            }

        }

        private static List<GroupApplicationResult> Converts(List<ApplicationGroupAccessEntity> groups)
        {

            Dictionary<string, GroupApplicationResult> _dic = new Dictionary<string, GroupApplicationResult>();

            foreach (ApplicationGroupAccessEntity item in groups)
            {

                if (!_dic.TryGetValue(item.ApplicationGroupName, out GroupApplicationResult group))
                    _dic.Add(item.ApplicationGroupName, group = new GroupApplicationResult()
                    {
                        ApplicationGroupId = item.ApplicationGroupId,
                        ApplicationGroupName = item.ApplicationGroupName,
                    });

                group.Accesses.Add(new AccessResult()
                {
                    UserId = item.UserId,
                    Username = item.Username,
                    ApplicationAccesses = item.ApplicationAccess.GetPrivilegesToString(),
                    EnvironmentAccesses = item.EnvironmentAccess.GetPrivilegesToString(),
                    TypeAccesses = item.TypeAccess.GetPrivilegesToString(),
                });

            }

            return _dic.Select(c => c.Value).ToList();

        }

        private static GroupApplicationResult Convert(ApplicationGroupAccessEntity group)
        {

            var _group = new GroupApplicationResult()
            {
                ApplicationGroupId = group.ApplicationGroupId,
                ApplicationGroupName = group.ApplicationGroupName,
            };

            _group.Accesses.Add(new AccessResult()
            {
                UserId = group.UserId,
                Username = group.Username,
                ApplicationAccesses = group.ApplicationAccess.GetPrivilegesToString(),
                EnvironmentAccesses = group.EnvironmentAccess.GetPrivilegesToString(),
                TypeAccesses = group.TypeAccess.GetPrivilegesToString(),
            });

            return _group;

        }


        private string GetUsername()
        {
            return User?.Identity.Name;
        }

        private readonly OptionServices _service;

    }
}