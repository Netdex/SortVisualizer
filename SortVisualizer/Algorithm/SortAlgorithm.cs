using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SortVisualizer.Algorithm
{
    abstract class SortAlgorithm
    {
        public event SwapEventHandler SwapEvent;
        public event CompareEventHandler CompareEvent;

        protected SortAlgorithm()
        {

        }

        public abstract void Sort(CompareItem[] barray);

        public delegate void SwapEventHandler(int a, int b);

        public delegate void CompareEventHandler(int a, int b);

        protected void OnSwap(int a, int b)
        {
            SwapEvent?.Invoke(a, b);
        }

        protected void OnCompare(int a, int b)
        {
            CompareEvent?.Invoke(a, b);
        }
    }
}
