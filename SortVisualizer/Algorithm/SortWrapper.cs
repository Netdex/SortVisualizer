using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SortVisualizer.Algorithm
{
    class SortWrapper
    {
        private const int SWAP_DELAY = 2;
        private const int COMPARE_DELAY = 2;

        private Random mRandom = new Random();
        public SortAlgorithm Algorithm { get; }

        public CompareItem[] Items { get; }

        public delegate void StateChangedHandler(int idx, CompareItem itm);

        public delegate void SortFinishedHandler();

        public event StateChangedHandler OnStateChanged;
        public event SortFinishedHandler OnSortFinished;
        public event SortAlgorithm.SwapEventHandler OnSwap;

        public SortWrapper(SortAlgorithm algorithm, int size)
        {
            Algorithm = algorithm;
            Algorithm.SwapEvent += (a, b) =>
            {
                ResetState(CompareItem.SortState.Swapping);
                Items[a].State = CompareItem.SortState.Swapping;
                Items[b].State = CompareItem.SortState.Swapping;
                OnStateChanged(a, Items[a]);
                OnStateChanged(b, Items[b]);
                
                Thread.Sleep(SWAP_DELAY);
            };
            Algorithm.CompareEvent += (a, b) =>
            {
                ResetState(CompareItem.SortState.Comparing);
                Items[a].State = CompareItem.SortState.Comparing;
                Items[b].State = CompareItem.SortState.Comparing;
                OnStateChanged(a, Items[a]);
                OnStateChanged(b, Items[b]);
                OnSwap(a, b);
                Thread.Sleep(COMPARE_DELAY);
            };
            Items = new CompareItem[size];
            for (int i = 0; i < size; i++)
            {
                Items[i] = new CompareItem(i + 1);
            }
        }

        public void SortAsync()
        {
            new Thread(() =>
            {
                Algorithm.Sort(Items);
                OnSortFinished();
            }).Start();
        }

        public void Randomize()
        {
            Shuffle(mRandom, Items);
            for (int i = 0; i < Items.Length; i++)
                OnStateChanged(i, Items[i]);
        }

        
        private void ResetState(CompareItem.SortState s)
        {
            for(int i = 0; i < Items.Length; i++)
            {
                CompareItem it = Items[i];
                if (it.State == s)
                {
                    it.State = CompareItem.SortState.None;
                    OnStateChanged(i, it);
                }
            }
        }

        private static void Shuffle<T>(Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        
    }

    class CompareItem : IComparable<CompareItem>
    {
        public int Value { get; }
        public SortState State { get; set; }

        public CompareItem(int v)
        {
            Value = v;
            State = SortState.None;
        }
        public int CompareTo(CompareItem o)
        {
            return Value - o.Value;
        }

        internal enum SortState
        {
            None,
            Comparing,
            Swapping,
        }
    }

}
