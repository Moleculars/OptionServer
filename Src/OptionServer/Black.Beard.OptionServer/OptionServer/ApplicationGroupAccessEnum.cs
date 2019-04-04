namespace Bb.OptionServer
{

    public enum AccessEntityEnum
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
