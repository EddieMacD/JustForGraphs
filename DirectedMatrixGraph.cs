using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForGraphs
{
    class DirectedMatrixGraph<dataType>
    {
        private List<dataType> Nodes;
        private List<List<int>> Matrix;
        private int Size;
        private bool Undirected;

        /// <summary>
        /// An implementation of a graph via an adjacency matrix. Works for both directed and undirected graphs
        /// </summary>
        /// <param name="node1">The name of the first node</param>
        public DirectedMatrixGraph(dataType node1)
        {
            Size = 1;
            Nodes = new List<dataType> { node1 };
            Matrix = new List<List<int>> { new List<int> { 0 } };
            Undirected = true;
        }

        public void AddNode(dataType nodeName)
        {
            List<int> newNode = new List<int> { 0 };

            for (int i = 0; i < Size; i++)
            {
                Matrix[i].Add(0);
                newNode.Add(0);
            }

            Nodes.Add(nodeName);
            Size++;
            Matrix.Add(newNode);
        }

        public void AddDirectedConnection(dataType startNode, dataType endNode, int weight)
        {
            int startIndex = NodeIndexLookup(startNode);
            int endIndex = NodeIndexLookup(endNode);

            if(startIndex != int.MinValue || endIndex != int.MinValue)
            {
                Matrix[startIndex][endIndex] = weight;
            }

            Undirected = false;
        }

        public void AddUndirectedConnection(dataType nodeA, dataType nodeB, int weight)
        {
            int indexA = NodeIndexLookup(nodeA);
            int indexB = NodeIndexLookup(nodeB);

            if (indexA != int.MinValue || indexB != int.MinValue)
            {
                Matrix[indexA][indexB] = weight;
                Matrix[indexB][indexA] = weight;
            }
        }

        public void RemoveNode(dataType nodeName)
        {
            int index = NodeIndexLookup(nodeName);

            if (index > -1)
            {
                Nodes.RemoveAt(index);
                Matrix.RemoveAt(index);
                Size--;

                for (int i = 0; i < Size; i++)
                {
                    Matrix[i].RemoveAt(index);
                }
            }
        }

        public void RemoveUndirectedConnection(dataType nodeA, dataType nodeB)
        {
            AddUndirectedConnection(nodeA, nodeB, 0);
        }

        public void PrintMatrix()
        {
            for (int i = 0; i < Size; i++)
            {
                Console.Write(EqualWrite(Nodes[i] + ":", 20));

                for (int j = 0; j < Size; j++)
                {
                    Console.Write(EqualWrite(Matrix[i][j].ToString(), 6));
                }

                Console.WriteLine("");
            }
        }

        public bool NeighbourCheck(dataType nodeA, dataType nodeB)
        {
            int indexA = NodeIndexLookup(nodeA);
            int indexB = NodeIndexLookup(nodeB);
            bool neighbours = false;

            if (indexA >= 0 && indexB >= 0)
            {
                if (Matrix[indexA][indexB] != 0)
                {
                    neighbours = true;
                }
            }

            return neighbours;
        }

        public int NeighbourDistance(dataType nodeA, dataType nodeB)
        {
            int indexA = NodeIndexLookup(nodeA);
            int indexB = NodeIndexLookup(nodeB);
            int weight = int.MinValue;
            bool neighbours = NeighbourCheck(nodeA, nodeB);

            if (neighbours)
            {
                weight = Matrix[indexA][indexB];
            }

            return weight;
        }

        private int NodeIndexLookup(dataType nodeName)
        {
            int index = int.MinValue;

            for (int i = 0; i < Size; i++)
            {
                if (Nodes[i].ToString() == nodeName.ToString())
                {
                    index = i;
                    i = int.MaxValue - 1;
                }
            }

            return index;
        }

        private dataType NodeNameLookup (int index)
        {
            dataType name = Nodes[0];

            for (int i = 0; i < Size; i++)
            {
                if (i == index)
                {
                    name = Nodes[i];
                    i = int.MaxValue - 1;
                }
            }

            return name;
        }

        private string EqualWrite(string input, int length)
        {
            return input.PadRight(length);
        }

        public bool DepthFirstBool (List<dataType> nodes, dataType currentNode, dataType targetNode)
        {
            bool target = false;
            
            if (currentNode.ToString() != targetNode.ToString() && target != true)
            {
                nodes.Add(currentNode);

                for (int i = 0; i < Size; i++)
                {
                    int edge = Matrix[NodeIndexLookup(currentNode)][i];
                    dataType newNode = NodeNameLookup(i);

                    if (!nodes.Contains(newNode) && edge != 0 && !target)
                    {
                        target = DepthFirstBool(nodes, newNode, targetNode);
                    }
                }
            }
            else
            {
                target = true;
            }

            return target;
        }

        public List<string> KruskalsAlgorithm()
        {
            List<(int, int, int)> minimumSpanningTreeEdges = new List<(int, int, int)>();
            DirectedMatrixGraph<dataType> minimumSpanningTree = new DirectedMatrixGraph<dataType>(Nodes[0]);
            List<(int, int, int)> edges = new List<(int, int, int)>();
            int mstEdges = 0;

            for (int i = 1; i < Nodes.Count; i++)
            {
                minimumSpanningTree.AddNode(Nodes[i]);
            }

            for (int i = 0; i < Size; i++)
            {
                List<int> node = Matrix[i];

                for (int j = 0; j < Size; j++)
                {
                    int edge = node[j];

                    if (!edges.Contains((j, i, edge)) && edge != 0)
                    {
                        edges.Add((i, j, edge));
                    }
                }
            }

            BubbleSortEdges(edges);

            foreach ((int, int, int) edge in edges)
            {
                if (mstEdges < Nodes.Count - 1)
                {
                    dataType start = NodeNameLookup(edge.Item1);
                    dataType destination = NodeNameLookup(edge.Item2);

                    if (!minimumSpanningTree.DepthFirstBool(new List<dataType>(), start, destination))
                    {
                        minimumSpanningTree.AddUndirectedConnection(NodeNameLookup(edge.Item1), NodeNameLookup(edge.Item2), edge.Item3);
                        minimumSpanningTreeEdges.Add((edge.Item1, edge.Item2, edge.Item3));
                        mstEdges++;
                    }
                }
            }

            return ListConvert(minimumSpanningTreeEdges);
        }

        public void BubbleSortEdges (List<(int, int, int)> edges)
        {
            bool changes = true;
            int i = edges.Count() - 1;

            while (changes && i > 0)
            {
                changes = false;

                for (int j = 0; j < i; j++)
                {
                    if (edges[j].Item3 > edges[j + 1].Item3)
                    {
                        (int, int, int) temp = edges[j];
                        edges[j] = edges[j + 1];
                        edges[j + 1] = temp;
                        changes = true;
                    }
                }

                i--;
            }
        }

        private List<string> ListConvert(List<(int, int, int)> edges)
        {
            List<string> stringList = new List<string>();

            BubbleSortEdges(edges);

            foreach ((int, int, int) edge in edges)
            {
                stringList.Add(NodeNameLookup(edge.Item1) + " to " + NodeNameLookup(edge.Item2) + ", " + edge.Item3);
            }

            return stringList;
        }

        public List<string> PrimsAlgorithm(dataType startNodeName)
        {
            List<(int, int, int)> minimumSpanningTree = new List<(int, int, int)>();
            List<int> mstNodeIndexes = new List<int> { NodeIndexLookup(startNodeName) };

            for (int n = 0; n < Size - 1; n++)
            {
                int comparer = int.MaxValue;
                int startNodeIndex = 0;
                int endNodeIndex = 0;

                for (int i = 0; i < Size; i++)
                {

                    if (!mstNodeIndexes.Contains(i))
                    {
                        List<int> node = Matrix[i];

                        for (int j = 0; j < Size; j++)
                        {
                            if (mstNodeIndexes.Contains(j))
                            {
                                if (node[j] < comparer && node[j] > 0)
                                {
                                    comparer = node[j];
                                    startNodeIndex = j;
                                    endNodeIndex = i;
                                }
                            }
                        }
                    }
                }

                mstNodeIndexes.Add(endNodeIndex);
                minimumSpanningTree.Add((startNodeIndex, endNodeIndex, Matrix[startNodeIndex][endNodeIndex]));
            }

            return ListConvert(minimumSpanningTree);
        }
    }
}
