namespace Bb.OptionServer.Entities
{
    public enum UserProfileEnum
    {
        Classical = 0,
        Premium = 8,
        Operator = 256,
        Administrator = 4096 | Premium | Operator,
    }

}
