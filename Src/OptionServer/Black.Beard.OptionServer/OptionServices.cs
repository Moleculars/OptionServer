using Bb.OptionServer;
using System.Linq;

namespace Bb
{

    public partial class OptionServices
    {

        public OptionServices(SqlManager manager)
        {
            _manager = manager;
        }


        public ApplicationGroupAccessEntity CheckGroup(string username, string groupName, AccessEntityEnum access, objectKingEnum objectKind)
        {

            var group = GroupApplication(username, groupName)
                .FirstOrDefault();

            if (group == null)
                throw new Exceptions.InvalidValueException($"{nameof(username)} or {nameof(groupName)}");

            AccessEntityEnum _access = AccessEntityEnum.None;

            switch (objectKind)
            {

                case objectKingEnum.Environment:
                    _access = group.EnvironmentAccess;
                    break;

                case objectKingEnum.Type:
                    _access = group.TypeAccess;
                    break;

                case objectKingEnum.Application:
                    _access = group.ApplicationAccess;
                    break;

                default:
                    _access = AccessEntityEnum.None;
                    break;

            }

            if (!_access.CanDoIt(access))
                throw new Exceptions.NotEnoughtRightException($"{nameof(username)} have not enought right for {access} {objectKind}");
            
            return group;

        }

        private readonly SqlManager _manager;

    }


    public enum objectKingEnum
    {
        Environment,
        Type,
        Application,
    }

}
