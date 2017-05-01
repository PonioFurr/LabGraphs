using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    class Graph
    {
        // Структура дуги
        public struct Edge
        {
            public int I
            {
                get;
            }
            public int J
            {
                get;
            }
            public int C
            {
                get;
            }

            public Edge(int I, int J, int C)
            {
                this.I = I;
                this.J = J;
                this.C = C;
            }
            public Edge(Edge e)
            {
                this.I = e.I;
                this.J = e.J;
                this.C = e.C;
            }
        }
    
        // Компаратор для упорядочивания дуг по параметру C
        class EdgeComparerByC : IComparer<Edge>
        {
            public int Compare(Edge x, Edge y)
            {
                if (x.C > y.C)
                    return 1;
                else if (x.C < y.C)
                    return -1;
                else
                    return 0;
            }
        }

        private List<int> nodeList;                       // список вершин
        private List<Edge> edgeList;                      // список дуг
        private Dictionary<int, List<int>> edgeBunchList; // список пучков дуг в более удобном виде

        public Graph(int[] I, int[] J, int[] C, bool oriented)  // параметр oriented на самом деле просто приводит к тому,
        {                                                       // что для каждой дуги будет создана противонаправленная дуга
            this.nodeList = new List<int>();
            this.edgeList = new List<Edge>();
            this.edgeBunchList = new Dictionary<int, List<int>>();

            if (I.Length != J.Length || I.Length!= C.Length)
                throw new Exception("Wrong parameters");

            for (int k = 0; k < I.Length; k++)
            {
                AddEdge(I[k], J[k], C[k]);

                if (oriented)
                        AddEdge(J[k], I[k], C[k]);
            }
        }

        public Graph(List<Edge> edgeList, bool oriented)
        {
            this.nodeList = new List<int>();
            this.edgeList = new List<Edge>();
            this.edgeBunchList = new Dictionary<int, List<int>>();

            foreach (Edge e in edgeList)
            {
                AddEdge(e.I, e.J, e.C);

                if (oriented)
                    AddEdge(e.J, e.I, e.C);
            }
        }

        // Добавление дуги, также дополняет список вершин и список пучков дуг
        public void AddEdge(int I, int J, int C)
        {
            if (!edgeList.Contains(new Edge(I, J, C)))
            {
                if (!nodeList.Contains(J))
                    nodeList.Add(J);

                if (!nodeList.Contains(I))
                    nodeList.Add(I);

                if (!edgeBunchList.Keys.Contains(I))
                    edgeBunchList.Add(I, new List<int>());

                edgeList.Add(new Edge(I, J, C));
                edgeBunchList[I].Add(edgeList.Count - 1);
            }
        }

        // Получение списка дуг в пучке
        private List<Edge> GetEdgeBunch(int node)
        {
            List<Edge> result = new List<Edge>();

            if(edgeBunchList.Keys.Contains(node))
            foreach (int k in edgeBunchList[node])
                result.Add(edgeList[k]);

            return result;
        }

        // Генерация кода для GraphViz: http://www.webgraphviz.com/
        public void GenerateGraphvizCode(string fileName)
        {
            string codeString = "digraph {\n";

            foreach (Edge e in edgeList)
                codeString += string.Format("{0} -> {1} [ label = \"{2}\" ]; \n", e.I, e.J, e.C);

            codeString += "}";

            System.IO.StreamWriter file = new System.IO.StreamWriter(String.Format("c:\\{0}.txt",fileName));
            file.WriteLine(codeString);

            file.Close();
        }

        // Поиск в ширину - работает
        public List<int> BreadthFirstSearch(int startNode, int destinationNode)
        {
            Queue<int> serchQueue = new Queue<int>();
            List<int> checkedNodes = new List<int>();
            bool result = false;
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();

            map.Add(startNode, new List<int> { startNode });
            serchQueue.Enqueue(startNode);
            do
            {
                int u = serchQueue.Dequeue();

                if (u == destinationNode)
                {
                    result = true;
                    break;
                }
                else
                {
                    checkedNodes.Add(u);
                    foreach(Edge e in GetEdgeBunch(u))
                        if (!checkedNodes.Contains(e.J))
                        {
                            int[] way = new int[ map[u].Count + 1 ];
                            map[u].CopyTo(way);
                            way[way.Length - 1] = e.J;
                            if (!map.ContainsKey(e.J))
                                map.Add(e.J, new List<int>(way));

                            serchQueue.Enqueue(e.J);
                        }
                }
            }
            while (serchQueue.Count != 0);

            if (result)
                return (map[destinationNode]);
            else
                return (new List<int> { -1 });
        }
        
        // Поиск в глубину - работает
        public List<int> DepthFirstSearch(int startNode, int destinationNode)
        {
            List<int> checkedNodes = new List<int>();
            bool result = false;
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            List<int> whiteNodes = new List<int>();
            int lastU;

            map.Add(startNode, new List<int> { startNode });
            int u = startNode;
            do
            {
                whiteNodes.Clear();
                if (!checkedNodes.Contains(u))
                {
                    checkedNodes.Add(u);
                    if (u == destinationNode)
                    {
                        result = true;
                        break;
                    }
                }

                foreach (Edge e in GetEdgeBunch(u))
                    if (!checkedNodes.Contains(e.J))
                        whiteNodes.Add(e.J);

                lastU = u;

                if (whiteNodes.Count != 0)
                {
                    int[] way = new int[map[u].Count + 1];
                    map[u].CopyTo(way);
                    way[way.Length - 1] = whiteNodes[0];
                    if (!map.ContainsKey(whiteNodes[0]))
                        map.Add(whiteNodes[0], new List<int>(way));

                    u = whiteNodes[0];
                }
                else
                    if (u != startNode)
                    u = map[u][map[u].Count-2];
            } while (!(lastU == startNode && whiteNodes.Count == 0));

            if (result)
                return (map[destinationNode]);
            else
                return (new List<int> { -1 });
        }
        
        // Алг. Беллмана-Форда - работает для графов без отрицательных петель (как и планировалось)
        public Dictionary<int, List<int>> BellmanFord(int i)
        {
            Dictionary<int, int> d = new Dictionary<int, int>();
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();

            d.Add(i, 0);
            map.Add(i, new List<int> { i });

            foreach(int k in nodeList)
                if (k!=i)
                    d.Add(k, int.MaxValue);

            for (int k = 0; k < d.Count - 1; k++)
                for (int l = 0; l < edgeList.Count; l++)
                    if (d[edgeList[l].I] < int.MaxValue)
                    {
                        d[edgeList[l].J] = Math.Min(d[edgeList[l].J], d[edgeList[l].I] + edgeList[l].C);
                        map[edgeList[l].J] = new List<int>();
                        for (int m = 0; m < map[edgeList[l].I].Count; m++)
                            map[edgeList[l].J].Add( map[edgeList[l].I][m]);
                        map[edgeList[l].J].Add( edgeList[l].J);
                    }                       

            return map;
        }

        // Алг. Дейкстры на черпаках - работает
        public Dictionary<int, List<int>> DijkstraBucket(int i)
        {
            Dictionary<int, int> d = new Dictionary<int, int>();
            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            List<int> checkedNodes = new List<int>();

            d.Add(i, 0);
            map.Add(i, new List<int> { i });

            foreach (int k in nodeList)
                if (k != i)
                    d.Add(k, int.MaxValue);

            int C = edgeList[0].C;
            foreach(Edge e in edgeList)
                if (e.C > C)
                    C = e.C;

            Bucket bucket = new Bucket(C * nodeList.Count);
            bucket.Insert(i, 0);
            int u;

            for (int b = 0; b < bucket.Lenght; b++)
                while ((u = bucket.Get(b)) != -1)
                {
                    foreach(Edge e in GetEdgeBunch(u))
                        if (d[e.J] > d[u] + e.C)
                        {
                            int radj = d[e.J];
                            d[e.J] = d[u] + e.C;
                            if (radj != int.MaxValue)
                                bucket.Remove(e.J, radj);
                            bucket.Insert(e.J, d[e.J]);

                            int[] way = new int[map[u].Count + 1];
                            map[u].CopyTo(way);
                            way[way.Length - 1] = e.J;
                            map.Add(e.J, new List<int>(way));
                        }
                }
            return map;
        }

        // Алг. Краскала - работает (предназначен для неориентированного графа)
        public Graph Kruskal()
        {
            Dictionary<int, int> color = new Dictionary<int, int>();
            List<Edge> sortedEdgeList = new List<Edge>();
            List<Edge> resultEdges = new List<Edge>();

            foreach (Edge e in edgeList)
                sortedEdgeList.Add(e);
            sortedEdgeList.Sort(new EdgeComparerByC());

            for (int k = 0; k < nodeList.Count; k++)
            {
                color.Add(nodeList[k], k);
            }

            for (int k = 0; k < sortedEdgeList.Count; k++)
            {
                if (color[sortedEdgeList[k].I] != color[sortedEdgeList[k].J])
                {
                    resultEdges.Add(sortedEdgeList[k]);

                    int c = color[sortedEdgeList[k].J];
                    for (int l = 0; l < nodeList.Count; l++)
                        if (color[nodeList[l]] == c)
                            color[nodeList[l]] = color[sortedEdgeList[k].I];
                }
            }

            Graph resultGraph = new Graph(resultEdges, true);

            return resultGraph;
        }

        static public Graph LabMethod(Graph g, int n1, int n2, int n3)
        {
            Graph tempG = g.Kruskal();

            List<int> nodesWeNeed = new List<int>();

            nodesWeNeed = tempG.DepthFirstSearch(n1, n2);
            nodesWeNeed = new List<int>(nodesWeNeed.Union(tempG.DepthFirstSearch(n1, n3)));
            nodesWeNeed = new List<int>(nodesWeNeed.Union(tempG.DepthFirstSearch(n2, n3)));

            List<Edge> edgesWeNeed = new List<Edge>();
            foreach (Edge e in tempG.edgeList)
                if (nodesWeNeed.Contains(e.I) && nodesWeNeed.Contains(e.J))
                    if (!edgesWeNeed.Contains(e))
                        edgesWeNeed.Add(e);

            return new Graph(edgesWeNeed, true);
        }
    }
}
