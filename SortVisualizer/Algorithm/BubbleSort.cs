using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortVisualizer.Algorithm
{
    class BubbleSort : SortAlgorithm
    {
        public override void Sort(CompareItem[] arr)
        {
            CompareItem temp;
            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1 - write; sort++)
                {
                    OnCompare(sort, sort + 1);
                    if (arr[sort].CompareTo(arr[sort + 1]) > 0)
                    {
                        temp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = temp;
                        OnSwap(sort, sort + 1);
                    }
                }
            }
            for (int i = 0; i < arr.Length - 1; i++)
            {
                OnCompare(i, i + 1);
            }
        }
    }
}
