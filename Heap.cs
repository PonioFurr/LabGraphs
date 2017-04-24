using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Heap
    {
        public int Size => elements.Count;

        public int this[int k] => elements[k];

        private List<int> elements;

        public Heap(List<int> elements)
        {
            this.elements = new List<int>();
            foreach (int e in elements)
                AddElement(e);
        }

        public void AddElement(int e)
        {
            if (Size == 0)
            {
                elements.Add(e);
            }
            else
            {
                elements.Add(e);
                int target = elements.Count - 1;
                int parent = GetParentIndex(target);

                while (elements[target] > elements[parent])
                {
                    int temp = elements[parent];
                    elements[parent] = elements[target];
                    elements[target] = temp;

                    target = parent;
                    parent = GetParentIndex(target);
                }
            }
        }

        public int ExtractElement()
        {
            int extracted = elements[0];

            elements[0] = elements[Size - 1];
            elements.RemoveAt(Size - 1);

            if (Size > 1)
            {
                int parent = 0;
                int target = (GetRightChildIndex(parent) >= Size || elements[GetLeftChildIndex(parent)] > elements[GetRightChildIndex(parent)]) ?
                        GetLeftChildIndex(parent) : (GetRightChildIndex(parent));
                if (elements[parent] < elements[target])
                    do
                    {
                        int temp = elements[parent];
                        elements[parent] = elements[target];
                        elements[target] = temp;

                        parent = target;
                        target = (GetRightChildIndex(parent) >= Size || elements[GetLeftChildIndex(parent)] > elements[GetRightChildIndex(parent)]) ?
                                GetLeftChildIndex(parent) : (GetRightChildIndex(parent));
                    } while (target < Size);
            }

            return (extracted);
        }

        private int GetLeftChildIndex(int k) => k * 2 + 1;

        private int GetRightChildIndex(int k) => k * 2 + 2;

        private int GetParentIndex(int k) => (k - 1) / 2;

        public static List<int> Sort(List<int> elements)
        {
            List<int> result = new List<int>();
            Heap h = new Heap(elements);
            while(h.Size!=0)
            {
                result.Add(h.ExtractElement());
            }

            return result;
        }
    }
}
