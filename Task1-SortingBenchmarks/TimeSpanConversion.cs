namespace Benchmarks;

public static class TimeSpanConversion
{
    // This method converts a TimeSpan object to a human-readable string
    // It is an extension method that can be called on any TimeSpan object 
    // within the Benchmarks namespace. it is declared using the 'this' keyword
    // as a static method in a static class. This makes it convenient to use throughout
    // the Benchmarks namespace without having to pass the TimeSpan object as an argument
    // because the units of time fluctuate
    //
    public static string ToReadableString(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalMilliseconds < 1)
        {
            // need to use ticks to get the nanoseconds as TimeSpan does
            // not have a Nanoseconds property (1 tick = 100 nanoseconds)
            double nanoseconds = timeSpan.Ticks * 100;
            return $"{nanoseconds:F2} ns";
        }
        else if (timeSpan.TotalSeconds < 1)
        {
            return $"{timeSpan.TotalMilliseconds:F2} ms";
        }
        else if (timeSpan.TotalMinutes < 1)
        {
            return $"{timeSpan.TotalSeconds:F2} s";
        }
        else
        {
            return $"{timeSpan.TotalMinutes:F2} min";
        }
    }
}