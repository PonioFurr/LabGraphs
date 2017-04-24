using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Bucket
    {
        public int Lenght
        { get { return bucket.Length; } }

        List<int>[] bucket;

        public Bucket(int count)
        {
            bucket = new List<int>[count];

            for (int k = 0; k < count; k++)
                bucket[k] = new List<int>();
        }

        public void Insert(int i, int r)
        {
            bucket[r].Add(i);
        }

        public void Remove(int i, int r)
        {
            if (bucket[r].Contains(i))
            bucket[r].Remove(i);
        }

        public int Get(int r)
        {
            if (bucket[r].Count != 0)
            {
                int b = bucket[r][0];
                bucket[r].RemoveAt(0);
                return b;
            }

            else
                return -1;
        }
    }
}
