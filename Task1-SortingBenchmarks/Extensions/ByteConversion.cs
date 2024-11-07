namespace ADS_A2.Extensions;

public static class ByteSizeConversion
{
    public static string ToReadableSize(this long bytes)
    {
        // This method converts a byte array to a human-readable string
        // It is an extension method that can be called on any byte array object 
        // within the ADS_A2.Extensions namespace. it is declared using the 'this' keyword
        // as a static method in a static class. This makes it convenient to use throughout
        // the ADS_A2.Extensions namespace without having to pass the byte array object as an argument
        // because the units of bytes fluctuate

        if (bytes< 1024)
        {
            return $"{bytes} bytes";
        }
        else if (bytes< 1024 * 1024)
        {
            return $"{bytes/ 1024} KB";
        }
        else if (bytes< 1024 * 1024 * 1024)
        {
            return $"{bytes/ (1024 * 1024)} MB";
        }
        else
        {
            return $"{bytes/ (1024 * 1024 * 1024)} GB";
        }
    }
}