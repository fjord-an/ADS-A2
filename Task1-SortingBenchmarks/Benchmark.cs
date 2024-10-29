using System.Diagnostics;

namespace Benchmarks;

public static class Benchmark
{
    public static Dictionary<InputFile, TimeSpan> Run(InputFile file1, InputFile file2)
    {
        // overload method to accept two InputFile objects instead of a list
        List<InputFile> files = new() { file1, file2 };
        return Run(files);
    }
    public static Dictionary<InputFile, TimeSpan> Run(List<InputFile>files)
    // runs and returns the results of the benchmark in a dictionary with the InputFile object as the key
    // and the time taken to sort the array as the value with the algorithm name as the key
    {
        // initialize variables to store the results and the time taken to sort the arrays
        Dictionary<InputFile, TimeSpan> results = new(); 
        Stopwatch sw = new();
        
        foreach (InputFile file in files)
        {
            // polymorphic call the BubbleSort method on the InputFile object, performing different algorithms
            // depending on the object type, then storing the benchmark results to the dictionary for reference
            TimeSpan algorithmTime = TimeAlgorithm(file);
            results.Add(file, algorithmTime);
        }

        // Check if the arrays are sorted correctly by comparing the respective files sorted arrays of each algorithm
        // this will ensure that the algorithms are working correctly as each algorithm will have the same output
        foreach (var fileGroup in results.Keys.GroupBy(k => k.FileName))
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
        // print the results
        PrintResults(results);
        
        return results;
    }

    public static void PrintResults(Dictionary<InputFile, TimeSpan> results)
    {
        // #print the results
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Results:");
        Console.ResetColor();

        foreach (var result in results)
        {
            // iterate through the results dictionary and print the time taken to sort the arrays for each file
            foreach (var algorithm in result.Key.AlgorithmPerformances)
                // print each individual algorithms performance of the InputFile object
            {
                Console.WriteLine($"{algorithm.Key} took {result.Value.ToReadableString()} to sort {result.Key.FileName}");
            }
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

    private static TimeSpan TimeAlgorithm(InputFile inputFile)
    {
        Stopwatch sw = new();
        TimeSpan duration;
        
        // perform the bubble sort algorithm on the input file
        sw.Start();
        inputFile.BubbleSort();
        sw.Stop();
        duration = sw.Elapsed;
        sw.Reset();
        
        // store the algorithm performance in the dictionary of the InputFile object
        inputFile.AlgorithmPerformances.Add(
            inputFile is OptimisedAlgorithms ? "Bubble Sort (Optimised)" : "Bubble Sort", duration);
            
        return duration;
    }
}