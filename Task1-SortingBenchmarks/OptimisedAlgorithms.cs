namespace Benchmarks;

public class OptimisedAlgorithms : InputFile
{
    public OptimisedAlgorithms(string file) : base(file)
    {
    }

    public override void BubbleSort()
    {
        // The bubble sort algorithm can be optimized by breaking the loop if no swaps are made
        // if no swaps are made then the array is already sorted, capping the worst case time complexity
        // from O(n^2) to O(n)
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
                break;
            }
        }
        // if (!swapped) Console.WriteLine("The array is already sorted");
    }
}