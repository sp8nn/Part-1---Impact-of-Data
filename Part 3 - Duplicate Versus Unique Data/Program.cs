using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("Part 3: Duplicate Value Sorting Performance\n");

        int size = 500000;

        int[] uniqueArray = GenerateUniqueArray(size);
        int[] halfRepeatArray = GenerateHalfRepeatArray(size);
        int[] heavyRepeatArray = GenerateHeavyRepeatArray(size);

        Console.WriteLine("Testing arrays...\n");

        TestDataset("All Unique Values", uniqueArray);
        TestDataset("50% Repeating Values", halfRepeatArray);
        TestDataset("Heavily Repeated Values (1-10)", heavyRepeatArray);
    }

    static void TestDataset(string label, int[] source)
    {
        Console.WriteLine($"- {label}");
        Measure("Quick Sort", QuickSort, source);
        Measure("Merge Sort", MergeSort, source);
        
        Console.WriteLine();
    }

    static void Measure(string name, Func<int[], int[]> sorter, int[] src)
    {
        int[] copy = CopyArray(src);
        Stopwatch sw = Stopwatch.StartNew();
        sorter(copy);
        sw.Stop();
        Console.WriteLine($"{name}: {sw.ElapsedMilliseconds} ms");
    }


    static int[] GenerateUniqueArray(int size)
    {
        int[] arr = new int[size];
        for (int i = 0; i < size; i++) arr[i] = i;

        Shuffle(arr);
        return arr;
    }

    static int[] GenerateHalfRepeatArray(int size)
    {
        int[] arr = new int[size];
        int half = size / 2;
        Random rand = new Random();

        for (int i = 0; i < half; i++) arr[i] = i;

        for (int i = half; i < size; i++) arr[i] = rand.Next(0, half);

        Shuffle(arr);
        return arr;
    }

    static int[] GenerateHeavyRepeatArray(int size)
    {
        int[] arr = new int[size];
        Random rand = new Random();
        for (int i = 0; i < size; i++) arr[i] = rand.Next(1, 11);
        return arr;
    }

    static void Shuffle(int[] arr)
    {
        Random rand = new Random();
        for (int i = 0; i < arr.Length; i++)
        {
            int j = rand.Next(arr.Length);
            int tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
    }


    static int[] CopyArray(int[] source)
    {
        int[] copy = new int[source.Length];
        for (int i = 0; i < source.Length; i++) copy[i] = source[i];
        return copy;
    }

    static int[] MergeSort(int[] input)
    {
        int[] arr = CopyArray(input);
        MergeSortInPlace(arr, 0, arr.Length - 1);
        return arr;
    }

    static void MergeSortInPlace(int[] arr, int left, int right)
    {
        if (left >= right) return;
        int mid = (left + right) / 2;
        MergeSortInPlace(arr, left, mid);
        MergeSortInPlace(arr, mid + 1, right);
        Merge(arr, left, mid, right);
    }

    static void Merge(int[] arr, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;
        int[] L = new int[n1];
        int[] R = new int[n2];

        for (int i = 0; i < n1; i++) L[i] = arr[left + i];
        for (int j = 0; j < n2; j++) R[j] = arr[mid + 1 + j];

        int ii = 0, jj = 0, k = left;

        while (ii < n1 && jj < n2)
            arr[k++] = (L[ii] <= R[jj]) ? L[ii++] : R[jj++];

        while (ii < n1) arr[k++] = L[ii++];
        while (jj < n2) arr[k++] = R[jj++];
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
            int p = Partition(arr, low, high);
            QuickSortInPlace(arr, low, p - 1);
            QuickSortInPlace(arr, p + 1, high);
        }
    }

    static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }

        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }
}
