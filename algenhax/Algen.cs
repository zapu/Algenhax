﻿using System;
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
            String className = winapi.managedGetClassName((IntPtr)hwnd);
            String text = winapi.managedGetWindowText((IntPtr)hwnd);

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
            IntPtr hwnd = winapi.FindWindow("#32770", "Obliczenia");
            
            if (hwnd == IntPtr.Zero)
            {
                throw new Exception("Cannot find window.");
            }

            String caption = winapi.managedGetWindowText(hwnd);
            Console.WriteLine("Window found: " + hwnd + " " + caption);

            winapi.EnumChildWindows(hwnd, callback, 0);

            winapi.checkWin32Error();

            if(handles.FirstOrDefault((ptr) => ptr != (IntPtr)0) == (IntPtr)0)
                throw new Exception("Handles not found.");

            setWindowText(handles[(int)AlgenHandle.LABEL], "liga rzadzi!");

            return true;
        }

        private void setWindowText(IntPtr handle, string val)
        {
            IntPtr strPtr = Marshal.StringToHGlobalAuto(val);
            winapi.SendMessage(handle, winapi.WM_SETTEXT, (IntPtr)(1), strPtr);
            Marshal.FreeHGlobal(strPtr);
            winapi.checkWin32Error();
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
            winapi.SendMessage(handles[(int)AlgenHandle.START], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            winapi.checkWin32Error();
        }
    }
}
