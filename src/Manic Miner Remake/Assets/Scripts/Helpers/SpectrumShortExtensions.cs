public static class SpectrumShortExtensions
{
    /// <summary>
    /// Get the X value from the short.
    /// </summary>
    /// <param name="s">Short</param>
    /// <returns>X co-ordinate</returns>
    public static int GetX(this short s)
    {
        int temp = s & 0x1f;  // is 11111 in binary
        return temp;
    }

    /// <summary>
    /// Get the Y value from the short.
    /// </summary>
    /// <param name="s">Short</param>   
    /// <returns>Y co-ordinate</returns>
    public static int GetY(this short s)
    {
        int temp = s;
        temp = temp >> 5;
        temp = temp & 0x0f;
        return temp;

        //int temp = s & 0x01e0; // is 0000 0001 1110 0000
        //temp = temp >> 5;
    }
}
