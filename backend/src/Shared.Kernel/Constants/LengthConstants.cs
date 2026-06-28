namespace Shared.Kernel.Constants;

public static class LengthConstants
{
    public static class Address
    {
        public const int MAX_LENGTH = 250;
    }
    
    public static class Description
    {
        public const int MAX_LENGTH = 350;
    }
    
    public static class Identifier
    {
        public const int MIN_LENGTH = 2;
        public const int MAX_LENGTH = 35;
    }

    public static class Name
    {
        public const int MIN_LENGTH = 3;
        public const int MAX_LENGTH = 150;
    }
}