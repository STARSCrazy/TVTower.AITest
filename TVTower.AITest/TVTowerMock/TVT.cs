using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TVTower.AITest.TVTowerMock
{
    public class TVT
    {
        public void AddToLog( string message )
        {
            //Console.WriteLine( message );
            System.Diagnostics.Trace.WriteLine( message );
        }

        public float GetMillisecs()
        {
            return 0;
        }
    }
}
