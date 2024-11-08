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
    // Object Oriented Adaptation of the functional Merge Sort Algorithm
    // GeeksforGeeks. (2013, March 15). Merge Sort—Data Structure and Algorithms Tutorials. GeeksforGeeks. https://www.geeksforgeeks.org/merge-sort/
    {
        void Merge(int[] array, int left, int middle, int right)
        {
            int n1 = middle - left + 1; // no of elements in the left array (with zero based index adjustment)
            int n2 = right - middle;
            
            int[] leftArray = new int[n1];
            int[] rightArray = new int[n2];
            
            
            int i, j, k; // loop indices 
            
            // copy the elements from the main array to the left and right arrays
            // splitting them into two
            for (i = 0; i < n1; i++)
                leftArray[i] = Numbers[left + i];
            for (j = 0; j < n2; j++)
                rightArray[j] = Numbers[middle + 1 + j];

            // reset the indices
            i = 0;
            j = 0;
            k = left;

            // merge the arrays back together in order, sorting the numbers.
            while (i < n1 && j < n2)
            {
                if (leftArray[i] <= rightArray[j])
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }
                k++;
            }
            
            // Copy remaining of L[]
            while (i < n1) {
                array[k] = leftArray[i];
                i++;
                k++;
            }
    
            // Copy remaining elements
            // of R[] if any
            while (j < n2) {
                array[k] = rightArray[j];
                j++;
                k++;
            }
        }
        
        void Sort(int[] array, int left, int right)
        {
            
            if (left < right) // will recursively loop until the elements are seperated (1 < 1 = false)
            // the base case will stop when the left index is greater than the right index
            {
                int middle = left + (right - left) / 2;
                
                Sort(array, left, middle);
                Sort(array, middle + 1, right);

                Merge(array, left, middle, right);
            }
        }
        
        Sort(Numbers, 0, Numbers.Length - 1); 
        // todo right index of the array - 1 because the first number is accounted for?
    }
}