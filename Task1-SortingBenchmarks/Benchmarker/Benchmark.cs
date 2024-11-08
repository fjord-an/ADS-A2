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

    public static void PrintResults(List<BenchmarkResult> results)
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
            string algorithmType = result.FileProperties.GetType().Name == "OptimisedAlgorithms" ? "(Optimised)" : "(Basic)";

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
        
        // #### TODO ##### fix this to the new implementation ((uncomment the code below))
        // Console.ForegroundColor = ConsoleColor.Yellow;
        // //with the swap flag:
        // Console.WriteLine($"BubbleSort of {file1.FileName} with the swap flag performance boost:\n {results.Keys.Where(k => k.Contains("swap optimization"))}ms");
        // //without the swap flag:
        // Console.WriteLine($"BubbleSort of {file1.FileName} with no swap checker: \n {BubbleSortBasic1Time}ms");
        // Console.ForegroundColor = ConsoleColor.Red;
        // //with the swap flag:
        // Console.WriteLine($"BubbleSort of {fileName2} with the swap flag performance boost:\n {BubbleSort2Time}ms");
        // //without the swap flag:
        // Console.WriteLine($"BubbleSort of {fileName2} with no swap checker: \n {BubbleSortBasic2Time}ms");
        // Console.ResetColor();
        // // print new line for the next comparison statement
        // Console.WriteLine();
        //
        // long file1Time;
        // long file2Time;
        //
        // //#################################################################
        // Console.ForegroundColor = ConsoleColor.Yellow;
        // // File 1's algorithms comparison
        // Console.Write(fileName1);
        // if (BubbleSort1Time < BubbleSortBasic1Time)
        // {
        //     // Swap flag is faster:
        //     Console.Write($" swap checker increased speed by {BubbleSortBasic1Time - BubbleSort1Time}ms");
        //     file1Time = BubbleSort1Time;
        // }
        // else
        // {
        //     // Swap flag is slower:
        //     Console.Write($" beat its swap checker algorithm by {BubbleSort1Time - BubbleSortBasic1Time}ms");
        //     file1Time = BubbleSortBasic1Time;
        // }
        //
        // // print new line for the next comparison statement
        // Console.WriteLine();
        //
        // //#################################################################
        // Console.ForegroundColor = ConsoleColor.Red;
        // // File 2's algorithms comparison
        // Console.Write(fileName2);
        // if (BubbleSort2Time < BubbleSortBasic2Time)
        // {
        //     // file 2 (partially sorted list)
        //     Console.Write($" swap checker increased speed by {BubbleSortBasic2Time - BubbleSort2Time}ms");
        //     file2Time = BubbleSort2Time;
        // }
        // else
        // {
        //     // file 1 (random list)
        //     Console.Write($" beat its swap checker algorithm by {BubbleSort2Time - BubbleSortBasic2Time}ms");
        //     file2Time = BubbleSortBasic2Time;
        // }
        //
        // // print new line for the next comparison statement
        // Console.WriteLine();
        //
        // if (file2Time < file1Time)
        //     // file 2 (partially sorted list)
        //     Console.WriteLine($"{fileName2} performed {file1Time - file2Time}ms faster");
        // else
        //     // file 1 (random list)
        //     Console.WriteLine($"{fileName1} performed {file2Time - file1Time}ms faster");
        //
        // Console.ResetColor();
    }

    private static BenchmarkResult MeasureAlgorithm(AlgorithmType algorithmName, InputFile inputFile, Action runAlgorithm)
    {
        // algorithmName = inputFile is OptimisedAlgorithms ? $"{algorithmName} (Optimised)" : algorithmName;
        Stopwatch sw = new();
        TimeSpan duration;

        // method to measure and run the bubble sort algorithm on the input file with memory usage
        BenchmarkResult RunAndMeasure()
        {
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
            inputFile.BubbleSort();
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