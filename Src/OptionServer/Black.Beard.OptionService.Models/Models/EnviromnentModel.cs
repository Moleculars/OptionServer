using System.ComponentModel.DataAnnotations;

namespace Bb.OptionService.Models
{

    public class EnviromnentModel
    {

        [Required]
        public string EnvironmentName { get; set; }

        [Required]
        public string Groupname { get; set; }

    }


}
