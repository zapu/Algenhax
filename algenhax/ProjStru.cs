using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;

namespace algenhax
{
    class ProjStru
    {
        const uint magicAddr = 0x004C80FA;
        const uint resultAddr = 0x004C80FD;

        public ProjStru(int sleepTime = 50)
        {
            this.sleepTime = sleepTime;
        }

        IntPtr mainHwnd;
        uint processId;
        IntPtr hProcess;
        int sleepTime;

        bool readingEnabled = false;

        Dictionary<object, IntPtr> mainDict = new Dictionary<object, IntPtr>();

        public static string[] mainW = { null, "go", "label", "ustawienia", "pokaz", null, null, "wirtualizacja" };
        public static string[] paramW = { "ok", "cancel", "l_o", null, "l_p", null, "p_k", null, "p_m", null, "skalowanie", "elitaryzm", null, null, null, null, "rand" };

        public void init()
        {
            mainHwnd = winapi.FindWindow3(IntPtr.Zero, "ProjStru");
            winapi.GetWindowThreadProcessId(mainHwnd, out processId);

            hProcess = winapi.OpenProcess(winapi.PROCESS_VM_READ, false, processId);
            if (hProcess != IntPtr.Zero)
            {
                byte[] buffer = new byte[2];
                int bytesRead;
                winapi.ReadProcessMemory(hProcess, (IntPtr)magicAddr, buffer, 2, out bytesRead);
                
                //Read magic, set by dll.
                if (bytesRead == 2 && buffer.SequenceEqual(new byte[] { 0xbe, 0xef }))
                    readingEnabled = true;
            }

            winapitools.findWindowHandles(mainHwnd, mainW, mainDict);
         }

        public struct ParametryZadania
        {
            public int l_osobnikow;
            public int l_pokolen;
            public double p_krzyzowania;
            public double p_mutacji;
            public bool skalowanie;
            public bool elitaryzm;
            public int rand;
        }

        public void setParametry(ParametryZadania zad)
        {
            IntPtr genHwnd = winapi.FindWindow("#32770", "Parametry zadania i algorytmu ewolucyjnego");
            while(genHwnd == IntPtr.Zero)
            {
                //Keep clicking, sometimes it just "highlights" the button for some reason
                winapi.PostMessage(mainDict["ustawienia"], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                genHwnd = winapi.FindWindow("#32770", "Parametry zadania i algorytmu ewolucyjnego");

                Thread.Sleep(50);
            }

            Thread.Sleep(sleepTime);

            Dictionary<object, IntPtr> dict = new Dictionary<object, IntPtr>();
            winapitools.findWindowHandles(genHwnd, paramW, dict);

            winapi.managedSetText(dict["l_o"], zad.l_osobnikow.ToString());
            winapi.managedSetText(dict["l_p"], zad.l_pokolen.ToString());
            winapi.managedSetText(dict["p_k"], zad.p_krzyzowania.ToString().Replace(',','.'));
            winapi.managedSetText(dict["p_m"], zad.p_mutacji.ToString().Replace(',', '.'));
            winapi.SendMessage(dict["skalowanie"], winapi.BM_SETCHECK, (zad.skalowanie ? (IntPtr)1 : (IntPtr)0), IntPtr.Zero);
            winapi.SendMessage(dict["elitaryzm"], winapi.BM_SETCHECK, (zad.elitaryzm ? (IntPtr)1 : (IntPtr)0), IntPtr.Zero);
            winapi.managedSetText(dict["rand"], zad.rand.ToString());

            Thread.Sleep(sleepTime);

            winapi.SendMessage(dict["ok"], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);

            Thread.Sleep(sleepTime);
        }

        public void run()
        {
            winapi.SendMessage(mainDict["pokaz"], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            winapi.SendMessage(mainDict["go"], winapi.BM_CLICK, IntPtr.Zero, IntPtr.Zero);

            while (true)
            {
                Thread.Sleep(50);

                string label = winapi.managedGetText(mainDict["label"]);
                if (label.Contains("Zakończono"))
                    break;
            }

            Thread.Sleep(sleepTime);
        }

        public double readResult()
        {
            if (!readingEnabled)
                throw new Exception("Reading is not enabled.");

            byte[] buffer = new byte[8];
            int bytesRead;
            winapi.ReadProcessMemory(hProcess, (IntPtr)resultAddr, buffer, 8, out bytesRead);

            return BitConverter.ToDouble(buffer, 0);
        }
    }
}
