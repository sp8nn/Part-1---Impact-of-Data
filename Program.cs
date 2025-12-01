using System;
using System.Diagnostics;


int[] largeArr = GenerateRandomArray(100000, 1, 1000);


int[] ascendingArray = largeArr.OrderBy(x => x).ToArray();
int[] descendingArray = largeArr.OrderByDescending(x => x).ToArray();


int size = largeArr.Length;

int[] partAsc = largeArr.Take(size / 3).OrderBy(x => x).ToArray();
int[] partDesc = largeArr.Skip(size / 3).Take(size / 3).OrderByDescending(x => x).ToArray();
int[] partRand = largeArr.Skip(2 * size / 3).Take(size / 3).ToArray();

int[] partialArray = partAsc.Concat(partDesc).Concat(partRand).ToArray();

Console.WriteLine("=== RANDOM DATA RESULTS ===");
MeasureAndPrint("Quick Sort", QuickSort, largeArr);
MeasureAndPrint("Merge Sort", MergeSort, largeArr);

int truncationLimit = 20000;
int[] truncatedRandom = TruncateArray(largeArr, truncationLimit);
MeasureAndPrint("Bubble Sort", BubbleSort, truncatedRandom);
MeasureAndPrint("Insertion Sort", InsertionSort, truncatedRandom);

Console.WriteLine("\n=== ASCENDING DATA RESULTS ===");
MeasureAndPrint("Quick Sort", QuickSort, ascendingArray);
MeasureAndPrint("Merge Sort", MergeSort, ascendingArray);

int[] truncatedAsc = TruncateArray(ascendingArray, truncationLimit);
MeasureAndPrint("Bubble Sort", BubbleSort, truncatedAsc);
MeasureAndPrint("Insertion Sort", InsertionSort, truncatedAsc);

Console.WriteLine("\n=== DESCENDING DATA RESULTS ===");
MeasureAndPrint("Quick Sort", QuickSort, descendingArray);
MeasureAndPrint("Merge Sort", MergeSort, descendingArray);

int[] truncatedDesc = TruncateArray(descendingArray, truncationLimit);
MeasureAndPrint("Bubble Sort", BubbleSort, truncatedDesc);
MeasureAndPrint("Insertion Sort", InsertionSort, truncatedDesc);

Console.WriteLine("\n=== PARTIALLY SORTED DATA RESULTS ===");
MeasureAndPrint("Quick Sort", QuickSort, partialArray);
MeasureAndPrint("Merge Sort", MergeSort, partialArray);

int[] truncatedPartial = TruncateArray(partialArray, truncationLimit);
MeasureAndPrint("Bubble Sort", BubbleSort, truncatedPartial);
MeasureAndPrint("Insertion Sort", InsertionSort, truncatedPartial);



static void MeasureAndPrint(string name, Func<int[], int[]> sorter, int[] source)
{
    int[] inputCopy = CopyArray(source);
    Stopwatch sw = Stopwatch.StartNew();
    int[] sorted = sorter(inputCopy);
    sw.Stop();
    Console.WriteLine($"Algorithm: {name} Time Taken: {sw.ElapsedMilliseconds} ms");
}

static int[] BubbleSort(int[] input)
{
    int[] arr = CopyArray(input);
    int n = arr.Length;
    bool swapped;
    for (int i = 0; i < n - 1; i++)
    {
        swapped = false;
        for (int j = 0; j < n - 1 - i; j++)
        {
            if (arr[j] > arr[j + 1])
            {
                int tmp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = tmp;
                swapped = true;
            }
        }
        if (!swapped) break;
    }
    return arr;
}

static int[] InsertionSort(int[] input)
{
    int[] arr = CopyArray(input);
    int n = arr.Length;
    for (int i = 1; i < n; i++)
    {
        int key = arr[i];
        int j = i - 1;
        while (j >= 0 && arr[j] > key)
        {
            arr[j + 1] = arr[j];
            j--;
        }
        arr[j + 1] = key;
    }
    return arr;
}

static int[] MergeSort(int[] input)
{
    int[] arr = CopyArray(input);
    if (arr.Length <= 1) return arr;
    MergeSortInPlace(arr, 0, arr.Length - 1);
    return arr;
}

static void MergeSortInPlace(int[] arr, int left, int right)
{
    if (left >= right) return;
    int mid = left + (right - left) / 2;
    MergeSortInPlace(arr, left, mid);
    MergeSortInPlace(arr, mid + 1, right);
    Merge(arr, left, mid, right);
}

static void Merge(int[] arr, int left, int mid, int right)
{
    int n1 = mid - left + 1;
    int n2 = right - mid;
    int[] leftArr = new int[n1];
    int[] rightArr = new int[n2];

    for (int i = 0; i < n1; i++) leftArr[i] = arr[left + i];
    for (int j = 0; j < n2; j++) rightArr[j] = arr[mid + 1 + j];

    int ii = 0, jj = 0, k = left;
    while (ii < n1 && jj < n2)
    {
        if (leftArr[ii] <= rightArr[jj])
        {
            arr[k++] = leftArr[ii++];
        }
        else
        {
            arr[k++] = rightArr[jj++];
        }
    }
    while (ii < n1) arr[k++] = leftArr[ii++];
    while (jj < n2) arr[k++] = rightArr[jj++];
}

static int[] QuickSort(int[] input)
{
    int[] arr = CopyArray(input);
    QuickSortInPlace(arr, 0, arr.Length - 1);
    return arr;
}

static void QuickSortInPlace(int[] arr, int low, int high)
{
    if (low < high)
    {
        int p = Partition(arr, low, high: high);
        QuickSortInPlace(arr, low, p - 1);
        QuickSortInPlace(arr, p + 1, high);
    }
}

static int Partition(int[] arr, int low, int high)
{
    int pivot = arr[high];
    int i = low - 1;
    for (int j = low; j <= high - 1; j++)
    {
        if (arr[j] <= pivot)
        {
            i++;
            int tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
    }
    int tmp2 = arr[i + 1];
    arr[i + 1] = arr[high];
    arr[high] = tmp2;
    return i + 1;
}

static int[] GenerateRandomArray(int length, int minValue, int maxValue)
{
    Random rand = new Random();
    int[] array = new int[length];
    for (int i = 0; i < length; i++)
        array[i] = rand.Next(minValue, maxValue);
    return array;
}

static int[] CopyArray(int[] source)
{
    int[] copy = new int[source.Length];
    for (int i = 0; i < source.Length; i++) copy[i] = source[i];
    return copy;
}

static int[] TruncateArray(int[] source, int newLength)
{
    int len = Math.Min(newLength, source.Length);
    int[] result = new int[len];
    for (int i = 0; i < len; i++) result[i] = source[i];
    return result;
}

static bool IsSorted(int[] arr)
{
    for (int i = 1; i < arr.Length; i++)
        if (arr[i - 1] > arr[i]) return false;
    return true;
}

/*
Sorting Performance Reflection

Why do you think the performance differs?
   The performance changes because each sorting algorithm responds differently to how the data is arranged. 
   Already sorted arrays require almost no work for some algorithms, while reverse sorted or mixed arrays 
   force certain algorithms to do a lot more comparisons and swaps. The structure of the input basically 
   determines how much effort the algorithm has to put in.

Did different algorithms react differently?
   Yes. Quick Sort showed noticeable performance changes depending on the input order fast on random or 
   partially sorted data but slower on reverse sorted cases. Merge Sort stayed the most consistent since it 
   always follows the same process. Bubble Sort and Insertion Sort improved a lot on sorted data but performed 
   poorly with reverse sorted arrays. Each algorithm’s strategy affects how it responds to different cases.

How does this represent real world data?
   Real world data is rarely fully random. It usually has some kind of structure, like partial ordering or 
   grouped patterns. The sorted, reverse, and mixed arrays in this assignment represent the kinds of input 
   shapes actual programs deal with, such as logs, time series information, or user generated lists.

Is any algorithm always best?
   No. There's no universally best algorithm. Some are faster on average, some are more predictable, and some 
   only work well on small datasets. The “best” choice depends on the data size, how the data is arranged, 
   and how much memory the program can use. Real applications choose an algorithm based on the situation, 
   not a single perfect solution.
*/
