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
        /// can list
        /// </summary>
        List = 2 | Read,

        /// <summary>
        /// can add type item
        /// </summary>
        Write = 4 | Read,

        /// <summary>
        /// can add type item
        /// </summary>
        Add = 8 | List | Write,

        /// <summary>
        /// Can delete item
        /// </summary>
        Delete = 16 | Add,

        /// <summary>
        /// is owner
        /// </summary>
        Owner = 32 | Read | Add | Write | Delete,

    }


}