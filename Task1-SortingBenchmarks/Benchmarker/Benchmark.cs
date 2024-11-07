using System.Diagnostics;
using ADS_A2.Algorithms;
using ADS_A2.Extensions;
using ADS_A2.input;
using Benchmarks;

namespace ADS_A2.Benchmarker;

public static class Benchmark
{
    public static List<BenchmarkResult> Run(InputFile file1, InputFile file2)
    {
        // overload method to accept two InputFile objects instead of a list
        List<InputFile> files = new() { file1, file2 };
        return Run(files);
    }
    public static List<BenchmarkResult> Run(List<InputFile> files)
    // runs and returns the results of the benchmark in a dictionary with the InputFile object as the key
    // and the time taken to sort the array as the value with the algorithm name as the key
    {
        // initialize variables to store the results and the time taken to sort the arrays
        List<BenchmarkResult> results = new(); 
        
        foreach (InputFile file in files)
        {
            // polymorphic call the BubbleSort method on the InputFile object, performing different algorithms
            // depending on the object type, then storing the benchmark results to the dictionary for reference
            results.Add(MeasureAlgorithm(file));
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

        foreach (var result in results)
        {
            // iterate through the results dictionary and print the time taken to sort the arrays for each file
            // print each individual algorithms performance of the InputFile object
            // By using my custom ToReadableString() extension method, the time taken to sort the arrays
            // is printed in a human-readable format
            
            // NOTE: if memory usage is reading 0, then memory usage is negligable. Such low amounts of memory
            // usage cannot be accurately measured using the GC.GetTotalMemory() method. Such is the case when
            // the optimised partially sorted array is sorted, as only 1 swap is made. Very little memory is used and hard to measure.

            Console.Write($"{result.AlgorithmName} took ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{result.ExecutionTime.ToReadableString()} ");
            Console.ResetColor();
            Console.WriteLine($"to sort {result.FileProperties.FileName}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Memory usage: {result.MemoryUsage.ToReadableSize()}");
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

    private static BenchmarkResult MeasureAlgorithm(InputFile inputFile)
    {
        string algorithmName = inputFile is OptimisedAlgorithms ? "Bubble Sort (Optimised)" : "Bubble Sort";
        Stopwatch sw = new();
        TimeSpan duration;
        // long memoryUsed = 0;
        bool noGCRegionStarted = false;

        BenchmarkResult BubbleSort()
        {
            // Because this method is run in a no GC (grabage collection) region, must set the forceFullCollection parameter to false, otherwise
            // the NoGC region will be ended and the memory usage will not be accurate, and errors will be thrown when trying to end the region
            // TODO link the reference/source/documentation:
            long initialMemory = GC.GetTotalMemory(false);
            
            // perform the bubble sort algorithm on the input file
            // timing the duration of the algorithm and storing the memory used
            sw.Start();
            inputFile.BubbleSort();
            sw.Stop();
            long finalMemory = GC.GetTotalMemory(false);
            
            duration = sw.Elapsed;
            // calculate the memory used by the algorithm (difference between the initial and final)
            long memoryUsed = finalMemory - initialMemory;
            sw.Reset();
                
            return new BenchmarkResult(memoryUsed, duration, algorithmName, inputFile);
        }
        
        // get the initial memory usage before sorting the array to calculate
        // the memory used by the algorithm
        // todo GC.Collect() is called to ensure that the memory usage is accurate
        
        // prevent the garbage collector from running during the benchmark up to the specified size
        // this ensures that the memory usage reading is accurate
        try
        {
            
            if (GC.TryStartNoGCRegion(1024 * 1024 * 1000)) // 100MB
            {
                noGCRegionStarted = true;
                
                return BubbleSort();
            }
            else
            {
                // if no GC region cannot be started, measure only the time taken to sort the array
                sw.Start();
                inputFile.BubbleSort();
                sw.Stop();
                duration = sw.Elapsed;
                // calculate the memory used by the algorithm (difference between the initial and final)
                sw.Reset();            
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Could not start a no GC region. No memory usage data will be available.");
                Console.ResetColor();
                
                return new BenchmarkResult(0, duration, algorithmName, inputFile);
            }
        }
        finally
        {
            // end the no GC region
            if (noGCRegionStarted)
                GC.EndNoGCRegion();
        }
    }
}