using System.Diagnostics;
using ADS_A2.Algorithms;
using ADS_A2.Extensions;
using ADS_A2.input;
using Benchmarks;

namespace ADS_A2.Benchmarker;

public static class Benchmark
{
    public static List<BenchmarkResult> Run(InputFile file1, InputFile file2, List<(AlgorithmType Type, Action<InputFile> Algorithm)> algorithms)
    {
        // overload method to accept two InputFile objects instead of a list
        List<InputFile> files = new() { file1, file2 };
        
        return Run(files, algorithms);
    }
    
    public static List<BenchmarkResult> Run(List<InputFile> files, List<(AlgorithmType Type, Action<InputFile> Algorithm)> algorithms)
    // runs and returns the results of the benchmark in a dictionary with the InputFile object as the key
    // and the time taken to sort the array as the value with the algorithm name as the key
    {
        // initialize variables to store the results and the time taken to sort the arrays
        List<BenchmarkResult> results = new(); 
        
        foreach (InputFile file in files)
        {
            // polymorphic call the different algorithms method on the InputFile object, performing different algorithms
            // depending on the object type, then storing the benchmark results to the dictionary for reference
            
            foreach (var (name, algorithm) in algorithms)
            {
                // using an anonymous function to pass the algorithm as a parameter to the MeasureAlgorithm method
                // for each different algorithm, measure the time taken to sort the array
                results.Add(MeasureAlgorithm(name, file, () => algorithm(file)));
            }
        }

        // Check if the arrays are sorted correctly by comparing the respective files sorted arrays of each algorithm
        // this will ensure that the algorithms are working correctly as each algorithm will have the same output
        foreach (var fileGroup in files.GroupBy(k => k.FileName))
        {
            var originalLists = fileGroup.Select(k => k.Contents).ToList();
            var sortedLists = fileGroup.Select(k => k.Numbers).ToList();
            bool allEqual = sortedLists.All(n => n.SequenceEqual(sortedLists[0]));
            
            if(allEqual)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Success: {fileGroup.Key} all sorted arrays are identical. First 5 elements:");
                // print the first 5 elements of the sorted arrays to verify the correctness, joining the elements with a comma
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach(var list in sortedLists)
                    Console.Write($"Sorted: {string.Join(", ", list[0..5])}... ");
                
                Console.WriteLine();
                foreach (var list in originalLists)
                    Console.Write($"Original: {string.Join(", ", list[0..5])}... ");
                
                Console.WriteLine("\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failure: Sorting algorithms for {fileGroup.Key} produce different results.");
            }
            Console.ResetColor();
        }
        
        return results;
    }


    private static BenchmarkResult MeasureAlgorithm(AlgorithmType algorithmName, InputFile inputFile, Action runAlgorithm)
    {
        Stopwatch sw = new();
        TimeSpan duration;

        // method to measure and run the bubble sort algorithm on the input file with memory usage
        BenchmarkResult RunAndMeasure()
        {
            // Copy the contents of the file to an array of integers to be sorted and convert to integers.
            // this will also remove any bad characters from the file and reset the array for each algorithm
            inputFile.Parse(inputFile.FilePath);
            
            // Because this method is run in a no GC (garbage collection) region, must set the forceFullCollection parameter to false, otherwise
            // the NoGC region will be ended and/or the method will not return until G/Collection occurs. Error will be thrown when trying to end the region
            // dotnet-bot. (n.d.). GC.GetTotalMemory Method (System). Retrieved November 7, 2024, from https://learn.microsoft.com/en-us/dotnet/api/system.gc.gettotalmemory?view=net-8.0
            long initialMemory = GC.GetTotalMemory(false);
            // ^ get the initial memory usage before sorting the array to calculate
            // the memory used by the algorithm
            
            // perform the bubble sort algorithm on the input file
            // timing the duration of the algorithm and storing the memory used
            sw.Start();
            // run the algorithm passed as a parameter (through Action delegate)
            runAlgorithm();
            sw.Stop();
            // get the final memory usage after sorting the array (with no GC interference)
            long finalMemory = GC.GetTotalMemory(false);
            
            duration = sw.Elapsed;
            // calculate the memory used by the algorithm (difference between the initial and final)
            long memoryUsed = finalMemory - initialMemory;
            sw.Reset();
            
            return new BenchmarkResult(memoryUsed, duration, algorithmName, inputFile);
        }

        // prevent the garbage collector from running during the benchmark up to the specified size
        // ensuring memory usage reading is accurate. after 100MB, the garbage collector will step in to avoid memory leaks
        // dotnet-bot. (n.d.). GC.TryStartNoGCRegion Method (System). Retrieved November 7, 2024, from https://learn.microsoft.com/en-us/dotnet/api/system.gc.trystartnogcregion?view=net-8.0
        
        // before starting the no GC region, run the garbage collector to free up memory
        GC.Collect();
        // start a no GC region, if unable to start (returns false), measure only the time taken to sort the array in the else block
        if (GC.TryStartNoGCRegion((1024 * 1024) * 100)) // 100MB (1024 * 1024 = 1MB)
        {
            try
            {
                return RunAndMeasure();
            }
            finally
            {
                // end the no GC region
                GC.EndNoGCRegion();
            }
        }
        else
        {
            // catch no GC region start failure by measuring only the time taken to sort the array
            sw.Start();
            runAlgorithm();
            sw.Stop();
            duration = sw.Elapsed;
            sw.Reset();            
            Console.ForegroundColor = ConsoleColor.Red;
            // show an error message if the no GC region cannot be started
            Console.WriteLine("Error: Could not start a no GC region. No memory usage data will be available.");
            Console.ResetColor();
            
            return new BenchmarkResult(0, duration, algorithmName, inputFile);
        }
    }
}