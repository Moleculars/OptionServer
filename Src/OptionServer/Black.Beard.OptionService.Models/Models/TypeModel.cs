using System.ComponentModel.DataAnnotations;

namespace Bb.OptionService.Models
{
    public class TypeModel
    {

        [Required]
        public string Groupname { get; set; }

        [Required]
        public string TypeName { get; set; }

        [Required]
        public string Extension { get; set; }

        public string Validator { get; set; }

        public int Version { get; set; }

    }


}
