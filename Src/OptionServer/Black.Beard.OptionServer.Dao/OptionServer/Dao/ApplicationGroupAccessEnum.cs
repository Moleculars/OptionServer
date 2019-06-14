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
        Reader = 1,

        /// <summary>
        /// can add type item
        /// </summary>
        Operator = 2 | Reader,

        /// <summary>
        /// Can delete item
        /// </summary>
        Manager = 4 | Operator,

        /// <summary>
        /// is owner
        /// </summary>
        Owner = 8 | Manager,

    }

}
