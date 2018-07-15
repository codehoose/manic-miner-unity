public static class AttributeExtensions
{
    public static int GetInk(this int val)
    {
        return val & 0x07; // The right most three bits
    }

    public static int GetPaper(this int val)
    {
        int temp = val >> 3; // move the value down three (effectively / 8)
        return temp & 0x07; // The right most three bits
    }

    public static byte GetInk(this byte val)
    {
        return (byte)(val & 0x07); // The right most three bits
    }

    public static byte GetPaper(this byte val)
    {
        int temp = val >> 3; // move the value down three (effectively / 8)
        return (byte)(temp & 0x07); // The right most three bits
    }

    public static bool IsFlashing(this int val)
    {
        return (val & 0x80) == 0x80;
    }

    public static bool IsBright(this int val)
    {
        return (val & 0x40) == 0x40;
    }
}
