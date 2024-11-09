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
    // Object Oriented Adaptation of the functional Merge Sort Algorithm
    // GeeksforGeeks. (2013, March 15). Merge Sort—Data Structure and Algorithms Tutorials. GeeksforGeeks. https://www.geeksforgeeks.org/merge-sort/
    {
        void Merge(int[] array, int left, int middle, int right)
        {
            int n1 = middle - left + 1; // no of elements in the left array (with zero based index adjustment)
            int n2 = right - middle; // no of elements in the right array
            
            int[] leftArray = new int[n1]; //the splitted arrays
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

            // merge the two arrays back together in ascending order,
            // sorting the numbers into the main array (index k)
            while (i < n1 && j < n2)
            {
                if (leftArray[i] <= rightArray[j]) 
                // choose the smallest element from the left or right array
                // adding it to the main array (index k)
                {
                    array[k] = leftArray[i]; //add the element to the main array
                    i++; // increment the left array index if the element is added
                }
                else
                {
                    array[k] = rightArray[j]; // add the element to the main array
                    j++; // increment the right array index if the element is added
                }
                k++; // increment the main array index after adding an element
                // (either from the left or right array)
            }
            
            // After one of the two arrays indices reach the end,
            // Copy leftover elements of the array with elements remaining
            // starting with leftArray[]:
            while (i < n1) {
                array[k] = leftArray[i];
                // increment both indices until i is equal to n1
                i++;
                k++;
            }
    
            // Copy remaining elements (if any) of rightArray[] if j has not reached
            // the end of its array 
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
