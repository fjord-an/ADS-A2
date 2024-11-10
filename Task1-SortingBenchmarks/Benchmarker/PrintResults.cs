using ADS_A2.Algorithms;
using ADS_A2.Extensions;

namespace ADS_A2.Benchmarker;

public static class PrintResults
{
    public static void Display(List<BenchmarkResult> results)
    {

        // #print the results
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Results:");
        Console.ResetColor();

        foreach (BenchmarkResult result in results)
        {
            // iterate through the results dictionary and print the time taken to sort the arrays for each file
            // print each individual algorithms performance of the InputFile object
            // By using my custom ToReadableString() extension method, the time taken to sort the arrays
            // is printed in a human-readable format

            // NOTE: if memory usage is reading 0, then memory usage is negligable. Such low amounts of memory
            // usage cannot be accurately measured using the GC.GetTotalMemory() method. Such is the case when
            // the optimised partially sorted array is sorted, as only 1 swap is made. Very little memory is used and hard to measure.
            string algorithmType = result.FileProperties.GetType().Name == "OptimisedAlgorithms"
                ? "(Optimised)"
                : "(Basic)";

            Console.Write($"{result.AlgorithmName} {algorithmType} took ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{result.ExecutionTime.ToReadableString()} ");
            Console.ResetColor();
            Console.WriteLine($"to sort {result.FileProperties.FileName}");
            Console.ForegroundColor = ConsoleColor.Gray;
            // print the memory usage of the algorithm (and if 0: then usage was too low to measure) often the 
            // GC.GetTotalMemory() method will round down to 0 when the memory usage is very low (under 4096 bytes)
            // cyberj0g. (2015, July 23). Answer to “GC.GetTotalMemory minimal resolution” [Online post]. Stack Overflow. https://stackoverflow.com/a/31581701
            Console.WriteLine(result.MemoryUsage == 0
                ? "Memory usage: Negligible/Unmeasurable" //else print the memory usage in bytes:
                : $"Memory usage: {result.MemoryUsage.ToReadableString()}");
            Console.ResetColor();
        }

        CompareAlgorithms(results);
    }

    private static void CompareAlgorithms(List<BenchmarkResult> results)
    {
        // Group results by file name
        var resultsByFile = results.GroupBy(r => r.FileProperties.FileName);

        foreach (var fileGroup in resultsByFile)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\nComparison Results for {fileGroup.Key}:");
            Console.ResetColor();

            // Filter results for BubbleSort and MergeSort within the file group
            var bubbleSortResults = fileGroup.Where(r => r.AlgorithmName == AlgorithmType.BubbleSort).ToList();
            var mergeSortResults = fileGroup.Where(r => r.AlgorithmName == AlgorithmType.MergeSort).ToList();

            // Find the best BubbleSort result (fastest and least memory usage)
            var bestBubbleSortTime = bubbleSortResults.OrderBy(r => r.ExecutionTime).FirstOrDefault();
            var bestBubbleSortMemory = bubbleSortResults.OrderBy(r => r.MemoryUsage).FirstOrDefault();

            // Find the best MergeSort result (fastest and least memory usage)
            var bestMergeSortTime = mergeSortResults.OrderBy(r => r.ExecutionTime).FirstOrDefault();
            var bestMergeSortMemory = mergeSortResults.OrderBy(r => r.MemoryUsage).FirstOrDefault();

            // Compare the best BubbleSort and MergeSort results for time
            var fastestAlgorithm = bestBubbleSortTime.ExecutionTime < bestMergeSortTime.ExecutionTime
                ? bestBubbleSortTime
                : bestMergeSortTime;

            // Compare the best BubbleSort and MergeSort results for memory usage
            var leastMemoryAlgorithm = bestBubbleSortMemory.MemoryUsage < bestMergeSortMemory.MemoryUsage
                ? bestBubbleSortMemory
                : bestMergeSortMemory;

            // Print the comparison results for the current file
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(
                $"Fastest Algorithm: {fastestAlgorithm.AlgorithmName} ({fastestAlgorithm.FileProperties.GetType().Name}) with time {fastestAlgorithm.ExecutionTime.ToReadableString()}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(
                $"Least Memory Usage Algorithm: {leastMemoryAlgorithm.AlgorithmName} ({leastMemoryAlgorithm.FileProperties.GetType().Name}) with memory usage {leastMemoryAlgorithm.MemoryUsage.ToReadableString()}");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}