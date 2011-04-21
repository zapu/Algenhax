using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace algenhax
{
    class Program
    {
        public static IEnumerable<double> enumerujDoubla()
        {
            for (int i = 1; i <= 9; i++)
            {
                yield return i * 0.01;
            }
            for (int i = 1; i <= 9; i++)
            {
                yield return i * 0.1;
            }
        }

        static void Main(string[] args)
        {
            //winapitools.enumWindows();

            Estym estym = new Estym();
            estym.init();

            /*Estym.ParametryGenetyczne pg = new Estym.ParametryGenetyczne();
            pg.elitaryzm = true;
            pg.skalowanie = false;
            pg.l_osobnikow = 15;
            pg.l_pokolen = 1337;
            pg.p_krzyzowania = 0.66;
            pg.p_mutacji = 0.01;

            estym.setParametryGenetyczne(pg);*/

            //estym.wykonajObliczenia();
            //Console.WriteLine("DONE");
            estym.sprawdz();

            /*IntPtr estymHwnd = winapi.FindWindow3(IntPtr.Zero, "ESTYM");
            IntPtr hmenu = winapi.GetMenu(estymHwnd);
            winapitools.enumMenus(hmenu);*/

            //winapi.GetMenu(
            //IntPtr hwnd = winapi.FindWindow("#32770", "Sprawdzanie wartości funkcji celu");
            //IntPtr hwnd = winapi.FindWindow("#32770", "Parametry genetyczne");
            //winapitools.enumChildWindows(hwnd);
        }

#if FALSE
        static void Main(string[] args)
        {
            try
            {
                Algen alg = new Algen();
                alg.init();

                Console.WriteLine("Co enumerujemy? 1 - krzyzowanie, 2 - mutacje");
                int en = 0;
                ConsoleKeyInfo a = Console.ReadKey();
                switch (a.KeyChar)
                {
                    case '1':
                        break;
                    case '2':
                        en = 1;
                        break;
                    default:
                        Console.WriteLine("huh?");
                        return;
                }



                foreach (double val in enumerujDoubla())
                {
                    if (en == 0)
                        alg.setKrzyzowania(Math.Round(val, 2).ToString().Replace(',', '.'));
                    else
                        alg.setMutacji(Math.Round(val, 2).ToString().Replace(',', '.'));

                    Thread.Sleep(50); // We probably don't need sleep here...

                    alg.sendStart();

                    Thread.Sleep(600);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
#endif
    }
}
