using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace algenhax
{
    class winapitools
    {
        private static bool callback(int hwnd, int lptr)
        {
            try
            {
                String className = winapi.managedGetClassName((IntPtr)hwnd);
                String text = winapi.managedGetWindowText((IntPtr)hwnd);

                Console.WriteLine(className + " - " + text);

                if (className == "Edit")
                {
                    int textLen = (int)winapi.SendMessage((IntPtr)hwnd, winapi.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                    StringBuilder sb = new StringBuilder(textLen);
                    winapi.SendMessage2((IntPtr)hwnd, winapi.WM_GETTEXT, textLen, sb);
                    winapi.checkWin32Error();
                    Console.WriteLine("\ttext: (" + textLen + ") " + sb.ToString());
                }
                else if (className == "Button")
                {
                    int chk = (int)winapi.SendMessage((IntPtr)hwnd, winapi.BM_GETCHECK, IntPtr.Zero, IntPtr.Zero);
                    Console.WriteLine("\tChecked: " + chk);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("info on " + hwnd + " failed " + e);
            }
            return true;
        }

        public static void enumWindows()
        {
            winapi.EnumWindows(callback, 0);
        }

        public static void enumChildWindows(IntPtr hwnd)
        {
            winapi.EnumChildWindows(hwnd, callback, 0);
        }

        public static IntPtr WaitForWindow(string className, string windowText)
        {
            StringBuilder builder = new StringBuilder(128);
            while (true)
            {
                IntPtr hwnd = winapi.FindWindow(className, windowText);
                winapi.GetWindowText(hwnd, builder, 128);
                //LastWin32Error acts weird here for some reason...
                if (/*Marshal.GetLastWin32Error() == 0 && */builder.ToString() == windowText)
                    return hwnd;

                builder.Clear();
                System.Threading.Thread.Sleep(5);
            }
        }

        public static void findWindowHandles(IntPtr windowHwnd, object[] keys, Dictionary<object, IntPtr> dict)
        {
            int keyIter = 0;
            winapi.CallBackPtr cb = delegate(int hwnd, int lptr)
            {
                if (keyIter < keys.Length && keys[keyIter] != null)
                    dict.Add(keys[keyIter], (IntPtr)hwnd);

                keyIter++;

                return true;
            };

            winapi.EnumChildWindows(windowHwnd, cb, 0);
        }
    }
}
