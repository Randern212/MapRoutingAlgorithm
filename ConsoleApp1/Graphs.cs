using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    class Vertex
    {
        public float positionX;
        public float positionY;
        Dictionary<int, Edge> edges;
    }

    class Edge
    {
        public float length;
        public float speed;
        public float time => length/speed;
    }

    class Graph
    { 
        Dictionary<int, Vertex> vertices;
        public void findPath(int rootID)
        {

        }
    }

    class GraphConstructor
    {

    }

}
