using ADS_A2.input;
using Benchmarks;

namespace ADS_A2.Algorithms;

public class OptimisedAlgorithms : InputFile
{
    public OptimisedAlgorithms(string fileName) : base(fileName)
    {
    }

    public override void BubbleSort()
    {
        // The bubble sort algorithm can be optimized by breaking the loop if no swaps are made
        // if no swaps are made then the array is already sorted, capping the worst case time complexity
        // from O(n^2) to O(n)
        
        // in the case of input2.txt, the partially sorted array, only 1 number needs to be swapped, making 
        // the time complexity O(1) as the array is already sorted, making a massive improvement in performance
        // for partially sorted arrays, saving 500 milliseconds to a whole second in some tests (on a laptop with an i7 processor)
        bool swapped;

        // reset the swapped flag and breaking the loop if no swaps are made
        for (int i = 0; i < Numbers.Length - 1; i++)
        {
            swapped = false;
            for (int j = 0; j < Numbers.Length - i - 1; j++)
            {
                if (Numbers[j] > Numbers[j + 1])
                {
                    // the swap can be accomplished in a single line by using tuple deconstruction in C#
                    (Numbers[j], Numbers[j + 1]) = (Numbers[j + 1], Numbers[j]);
                    // when the inner loop completes without any swaps, the array is sorted
                    swapped = true;
                }
            }
            if (!swapped)
            {
                // Array is already sorted
                break;
            }
        }
    }

    public override void MergeSort()
    {
        throw new NotImplementedException();   
    }
}