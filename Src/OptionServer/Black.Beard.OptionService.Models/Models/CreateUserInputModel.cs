using System.ComponentModel.DataAnnotations;

namespace Bb.OptionService.Models
{

    public class CreateUserInputModel
    {

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Mail { get; set; }

        public string Pseudo { get; set; }

    }


}
