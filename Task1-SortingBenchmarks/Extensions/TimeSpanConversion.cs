namespace ADS_A2.Extensions;

public static class TimeSpanConversion
{
    // This method converts a TimeSpan object to a human-readable string
    // It is an extension method that can be called on any TimeSpan object 
    // within this namespace. it is declared using the 'this' keyword (extending an instance of the TimeSpan class)
    // as a static method in a static class. This makes it convenient to use throughout
    // the Benchmarks namespace without having to pass the TimeSpan object as an argument
    // because the units of time fluctuate. 
    
    public static string ToReadableString(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalMilliseconds < 1)
        {
            if (timeSpan.Ticks < 1000)
            {
                // need to use ticks to get the nanoseconds as TimeSpan does
                // not have a Nanoseconds property (1 tick = 100 nanoseconds)
                double nanoseconds = timeSpan.Ticks * 100;
                return $"{nanoseconds:F2}ns";
            }
            // if the time is less than 1 millisecond but greater than 1 microsecond:
            // 1 microsecond = ticks / 10
            // Skeet, J. (2009, July 30). Answer to “C# time in microseconds” [Online post]. Stack Overflow. https://stackoverflow.com/a/1206392
            double microseconds = timeSpan.Ticks / 10;
            return $"{microseconds}μs";
        }
        else if (timeSpan.TotalSeconds < 1)
        {
            return $"{timeSpan.TotalMilliseconds:F2}ms";
        }
        else if (timeSpan.TotalMinutes < 1)
        {
            return $"{timeSpan.TotalSeconds:F2}s";
        }
        else
        {
            return $"{timeSpan.TotalMinutes:F2}min";
        }
    }
}