using System;
using System.Collections.Generic;

namespace Bb.OptionService.Models
{

    public class GroupApplicationResult
    {

        public GroupApplicationResult()
        {
            this.Accesses = new List<AccessResult>();
        }

        public Guid ApplicationGroupId { get; set; }

        public string ApplicationGroupName { get; set; }

        public List<AccessResult> Accesses { get; set; }

    }


}
