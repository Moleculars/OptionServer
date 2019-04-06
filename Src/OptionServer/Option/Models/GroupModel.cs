using Bb.OptionService.Models;

namespace Bb.Option.Models
{

    public class GroupModel
    {

        public GroupModel(GroupApplicationResult result)
        {
            var c = result.Accesses[0];
            Group_Name = result.ApplicationGroupName;
            Username = string.Join(',', c.Username);
            Application_Accesses = string.Join(',', c.ApplicationAccesses);
            Environment_Accesses = string.Join(',', c.EnvironmentAccesses);
            Type_Accesses = string.Join(',', c.TypeAccesses);
        }

        public string Group_Name { get; internal set; }

        public string Username { get; internal set; }

        public string Application_Accesses { get; internal set; }

        public string Environment_Accesses { get; internal set; }

        public string Type_Accesses { get; internal set; }

    }
}
