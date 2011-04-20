using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algenhax
{
    class apitools
    {
        private static bool callback(int hwnd, int lptr)
        {
            try
            {
                String className = api.managedGetClassName((IntPtr)hwnd);
                String text = api.managedGetWindowText((IntPtr)hwnd);

                Console.WriteLine("Window " + className + " - " + text);
            }
            catch (Exception e)
            {
                Console.WriteLine("info on " + hwnd + " failed");
            }
            return true;
        }

        public static void enumWindows()
        {
            api.EnumWindows(callback, 0);
        }
    }
}
