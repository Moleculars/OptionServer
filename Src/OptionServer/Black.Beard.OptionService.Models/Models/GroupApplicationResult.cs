using System.Collections.Generic;

namespace Bb.OptionService.Models
{

    public class OwnerResult
    {

        public OwnerResult()
        {
            Groups = new List<GroupApplicationResult>();
        }

        public List<GroupApplicationResult> Groups { get; set; }

        public string Name { get; set; }

    }

    public class GroupApplicationResult
    {

        public GroupApplicationResult()
        {
            Access = new List<AccessResult>();
        }

        public string ApplicationGroupName { get; set; }

        public List<AccessResult> Access { get; }

        public string Owner { get; set; }

    }

    public class AccessResult
    {

        public string[] ApplicationAccesses { get; set; }

        public string[] EnvironmentAccesses { get; set; }

        public string[] TypeAccesses { get; set; }
    }


}
