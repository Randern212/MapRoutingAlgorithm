using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{

    class Queury
    {
        float sourceX;
        float sourceY;
        float destinationX;
        float destinationY;
        float R;
        public Queury() { }
        Func<float, float, float, float,float> euclideanDistance = (x1, y1, x2, y2) => MathF.Sqrt(((x1-x2)*(x1-x2))+((y1-y2)*(y1-y2))) ; 
        public void measureTrip()
        {

        }
    }
    class QueryConstructor
    {
        public static Queury construct()
        {
            Queury queury = new Queury();

            return queury;
        }
    }
}
