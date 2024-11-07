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
            
            List<InputFile> inputFiles = new() {file1Optimised, file2Optimised, file1, file2};

            List<BenchmarkResult> results = Benchmark.Run(inputFiles);
            // print the results of the benchmark
            Benchmark.PrintResults(results);

        }
    }
}
