using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForGraphs
{
    class DirectedListGraph<dataType>
    {
        private List<Node<dataType>> Nodes;
        private bool Undirected;

        public DirectedListGraph()
        {
            Nodes = new List<Node<dataType>>();
            Undirected = true;
        }

        public void AddNode(dataType name)
        {
            Nodes.Add(new Node<dataType>(name));
        }

        public void AddUndirectedConnection(dataType nodeAName, dataType nodeBName, int weight)
        {
            Node<dataType> nodeA = NodeLookup(nodeAName);
            Node<dataType> nodeB = NodeLookup(nodeBName);

            nodeA.AssignNeighbour(nodeB, weight);
            nodeB.AssignNeighbour(nodeA, weight);
        }

        public void AddDirectedConnection(dataType startNodeName, dataType endNodeName, int weight)
        {
            Node<dataType> startNode = NodeLookup(startNodeName);
            Node<dataType> endNode = NodeLookup(endNodeName);

            startNode.AssignNeighbour(endNode, weight);
            Undirected = false;
        }

        public void RemoveUndirectedConnection(dataType nodeAName, dataType nodeBName)
        {
            Node<dataType> nodeA = NodeLookup(nodeAName);
            Node<dataType> nodeB = NodeLookup(nodeBName);

            nodeA.RemoveNeighbour(nodeB);
            nodeB.RemoveNeighbour(nodeA);
        }

        public void RemoveDirectedConnection(dataType startNodeName, dataType endNodeName)
        {
            Node<dataType> startNode = NodeLookup(startNodeName);
            Node<dataType> endNode = NodeLookup(endNodeName);

            startNode.RemoveNeighbour(endNode);
        }

        private Node<dataType> NodeLookup(dataType name)
        {
            int index = int.MinValue;
            Comparer<dataType> comparer = Comparer<dataType>.Default;

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (comparer.Compare(Nodes[i].GetName(), name) == 0)
                {
                    index = i;
                    i = int.MaxValue - 1;
                }
            }

            return Nodes[index];
        }

        private int NodeIndexLookup(dataType name)
        {
            int index = int.MinValue;
            Comparer<dataType> comparer = Comparer<dataType>.Default;

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (comparer.Compare(Nodes[i].GetName(), name) == 0)
                {
                    index = i;
                    i = int.MaxValue - 1;
                }
            }

            return index;
        }

        public void RemoveNode(dataType name)
        {
            Node<dataType> node = NodeLookup(name);

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i] == node)
                {
                    Nodes.RemoveAt(i);
                    i--;
                }
                else
                {
                    Nodes[i].RemoveNeighbour(node);
                }
            }
        }

        #region Search
        public List<dataType> BreadthFirstSearch(dataType seedNodeName)
        {
            Queue<Node<dataType>> queue = new Queue<Node<dataType>>();
            List<dataType> nodeOrder = new List<dataType>();
            Node<dataType> seedNode = NodeLookup(seedNodeName);

            queue.Enqueue(seedNode);
            seedNode.ChangeSearchFlag(true);

            while (queue.Count != 0)
            {
                Node<dataType> current = queue.Dequeue();

                nodeOrder.Add(current.GetName());

                current.BreadthFirstSearch(queue, nodeOrder);
            }

            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].ChangeSearchFlag(false);
            }

            return nodeOrder;
        }

        public List<dataType> DepthFirst(dataType seedNodeName)
        {
            List<dataType> nodes = new List<dataType>();
            Node<dataType> seedNode = NodeLookup(seedNodeName);
            seedNode.DepthFirst(nodes);

            foreach (Node<dataType> node in Nodes)
            {
                node.ChangeSearchFlag(false);
            }

            return nodes;
        }

        public bool DepthFirstBool(dataType seedNodeName, dataType targetNodeName)
        {
            List<dataType> nodes = new List<dataType>();
            Node<dataType> seedNode = NodeLookup(seedNodeName);
            Node<dataType> targetNode = NodeLookup(targetNodeName);
            bool found = seedNode.DepthFirstBool(nodes, targetNode);

            foreach (Node<dataType> node in Nodes)
            {
                node.ChangeSearchFlag(false);
            }

            return found;
        }
        #endregion

        #region MST
        public List<string> KruskalsAlgorithm()
        {
            List<Edge<dataType>> connections = new List<Edge<dataType>>();
            DirectedListGraph<dataType> minimumSpanningTree = new DirectedListGraph<dataType>();
            int mstEdges = 0;

            for (int i = 0; i < Nodes.Count; i++)
            {
                minimumSpanningTree.AddNode(Nodes[i].GetName());
            }

            foreach (Node<dataType> node in Nodes)
            {
                foreach (Edge<dataType> edge in node.GetNeighbours())
                {
                    Edge<dataType> temp = new Edge<dataType>(edge.GetDestination(), edge.GetStart(), edge.GetWeight());
                    bool duplicate = false;

                    foreach(Edge<dataType> connection in connections)
                    {
                        if(connection.ToString() == temp.ToString())
                        {
                            duplicate = true;
                        }
                    }

                    if(!duplicate)
                    {
                        connections.Add(edge);
                    }
                }
            }

            BubbleSortEdges(connections);

            foreach (Edge<dataType> edge in connections)
            {
                if (mstEdges < Nodes.Count - 1)
                {
                    dataType start = edge.GetStart().GetName();
                    dataType destination = edge.GetDestination().GetName();

                    if (!minimumSpanningTree.DepthFirstBool(start, destination))
                    {
                        minimumSpanningTree.AddUndirectedConnection(edge.GetStart().GetName(), edge.GetDestination().GetName(), edge.GetWeight());
                        mstEdges++;
                    }
                }
            }

            return ListConvert(minimumSpanningTree);
        }

        private void BubbleSortEdges(List<Edge<dataType>> edges)
        {
            bool changes = true;
            int i = edges.Count() - 1;

            while (changes && i > 0)
            {
                changes = false;

                for (int j = 0; j < i; j++)
                {
                    if (edges[j].GetWeight() > edges[j + 1].GetWeight())
                    {
                        Edge<dataType> temp = edges[j];
                        edges[j] = edges[j + 1];
                        edges[j + 1] = temp;
                        changes = true;
                    }
                }

                i--;
            }
        }

        public List<string> PrimsAlgorithm(dataType startNodeName)
        {
            DirectedListGraph<dataType> minimumSpanningTree = new DirectedListGraph<dataType>();
            List<Edge<dataType>> edges = new List<Edge<dataType>>();
            List<dataType> mstNodes = new List<dataType>();
            dataType newNodeName = startNodeName;

            minimumSpanningTree.AddNode(newNodeName);
            mstNodes.Add(newNodeName);

            for (int i = 1; i < Nodes.Count; i++)
            {
                edges.AddRange(NodeLookup(newNodeName).GetNeighbours());
                BubbleSortEdges(edges);

                for (int j = 0; j < edges.Count(); j++)
                {
                    Edge<dataType> edge = edges[j];
                    newNodeName = edge.GetDestination().GetName();

                    if (!mstNodes.Contains(newNodeName))
                    {
                        mstNodes.Add(newNodeName);
                        minimumSpanningTree.AddNode(newNodeName);
                        minimumSpanningTree.AddUndirectedConnection(edge.GetStart().GetName(), newNodeName, edge.GetWeight());
                        edges.RemoveAt(j);
                        j = int.MaxValue - 1;
                    }
                }
            }

            return ListConvert(minimumSpanningTree);
        }

        private List<string> ListConvert (DirectedListGraph<dataType> graph)
        {
            List<Edge<dataType>> connections = new List<Edge<dataType>>();
            List<string> stringList = new List<string>();

            foreach (Node<dataType> node in graph.Nodes)
            {
                foreach (Edge<dataType> edge in node.GetNeighbours())
                {
                    Edge<dataType> temp = new Edge<dataType>(edge.GetDestination(), edge.GetStart(), edge.GetWeight());
                    bool duplicate = false;

                    foreach (Edge<dataType> connection in connections)
                    {
                        if (connection.ToString() == temp.ToString())
                        {
                            duplicate = true;
                        }
                    }

                    if (!duplicate)
                    {
                        connections.Add(edge);
                    }
                }
            }

            BubbleSortEdges(connections);

            foreach (Edge<dataType> connection in connections)
            {
                stringList.Add(connection.ToString());
            }

            return stringList;
        }
        #endregion
    }
}
