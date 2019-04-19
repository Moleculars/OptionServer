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
        Add = 4 | Read | Write,

        /// <summary>
        /// Can delete item
        /// </summary>
        Delete = 8 | Add,

        /// <summary>
        /// is owner
        /// </summary>
        Admin = 16 | Read | Add | Write | Delete,

    }

}
