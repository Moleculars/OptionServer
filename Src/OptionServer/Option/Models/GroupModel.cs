using Bb.OptionService.Models;

namespace Bb.Option.Models
{

    public class GroupModel
    {

        public GroupModel(GroupApplicationResult result)
        {
            var c = result.Access[0];
            Group_Name = result.ApplicationGroupName;
            Username = string.Join(',', result.Owner);

            Application_Accesses = string.Join(',', c.ApplicationAccesses);
            Environment_Accesses = string.Join(',', c.EnvironmentAccesses);
            Type_Accesses = string.Join(',', c.TypeAccesses);

            if (string.IsNullOrEmpty(Application_Accesses))
                Application_Accesses = "None";

            if (string.IsNullOrEmpty(Environment_Accesses))
                Environment_Accesses = "None";

            if (string.IsNullOrEmpty(Type_Accesses))
                Type_Accesses = "None";

        }

        public string Group_Name { get; internal set; }

        public string Username { get; internal set; }

        public string Application_Accesses { get; internal set; }

        public string Environment_Accesses { get; internal set; }

        public string Type_Accesses { get; internal set; }

    }
}
