namespace Benchmarks 
{
    public class ReadFile
    {
        private string[] contents;
        private int[] numbers;
        
        public ReadFile(string path = "../../../input/a2_task1_input1.txt")
        {
            Parse(path);
        }
        public void Parse(string path)
        {
            using (StreamReader reader = new (path))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    contents = line.Split(' ');
                    numbers = new int[numbers.Length];
                    
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        numbers[i] = int.Parse(contents[i]);
                    }
                }
            }
        }
        
        public int[] BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }
            return array;
        }
        
        public void PrintArray(int[] array)
        {
            foreach (int number in array)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
        }
    }
}
