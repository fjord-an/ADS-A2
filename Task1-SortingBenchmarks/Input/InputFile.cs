using ADS_A2.Benchmarker;

namespace ADS_A2.input 
{
public abstract class InputFile
    {
        // using object oriented approach to maintain the states of the file contents for results comparison
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public string[] Contents {get; private set;}
        public int[] Numbers { get; private set; }
        // TODO need to change base directory in accordance with replit's VM file structure and base directory
        private static readonly string BaseDirectory = Path.GetFullPath("../../../input/");
        public BenchmarkResult AlgorithmPerformances { get; set; }


        public InputFile(string fileName)
        {
            FileName = fileName;
            FilePath = Path.Combine(BaseDirectory, fileName);
            
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("File not found");
            
            Parse(FilePath);
            
            AlgorithmPerformances = new BenchmarkResult(0, TimeSpan.Zero, "", this); 
        }

        private void Parse(string path)
        {
            using(StreamReader reader = new (path))
            {
                string? line;
                // by using a while loop we can read the file line by line until the line is null (end of file)
                // dotnet-bot. (n.d.). StreamReader.ReadLine Method (System.IO). Retrieved November 7, 2024, from https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.readline?view=net-8.0
                while ((line = reader.ReadLine()) != null)
                {
                    // extract the numbers from the file which are delimited by a space
                    // going line by line and splitting the line by the space character
                    Contents = line.Split(' ');
                    // create a new array to store the numbers parsed by the string array to integers
                    // O(n) space complexity
                    Numbers = new int[Contents.Length];
                    
                    for (int i = 0; i < Numbers.Length; i++)
                    {
                        // for each element in the string array, parse the string to an integer and store it in the Numbers array
                        // O(n) time complexity
                        // O(n) space complexity, as we are storing the integers in a new array in memory
                        // this may not be good practice for large files as there is a possibility of running out of memory
                        // when dealing with large datasets, it is better to either sort the file in place or use a temporary file, writing to disk.
                        // when space complexity is a concern: I/O>memory, when time complexity is a concern: memory>I/O)
                        
                        // in this case, we are storing the integers in memory to sort the array in place as space complexity is not a concern
                        // for the purpose of this project
                        if (int.TryParse(Contents[i], out var result))
                        {
                            if (result != 0)
                                Numbers[i] = result;
                        }
                    }
                }
            }
        }

        // abstract BubbleSort method to be implemented in different
        // ways by the derived classes (optimised and non-optimised)
        public abstract void BubbleSort();
        
        public abstract void MergeSort();
        
        public virtual void PrintArray()
        // print the contents of the array to the console for debugging purposes
        {
            foreach (int number in Numbers)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
        }
    }
}
