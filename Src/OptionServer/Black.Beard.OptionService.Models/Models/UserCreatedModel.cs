using System;

namespace Bb.OptionService.Models
{
    public class UserCreatedResultModel
    {

        public bool Valid { get; set; }

        public Guid Id { get; set; }

        public string Result { get; set; }

        public string Username { get; set; }

        public string Pseudo { get; set; }

    }


}
