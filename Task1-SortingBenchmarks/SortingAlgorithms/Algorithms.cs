using ADS_A2.input;
using Benchmarks;

namespace ADS_A2.Algorithms;

public class Algorithms : InputFile
{
    public Algorithms(string fileName) : base(fileName)
    {
    }
    
    public override void BubbleSort()
    {
        // Worst Case: O(n^2)

        // reset the swapped flag and breaking the loop if no swaps are made
        for (int i = 0; i < Numbers.Length - 1; i++)
        {
            for (int j = 0; j < Numbers.Length - i - 1; j++)
            {
                if (Numbers[j] > Numbers[j + 1])
                {
                    // the swap can be accomplished in a single line by using tuple deconstruction in C#
                    (Numbers[j], Numbers[j + 1]) = (Numbers[j + 1], Numbers[j]);
                    // when the inner loop completes without any swaps, the array is sorted
                }
            }
        }
    }

    public override void MergeSort()
    // GeeksforGeeks. (2013, March 15). Merge Sort—Data Structure and Algorithms Tutorials. GeeksforGeeks. https://www.geeksforgeeks.org/merge-sort/
    {
        int Merge(int[] array, int left, int middle, int right)
        {
           return array.Length;
        }
        
        void Sort(int[] array)
        {
            
        }
    }
}