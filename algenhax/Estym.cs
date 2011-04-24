using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;

namespace algenhax
{
    class Estym
    {
        public Estym(int sleepTime = 50)
        {
            this.sleepTime = sleepTime;
        }

        int sleepTime;
        IntPtr estymHwnd;

        public static string[] GenetyczneW = { "ok", "cancel", "populacja", null, "pokolenia", null, "pk", null, "pm", null, "skalowanie", "elitaryzm" };
        public static string[] SprawdzanieW = { "cancel", null, "1", null, "2", null, "3", null, "4", null, "5", null, "6", null, "7", null, "8", null, "9", null, "10", null, "wynik", "oblicz" };

        public struct ParametryGenetyczne
        {
            public int l_osobnikow;
            public int l_pokolen;
            public double p_krzyzowania;
            public double p_mutacji;
            public bool skalowanie;
            public bool elitaryzm;
        }

        public void setParametryGenetyczne(ParametryGenetyczne p)
        {
            IntPtr genHwnd = winapi.FindWindow("#32770", "Parametry genetyczne");
            if (genHwnd == IntPtr.Zero)
            {
                winapi.PostMessage(estymHwnd, winapi.WM_COMMAND, (IntPtr)4002, IntPtr.Zero);
                genHwnd = winapitools.WaitForWindow("#32770", "Parametry genetyczne");
            }

            Thread.Sleep(sleepTime);

            Dictionary<object, IntPtr> dict = new Dictionary<object, IntPtr>();
            winapitools.findWindowHandles(genHwnd, GenetyczneW, dict);

            winapi.managedSetText(dict["populacja"], p.l_osobnikow.ToString());
            winapi.managedSetText(dict["pokolenia"], p.l_pokolen.ToString());
            winapi.managedSetText(dict["pk"], p.p_krzyzowania.ToString().Replace(',', '.'));
            winapi.managedSetText(dict["pm"], p.p_mutacji.ToString().Replace(',', '.'));
            winapi.SendMessage(dict["skalowanie"], winapi.BM_SETCHECK, (p.skalowanie ? (IntPtr)1 : (IntPtr)0), IntPtr.Zero);
            winapi.SendMessage(dict["elitaryzm"], winapi.BM_SETCHECK, (p.elitaryzm ? (IntPtr)1 : (IntPtr)0), IntPtr.Zero);

            Thread.Sleep(sleepTime);

            winapi.SendMessage(dict["ok"], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);

            Thread.Sleep(sleepTime);
        }

        public void wykonajObliczenia()
        {
            winapi.PostMessage(estymHwnd, winapi.WM_COMMAND, (IntPtr)4005, IntPtr.Zero);

            while (winapi.FindWindow("#32770", "Postęp symulacji") != IntPtr.Zero)
            {
                Thread.Sleep(sleepTime);
            }

            Thread.Sleep(sleepTime);
        }

        public double[] sprawdz()
        {
            IntPtr sprHwnd = winapi.FindWindow("#32770", "Sprawdzanie wartości funkcji celu");
            if (sprHwnd == IntPtr.Zero)
            {
                winapi.PostMessage(estymHwnd, winapi.WM_COMMAND, (IntPtr)4007, IntPtr.Zero);
                sprHwnd = winapitools.WaitForWindow("#32770", "Sprawdzanie wartości funkcji celu");
            }

            Thread.Sleep(sleepTime);

            Dictionary<object, IntPtr> dict = new Dictionary<object, IntPtr>();
            winapitools.findWindowHandles(sprHwnd, SprawdzanieW, dict);

            winapi.SendMessage(dict["oblicz"], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);

            double[] result = new double[11];

            result[0] = Double.Parse(winapi.managedGetText(dict["wynik"]).Replace('.',','));
            for (int i = 1; i <= 10; i++)
            {
                result[i] = Double.Parse(winapi.managedGetText(dict[i.ToString()]).Replace('.', ','));
            }

            //winapi.SendMessage(sprHwnd, winapi.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            winapi.SendMessage(dict["cancel"], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);

            Thread.Sleep(sleepTime);

            return result;
        }

        public void init()
        {
            estymHwnd = winapi.FindWindow3(IntPtr.Zero, "ESTYM");
            if (estymHwnd == IntPtr.Zero)
                throw new Exception("Cannot find Estym window.");
        }
    }
}
