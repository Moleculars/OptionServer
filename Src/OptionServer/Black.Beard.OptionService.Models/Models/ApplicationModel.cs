using System.ComponentModel.DataAnnotations;

namespace Bb.OptionService.Models
{

    public class ApplicationModel
    {

        [Required]
        public string GroupName { get; set; }

        [Required]
        public string ApplicationName { get; set; }

    }


}
