namespace Benchmarks 
{
    public abstract class InputFile
    {
        // using object oriented approach to maintain the states of the file contents for results comparison
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public string[] Contents {get; private set;}
        public int[] Numbers { get; private set; }
        private static readonly string BaseDirectory = Path.GetFullPath("../../../input/");
        public Dictionary<string, TimeSpan> AlgorithmPerformances { get; set; }
        
        public InputFile(string file)
        {
            AlgorithmPerformances = new Dictionary<string, TimeSpan>();
            FileName = file;
            FilePath = Path.Combine(BaseDirectory, file);
            
            if (!File.Exists(FilePath))
                throw new FileNotFoundException("File not found");
            
            Parse(FilePath);
        }
        
        public void Parse(string path)
        {
            using (StreamReader reader = new (path))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    // extract the numbers from the file which are delimited by a space
                    Contents = line.Split(' ');
                    Numbers = new int[Contents.Length];
                    
                    for (int i = 0; i < Numbers.Length; i++)
                    {
                        Numbers[i] = int.Parse(Contents[i]);
                    }
                }
            }
        }

        public abstract void BubbleSort();
        
        public virtual void PrintArray()
        {
            foreach (int number in Numbers)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
        }
    }
}
