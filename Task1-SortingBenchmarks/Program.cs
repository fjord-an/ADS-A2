using System.Diagnostics;
using ADS_A2.Algorithms;
using ADS_A2.Benchmarker;
using ADS_A2.input;

namespace Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // default file names
            string fileName1 = "a2_task1_input1.txt";
            string fileName2 = "a2_task1_input2.txt";

            // check if the user has provided the file names as arguments
            switch (args.Length)
            {
                case 0:
                    Console.WriteLine(
                        $"Please provide two file names to compare (dotnet run [file1] [file2]) or (./SortingBenchmarks [file1] [file2]\n " +
                        $"-> The default files ({fileName1} and {fileName2}) will be used for comparison");
                    break;
                case 1:
                    fileName1 = args[0];
                    Console.WriteLine(
                        $"Please provide two file names to compare (dotnet run [file1] [file2]) or (./SortingBenchmarks [file1] [file2]\n" +
                        $" -> The second default file will be used for comparison ({fileName2})");
                    break;
                case 2:
                    fileName1 = args[0];
                    fileName2 = args[1];
                    Console.WriteLine($"Comparing {fileName1} and {fileName2}");
                    break;
                default:
                    Console.WriteLine(
                        $"Usage: (./SortingBenchmarks || dotnet run) [filename1] [filename2]\n " +
                        $"-> The default files {fileName1} and {fileName2}will be used for comparison");
                    return;
            }

            // read the files and start the stopwatch
            var file1Optimised = new OptimisedAlgorithms(fileName1);
            var file2Optimised = new OptimisedAlgorithms(fileName2);
            var file1 = new Algorithms(fileName1);
            var file2 = new Algorithms(fileName2);
            
            // List<InputFile> inputFiles = new()
            // {
            //     file1, file1Optimised, file2, file2Optimised
            //     
            // };

            
            // create separate instances for each sorting algorithm
            var file1OptimisedBubble = new OptimisedAlgorithms(fileName1);
            var file1OptimisedMerge = new OptimisedAlgorithms(fileName1);
            var file2OptimisedBubble = new OptimisedAlgorithms(fileName2);
            var file2OptimisedMerge = new OptimisedAlgorithms(fileName2);

            var file1Bubble = new Algorithms(fileName1);
            var file1Merge = new Algorithms(fileName1);
            var file2Bubble = new Algorithms(fileName2);
            var file2Merge = new Algorithms(fileName2);
            
            // create a list of algorithms to benchmark with named elements in a tuple
            List<InputFile> inputFiles = new()
            {
                file1Bubble, file1Merge, file2Bubble, file2Merge,
                file1OptimisedBubble, file1OptimisedMerge, file2OptimisedBubble, file2OptimisedMerge
            };
            
            // create a list of algorithms to benchmark with named elements in a tuple
            List<(AlgorithmType Type, Action<InputFile> Algorithm)> algorithms = new()
            {
                (AlgorithmType.BubbleSort, testRun => testRun.BubbleSort()),
                (AlgorithmType.MergeSort, testRun => testRun.MergeSort())
            };
            
            List<BenchmarkResult> results = Benchmark.Run(inputFiles, algorithms);

            // List<BenchmarkResult> results = Benchmark.Run(inputFiles);
            // print the results of the benchmark
            Benchmark.PrintResults(results);
        }
    }
}
