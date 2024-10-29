namespace Benchmarks;

public class Algorithms : InputFile
{
    public Algorithms(string file) : base(file)
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
}