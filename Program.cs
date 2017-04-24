using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Program
    {
        static void WritePath(List<int> path, string header)
        {
            Console.WriteLine(header);

            for (int k=0; k<path.Count-1;k++)
                Console.Write("{0}->", path[k]);
            Console.WriteLine("{0}", path[path.Count-1]);
        }

        static void WritePath(Dictionary<int,List<int>> path, string header)
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
            Graph g = new Graph(new int[] { 2, 1, 3, 4, 4, 5, 6, 9, 6, 7 }, new int[] { 1, 3, 4, 5, 6, 9, 9, 10, 7, 8 }, new int[] { 4, 2, 3, 3, 1, 6, 2, 4, 5, 6 });
            //g.GenerateGraphvizCode("g");

            Graph gMin = g.Kruskal();
            //gMin.GenerateGraphvizCode("gMin");

            WritePath(g.DepthFirstSearch(3, 10), "\nПоиск пути в глубину от вершины 3 до вершины 10:");

            WritePath(g.BreadthFirstSearch(3, 10), "\nПоиск пути в ширину от вершины 3 до вершины 10:");

            WritePath(g.BellmanFord(3), "\nБелман Форд:");

            WritePath(g.DijkstraBucket(3), "\nДэйкстра:");

            Console.WriteLine("\nБыло до сортировки:");
            List<int> heapOfTrash = new List<int> { 4,10,11,55,1,2,0,5,6};
            foreach(int i in heapOfTrash)
                Console.Write("{0}, ",i);
            Console.WriteLine();
            Console.WriteLine("После пирамидки:");
            foreach (int i in Heap.Sort(heapOfTrash))
                Console.Write("{0}, ", i);
            Console.WriteLine();

            Console.ReadKey();  
        }
    }
}
