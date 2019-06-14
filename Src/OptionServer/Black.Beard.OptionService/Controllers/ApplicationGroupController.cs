using Bb.Controllers;
using Bb.OptionServer.Entities;
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
        public ActionResult<RootResultModel<OwnerResult>> Create(string groupName)
        {

            OwnerResult execute(ControllerBase self, string username)
            {
                var user = _service.User(username);
                user = _service.CreateGroupApplication(user, groupName);
                OwnerResult result = Convert(user.OwnerAccess.Values.First());

                return result;
            }

            return this.Execute(execute, true);

        }

        /// <summary>
        /// Gets the specified group name with information that user can access.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        [HttpGet("get/{groupName}", Name = "applicationGroup.get")]
        public ActionResult<RootResultModel<GroupApplicationResult>> Get(string groupName)
        {

            GroupApplicationResult execute(ControllerBase self, string username)
            {
                UserEntity user = _service.User(username);
                OptionPath path = user.ResolveGroup(groupName);
                GroupApplicationResult _item = Convert(path.Group);
                return _item;

            }

            return this.Execute(execute, true);

        }

        /// <summary>
        /// return the List of group that user can access 
        /// </summary>
        /// <returns></returns>
        [HttpGet("list", Name = "applicationGroup.list")]
        public ActionResult<RootResultModel<List<OwnerResult>>> List()
        {

            List<OwnerResult> execute(ControllerBase self, string username)
            {

                var user = _service.User(username);
                List<OwnerResult> result = Converts(user.OwnerAccess.Values);
                return result;

            }

            return this.Execute(execute, true);

        }

        /// <summary>
        /// set grant or revoke access to application for specified user.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        [HttpPost("access", Name = "applicationGroup.access")]
        public ActionResult<RootResultModel<List<OwnerResult>>> SetAccess(GrantModel model)
        {

            List<OwnerResult> execute(ControllerBase self, string username)
            {

                var userOwner = _service.User(username);
                var user = _service.User(model.User);

                var userResult = _service.SetAccess(
                    userOwner,
                    user,
                    model.GroupName,
                    (AccessEntityEnum)(int)model.AccessApplication,
                    (AccessEntityEnum)(int)model.AccessType,
                    (AccessEntityEnum)(int)model.AccessEnvironment
                );

                List<OwnerResult> result = Converts(userResult.OwnerAccess.Values);

                return result;

            }

            return this.Execute(execute, true);

        }


        private List<OwnerResult> Converts(Dictionary<string, OwnerEntity>.ValueCollection values)
        {

            return values.Select(c => Convert(c)).ToList();
        }

        private static List<OwnerResult> Convert(IEnumerable<OwnerEntity> owners)
        {
            return owners.Select(c => Convert(c)).ToList();
        }

        private static OwnerResult Convert(OwnerEntity owner)
        {
            return new OwnerResult()
            {
                Name = owner.Pseudo,
                Groups = Converts(owner.Groups.Values),
            };
        }

        private static List<GroupApplicationResult> Converts(IEnumerable<ApplicationGroupPath> groups)
        {
            return groups.Select(c => Convert(c)).ToList();
        }

        private static GroupApplicationResult Convert(ApplicationGroupPath group)
        {
            return Convert(group.Infos);
        }

        private static List<GroupApplicationResult> Converts(IEnumerable<GroupEntity> infos)
        {
            return infos.Select(c => Convert(c)).ToList();
        }

        private static GroupApplicationResult Convert(GroupEntity infos)
        {

            var _group = new GroupApplicationResult()
            {
                ApplicationGroupName = infos.GroupName,
                Owner = infos.Owner.Pseudo,
            };

            _group.Access.Add(new AccessResult()
            {
                ApplicationAccesses = infos.ApplicationAccess.GetPrivilegesToString(),
                EnvironmentAccesses = infos.EnvironmentAccess.GetPrivilegesToString(),
                TypeAccesses = infos.TypeAccess.GetPrivilegesToString(),
            });

            return _group;

        }


        //private static GroupApplicationResult Convert(ApplicationGroupTable group)
        //{

        //    var _group = new GroupApplicationResult()
        //    {
        //        ApplicationGroupName = group.Name,
        //    };

        //    return _group;

        //}

        private readonly OptionServices _service;

    }
}