namespace Bb.OptionService.Models
{


    public class GrantModel
    {
        public string User { get; set; }

        public string GroupName { get; set; }

        public AccessModuleEnum AccessApplication { get; set; }

        public AccessModuleEnum AccessType { get; set; }

        public AccessModuleEnum AccessEnvironment { get; set; }

    }


    public enum AccessModuleEnum
    {

        /// <summary>
        /// No access
        /// </summary>
        None = 0,

        /// <summary>
        /// Can read
        /// </summary>
        Read = 1,

        /// <summary>
        /// can add type item
        /// </summary>
        Write = 2 | Read,

        /// <summary>
        /// can add type item
        /// </summary>
        Add = 4 | Write,

        /// <summary>
        /// Can delete group
        /// </summary>
        Delete = 8,

        /// <summary>
        /// is owner
        /// </summary>
        Admin = Read | Add | Write | Delete,

    }

}