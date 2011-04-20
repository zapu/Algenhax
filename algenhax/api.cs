using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace algenhax
{
    class api
    {
        public delegate bool CallBackPtr(int hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBackPtr callPtr, int lPar);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, CallBackPtr callback, int lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow2(string lpClassName, IntPtr lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage2(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        public static void checkWin32Error()
        {
            int lastError = Marshal.GetLastWin32Error();
            if (lastError != 0)
                throw new Exception("Win32 error " + lastError);
        }

        public static string managedGetClassName(IntPtr hWnd)
        {
            StringBuilder builder = new StringBuilder(128);
            GetClassName(hWnd, builder, 128);
            checkWin32Error();
            return builder.ToString();
        }

        public static string managedGetWindowText(IntPtr hWnd)
        {
            StringBuilder builder = new StringBuilder(128);
            GetWindowText(hWnd, builder, 128);
            checkWin32Error();
            return builder.ToString();
        }

        public const UInt32 EM_SETSEL = 0x00B1;
        public const UInt32 EM_REPLACESEL = 0x00C2;

        public const UInt32 BM_CLICK = 0x00F5;

        public const UInt32 WM_SETTEXT = 0x000C;
        public const UInt32 WM_COMMAND = 0x0111;

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUITEMINFO
        {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public int wID;
            public int hSubMenu;
            public int hbmpChecked;
            public int hbmpUnchecked;
            public int dwItemData;
            public string dwTypeData;
            public uint cch;
            public int hbmpItem;
        }

        public const UInt32 MIIM_MAXHEIGHT = 0x00000001;
        public const UInt32 MIIM_BACKGROUND = 0x00000002;
        public const UInt32 MIIM_HELPID = 0x00000004;
        public const UInt32 MIIM_MENUDATA = 0x00000008;
        public const UInt32 MIIM_STYLE = 0x00000010;
        public const UInt32 MIIM_APPLYTOSUBMENUS = 0x80000000;
        public const UInt32 MIIM_DATA = 0x00000020;
        public const UInt32 MIIM_ID = 0x00000002;
        public const UInt32 MIIM_TYPE = 0x00000010;
        public const UInt32 MIIM_SUBMENU = 0x00000004;

        public const UInt32 MFT_STRING = 0x00000000;

        [DllImport("user32.dll")]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [DllImport("user32.dll")]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);
    }
}
