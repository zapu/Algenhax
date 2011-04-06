using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
   
using System.Runtime.InteropServices;

namespace algenhax
{
    class Program
    {
        static void Main(string[] args)
        {
            Algen alg = new Algen();
            alg.init();

            alg.setKrzyzowania((0.01).ToString());
            alg.sendStart();
        }
    }
}
