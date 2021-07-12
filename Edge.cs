using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForGraphs
{
    class Edge<dataType>
    {
        private Node<dataType> Start;
        private Node<dataType> Destination;
        private int Weight;

        public Edge(Node<dataType> start, Node<dataType> destination, int weight)
        {
            Start = start;
            Destination = destination;
            Weight = weight;
        }

        public Node<dataType> GetStart()
        {
            return Start;
        }

        public Node<dataType> GetDestination()
        {
            return Destination;
        }

        public int GetWeight()
        {
            return Weight;
        }

        public string ToString()
        {
            return Start.GetName().ToString() + " to " + Destination.GetName().ToString() + ", " + Weight.ToString();
        }
    }
}
