using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace algenhax
{
    class Algen
    {
        public Algen()
        {
            handles = new IntPtr[(int)AlgenHandle.LAST];
        }

        enum AlgenHandle
        {
            KRZYZOWANIA = 0,
            MUTACJI = 1,
            LABEL = 2,
            START = 3,
            LAST,
        }

        IntPtr[] handles;

        int enumIt = 0;
        private bool callback(int hwnd, int lptr)
        {
            StringBuilder sb = new StringBuilder(100);
            String className;
            String text;

            api.GetClassName((IntPtr)hwnd, sb, 100);
            className = sb.ToString();
            api.GetWindowText((IntPtr)hwnd, sb, 100);
            text = sb.ToString();

            if (className == "Edit")
            {
                if (enumIt == 2 && handles[(int)AlgenHandle.KRZYZOWANIA] == IntPtr.Zero)
                    handles[(int)AlgenHandle.KRZYZOWANIA] = (IntPtr)hwnd;
                if (enumIt == 3 && handles[(int)AlgenHandle.MUTACJI] == IntPtr.Zero)
                    handles[(int)AlgenHandle.MUTACJI] = (IntPtr)hwnd;

                enumIt++;
            }
            else if (className == "Static")
            {
                handles[(int)AlgenHandle.LABEL] = (IntPtr)hwnd;
            }
            else if (className == "Button" && text == "Start" && handles[(int)AlgenHandle.START] == IntPtr.Zero)
            {
                handles[(int)AlgenHandle.START] = (IntPtr)hwnd;
            }

            return true;
        }

        public bool init()
        {
            //IntPtr hwnd = api.FindWindow("#32770", "Wyniki");
            IntPtr hwnd = api.FindWindow2("#32770", IntPtr.Zero);
            if (hwnd == IntPtr.Zero)
            {
                throw new Exception("Cannot find window.");
            }

            api.EnumChildWindows(hwnd, callback, 0);

            checkWin32Error();

            if(handles.FirstOrDefault((ptr) => ptr != (IntPtr)0) == (IntPtr)0)
                throw new Exception("Handles not found.");

            StringBuilder sb = new StringBuilder(100);
            api.GetWindowText(hwnd, sb, 100);
            Console.WriteLine("Window: " + hwnd + " " + sb.ToString());

            setWindowText(handles[(int)AlgenHandle.LABEL], "liga rzadzi!");

            return true;
        }

        private void checkWin32Error()
        {
            int lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
                throw new Exception("Win32 error " + lastError);
        }

        private void setWindowText(IntPtr handle, string val)
        {
            IntPtr strPtr = Marshal.StringToHGlobalAuto(val);
            api.SendMessage(handle, api.WM_SETTEXT, (IntPtr)(1), strPtr);
            Marshal.FreeHGlobal(strPtr);
            checkWin32Error();
        }

        public void setKrzyzowania(string val)
        {
            setWindowText(handles[(int)AlgenHandle.KRZYZOWANIA], val);
        }

        public void setMutacji(string val)
        {
            setWindowText(handles[(int)AlgenHandle.MUTACJI], val);
        }

        public void sendStart()
        {
            api.SendMessage(handles[(int)AlgenHandle.START], api.BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            checkWin32Error();
        }
    }
}
