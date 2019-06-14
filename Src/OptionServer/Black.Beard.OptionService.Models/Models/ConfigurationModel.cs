using System.ComponentModel.DataAnnotations;

namespace Bb.OptionService.Models
{
    public class ConfigurationModel
    {

        [Required]
        public string GroupName { get; set; }

        [Required]
        public string ApplicationName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Datas { get; set; }



        [Required]
        public string Environment { get; set; }

        [Required]
        public string Type { get; set; }


    }


}
