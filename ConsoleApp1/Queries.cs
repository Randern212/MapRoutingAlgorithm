using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{

    class Queury
    {
        public Queury() { }

        float sourceX;
        float sourceY;
        float destinationX;
        float destinationY;
        float R;
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
