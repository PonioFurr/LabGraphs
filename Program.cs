using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Program
    {
        public static void WritePath(List<int> path, string header)
        {
            Console.WriteLine(header);

            for (int k=0; k<path.Count-1;k++)
                Console.Write("{0}->", path[k]);
            Console.WriteLine("{0}", path[path.Count-1]);
        }

        public static void WritePath(Dictionary<int,List<int>> path, string header)
        {
            Console.WriteLine(header);

            foreach(int k in path.Keys)
            {
                Console.Write("To {0}: ",k);
                for (int l = 0; l < path[k].Count - 1; l++)
                    Console.Write("{0}->", path[k][l]);
                Console.WriteLine("{0}", path[k][path[k].Count - 1]);
            }
            
        }

        static void Main(string[] args)
        {
            // Чтобы вывести код для графвиза в файл нужны права админа
            Graph g = new Graph(new int[] { 0, 1, 2, 2, 4, 3, 4, 6, 5 }, new int[] { 1, 2, 3, 4, 5, 6, 6, 7, 8 }, new int[] { 1, 2, 3, 1, 3, 1, 2, 4, 1 }, true);

            g.GenerateGraphvizCode("destinationGraph");
            Graph.LabMethod(g, 2, 5, 6).GenerateGraphvizCode("newGraph");


            Console.ReadKey();  
        }
    }
}
