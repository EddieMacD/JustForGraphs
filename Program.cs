using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustForGraphs
{
    class Program
    {
        static void Main(string[] args)
        {
            #region nodeConsts
            const string nodeAName = "A";
            const string nodeBName = "B";
            const string nodeCName = "C";
            const string nodeDName = "D";
            const string nodeEName = "E";
            const string nodeFName = "F";
            const string nodeGName = "G";
            const string nodeHName = "H";

            const int AB = 25;
            const int AC = 16;
            const int AH = 14;
            const int BC = 17;
            const int BD = 12;
            const int BE = 18;
            const int CD = 12;
            const int CG = 21;
            const int CH = 20;
            const int DF = 15;
            const int EF = 11;
            const int FG = 24;
            const int GH = 18;
            #endregion

            #region listSetup
            DirectedListGraph<string> listGraph = new DirectedListGraph<string>();
            listGraph.AddNode(nodeAName);
            listGraph.AddNode(nodeBName);
            listGraph.AddNode(nodeCName);
            listGraph.AddNode(nodeDName);
            listGraph.AddNode(nodeEName);
            listGraph.AddNode(nodeFName);
            listGraph.AddNode(nodeGName);
            listGraph.AddNode(nodeHName);

            listGraph.AddUndirectedConnection(nodeAName, nodeBName, AB);
            listGraph.AddUndirectedConnection(nodeAName, nodeCName, AC);
            listGraph.AddUndirectedConnection(nodeAName, nodeHName, AH);
            listGraph.AddUndirectedConnection(nodeBName, nodeCName, BC);
            listGraph.AddUndirectedConnection(nodeBName, nodeDName, BD);
            listGraph.AddUndirectedConnection(nodeBName, nodeEName, BE);
            listGraph.AddUndirectedConnection(nodeCName, nodeDName, CD);
            listGraph.AddUndirectedConnection(nodeCName, nodeGName, CG);
            listGraph.AddUndirectedConnection(nodeCName, nodeHName, CH);
            listGraph.AddUndirectedConnection(nodeDName, nodeFName, DF);
            listGraph.AddUndirectedConnection(nodeEName, nodeFName, EF);
            listGraph.AddUndirectedConnection(nodeFName, nodeGName, FG);
            listGraph.AddUndirectedConnection(nodeGName, nodeHName, GH);
            #endregion

            #region matrixSetup
            DirectedMatrixGraph<string> matrixGraph = new DirectedMatrixGraph<string>(nodeAName);
            matrixGraph.AddNode(nodeBName);
            matrixGraph.AddNode(nodeCName);
            matrixGraph.AddNode(nodeDName);
            matrixGraph.AddNode(nodeEName);
            matrixGraph.AddNode(nodeFName);
            matrixGraph.AddNode(nodeHName);
            matrixGraph.AddNode(nodeGName);

            matrixGraph.AddUndirectedConnection(nodeAName, nodeBName, AB);
            matrixGraph.AddUndirectedConnection(nodeAName, nodeCName, AC);
            matrixGraph.AddUndirectedConnection(nodeAName, nodeHName, AH);
            matrixGraph.AddUndirectedConnection(nodeBName, nodeCName, BC);
            matrixGraph.AddUndirectedConnection(nodeBName, nodeDName, BD);
            matrixGraph.AddUndirectedConnection(nodeBName, nodeEName, BE);
            matrixGraph.AddUndirectedConnection(nodeCName, nodeDName, CD);
            matrixGraph.AddUndirectedConnection(nodeCName, nodeGName, CG);
            matrixGraph.AddUndirectedConnection(nodeCName, nodeHName, CH);
            matrixGraph.AddUndirectedConnection(nodeDName, nodeFName, DF);
            matrixGraph.AddUndirectedConnection(nodeEName, nodeFName, EF);
            matrixGraph.AddUndirectedConnection(nodeFName, nodeGName, FG);
            matrixGraph.AddUndirectedConnection(nodeGName, nodeHName, GH);
            #endregion

            #region mst
            List<string> KruskalList = listGraph.KruskalsAlgorithm();
            List<string> PrimList = listGraph.PrimsAlgorithm("A");
            List<string> KruskalArray = matrixGraph.KruskalsAlgorithm();
            List<string> PrimArray = matrixGraph.PrimsAlgorithm("A");

            Console.WriteLine("Kruskal: List");
            foreach (string connection in KruskalList)
            {
                Console.WriteLine(connection);
            }
            Console.WriteLine();

            Console.WriteLine("Kruskal: Array");
            foreach (string connection in KruskalArray)
            {
                Console.WriteLine(connection);
            }
            Console.WriteLine();

            Console.WriteLine("Prim: List");
            foreach (string connection in PrimList)
            {
                Console.WriteLine(connection);
            }
            Console.WriteLine();

            Console.WriteLine("Prim: Array");
            foreach (string connection in PrimArray)
            {
                Console.WriteLine(connection);
            }
            #endregion

            Console.ReadLine();
        }
    }
}
