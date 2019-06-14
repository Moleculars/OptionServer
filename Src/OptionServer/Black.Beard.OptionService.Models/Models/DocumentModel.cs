using System.ComponentModel.DataAnnotations;

namespace Bb.OptionService.Models
{
    public class DocumentModel
    {

        [Required]
        public string GroupName { get; set; }

        [Required]
        public string ApplicationName { get; set; }

        [Required]
        public string Name { get; set; }

        public string TypeName { get; set; }

        [Required]
        public string EnvironmentName { get; set; }

        [Required]
        public string Content { get; set; }


    }


}
