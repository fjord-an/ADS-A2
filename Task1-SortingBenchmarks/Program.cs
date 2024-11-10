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
            string fileName1 = "unsorted_numbers.txt";
            string fileName2 = "partially_sorted_numbers.txt";

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

            // Check if files are valid
            fileName1 = ValidateFile(fileName1);
            fileName2 = ValidateFile(fileName2);
            
            
            // create a list of algorithms to benchmark with named elements in a tuple
            List<(AlgorithmType Type, Action<InputFile> Algorithm)> algorithms = new()
            {
                // these Action delegates are used to pass the method as a parameter to
                // the MeasureAlgorithm method for code reuse
                (AlgorithmType.BubbleSort, testRun => testRun.BubbleSort()),
                (AlgorithmType.MergeSort, testRun => testRun.MergeSort())
            };
            
            // read unique instances of the input files so that the same file is not sorted multiple times
            var file1Optimised = new OptimisedAlgorithms(fileName1);
            var file2Optimised = new OptimisedAlgorithms(fileName2);
            var file1 = new Algorithms(fileName1);
            var file2 = new Algorithms(fileName2);
            
            // create a list of input files to benchmark
            List<InputFile> inputFiles = new()
            {
                file1, file1Optimised, file2, file2Optimised
            };

            
            List<BenchmarkResult> results = Benchmark.Run(inputFiles, algorithms);

            // print the results of the benchmark
            PrintResults.Display(results);
        }
        
       
        private static string ValidateFile(string fileName)
        {
            while (!File.Exists(fileName))
            {
                Console.WriteLine($"File '{fileName}' not found. Please enter the relative directory to load the file:");
                string relativeDirectory = Console.ReadLine();
                Console.WriteLine("Please enter the filename:");
                string newFileName = Console.ReadLine();
                fileName = Path.Combine(relativeDirectory, newFileName);
            }
            return fileName;
        }
    }
}
