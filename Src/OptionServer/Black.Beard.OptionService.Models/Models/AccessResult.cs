using System;

namespace Bb.OptionService.Models
{
    public class AccessResult
    {

        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string[] ApplicationAccesses { get; set; }

        public string[] EnvironmentAccesses { get; set; }

        public string[] TypeAccesses { get; set; }

    }
}