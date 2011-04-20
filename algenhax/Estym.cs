using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace algenhax
{
    class Estym
    {
        public Estym()
        {

        }

        IntPtr estymHwnd;

        public static string[] GenetyczneW = { "ok", "cancel", "populacja", null, "pokolenia", null, "pk", null, "pm", null, "skalowanie", "elitarnosc" };
        public static string[] SprawdzanieW = { "cancel", null, "1", null, "2", null, "3", null, "4", null, "5", null, "6", null, "7", null, "8", null, "9", null, "10", null, "wynik", "oblicz" };


        public void menusIterate(IntPtr hMenu, bool moar)
        {
            int menuCount = winapi.GetMenuItemCount(hMenu);
            for (int i = 0; i < menuCount; i++)
            {
                winapi.MENUITEMINFO mif = new winapi.MENUITEMINFO();
                mif.cbSize = (uint)Marshal.SizeOf(typeof(winapi.MENUITEMINFO));
                mif.fMask = winapi.MIIM_TYPE | winapi.MIIM_ID | winapi.MIIM_SUBMENU;
                mif.fType = winapi.MFT_STRING;
                mif.cch = 255;
                mif.dwTypeData = new string((char)0, 255);
                bool a = winapi.GetMenuItemInfo(hMenu, (uint)i, true, ref mif);
                if (a)
                {
                    if (!moar)
                        Console.Write("\t");
                    Console.WriteLine(i + " " + mif.wID + " " + mif.hSubMenu + " " + mif.dwTypeData);
                    menusIterate((IntPtr)mif.hSubMenu, false);
                }
            }
        }

        public void init()
        {
            estymHwnd = winapi.FindWindow("Afx:400000:8:10003:6:1b0579", "ESTYM");
            if (estymHwnd == IntPtr.Zero)
                throw new Exception("Cannot find Estym window.");

            Console.WriteLine("Found estym hwnd " + estymHwnd);

            IntPtr hMenu = winapi.GetMenu(estymHwnd);

            Console.WriteLine("Menu found " + hMenu);

            //menusIterate(hMenu, true);

            //winapi.PostMessage(estymHwnd, winapi.WM_COMMAND, (IntPtr)4002, IntPtr.Zero);

            IntPtr genhwnd = winapitools.WaitForWindow("#32770", "Sprawdzanie wartości funkcji celu");

            //winapitools.enumChildWindows(genhwnd);

            Dictionary<object, IntPtr> dict = new Dictionary<object,IntPtr>();
            winapitools.findWindowHandles(genhwnd, SprawdzanieW, dict);

            winapi.managedSetText(dict["wynik"], "roflol2");
            //winapi.SendMessage(genhwnd, winapi.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

        }
    }
}
