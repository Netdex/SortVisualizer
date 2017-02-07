using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortVisualizer.Algorithm
{
    class QuickSort : SortAlgorithm
    {
        public override void Sort(CompareItem[] barray)
        {
            Quicksort(barray, 0, barray.Length - 1);
        }

        public void Quicksort(CompareItem[] elements, int left, int right)
        {
            int i = left, j = right;
            int p = (left + right) / 2;
            CompareItem pivot = elements[p];
           
            while (i <= j)
            {
                OnCompare(i, p);
                while (elements[i].CompareTo(pivot) < 0)
                {
                    OnCompare(i, p);
                    i++;
                }

                OnCompare(j, p);
                while (elements[j].CompareTo(pivot) > 0)
                {
                    OnCompare(j, p);
                    j--;
                }

                if (i <= j)
                {
                    CompareItem tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;
                    OnSwap(i, j);
                    i++;
                    j--;
                }
            }

            if (left < j)
            {
                Quicksort(elements, left, j);
            }
            if (i < right)
            {
                Quicksort(elements, i, right);
            }
        }

    }
}
