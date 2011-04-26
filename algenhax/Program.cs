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

        public static IEnumerable<double> enumerujDoubla2()
        {
            for (int i = 1; i <= 9; i++)
            {
                yield return i * 0.01 + 0.0014;
            }
            for (int i = 1; i <= 9; i++)
            {
                yield return i * 0.1 + 0.014;
            }
        }

        static void Main(string[] args)
        {
            EstymMain();
            //ProjStruMain();
        }

        static void ProjStruPrint(int lp, ProjStru.ParametryZadania p, double wynik)
        {
            Console.Write(lp + ";" +
                p.l_osobnikow + ";" + p.l_pokolen + ";" +
                (p.skalowanie ? 1 : 0) + ";" + (p.elitaryzm ? 1 : 0) + ";" +
                p.p_krzyzowania + ";" + p.p_mutacji + ";" +
                wynik);
            Console.WriteLine(); 
        }

        static void ProjStruMain()
        {
            ProjStru proj = new ProjStru();
            proj.init();

            Console.WriteLine("L.p.;Liczba osobników;Liczba pokoleń;Skalowanie;Elitaryzm;P krzyżowania; P mutacji;Najniższy koszt osobnika");

            int lp = 0;

            double optymalne = 0;
            int optymalny_int = 0;
            double opt_wynik = Double.PositiveInfinity;

            ProjStru.ParametryZadania p = new ProjStru.ParametryZadania();
            for (byte i = 0; i < 4; i++)
            {
                p.elitaryzm = (i & 0x01) == 1;
                p.skalowanie = ((i >> 1) & 0x01) == 1;
                p.l_osobnikow = 100;
                p.l_pokolen = 100;
                p.p_mutacji = 0.2;

                optymalne = 0;
                optymalny_int = 0;
                opt_wynik = Double.PositiveInfinity;

                foreach (double d in enumerujDoubla())
                {
                    p.p_krzyzowania = d;
                    proj.setParametry(p);
                    proj.run();
                    double result = proj.readResult();
                    if (result < opt_wynik)
                    {
                        opt_wynik = result;
                        optymalne = d;
                    }
                    ProjStruPrint(lp++, p, result);
                    System.Threading.Thread.Sleep(50);
                }

                p.p_krzyzowania = optymalne;
                opt_wynik = Double.PositiveInfinity;

                foreach (double d in enumerujDoubla())
                {
                    p.p_mutacji = d / 2;
                    proj.setParametry(p);
                    proj.run();
                    double result = proj.readResult();
                    if (result < opt_wynik)
                    {
                        opt_wynik = result;
                        optymalne = d / 2;
                    }
                    ProjStruPrint(lp++, p, result);
                    System.Threading.Thread.Sleep(50);
                }

                p.p_mutacji = optymalne;
                opt_wynik = Double.PositiveInfinity;

                foreach (int d in new int[] { 25, 50, 100, 150, 200 })
                {
                    p.l_osobnikow = d;
                    proj.setParametry(p);
                    proj.run();
                    double result = proj.readResult();
                    if (result < opt_wynik)
                    {
                        opt_wynik = result;
                        optymalny_int = d;
                    }
                    ProjStruPrint(lp++, p, result);
                    System.Threading.Thread.Sleep(50);
                }

                p.l_osobnikow = optymalny_int;
                opt_wynik = Double.PositiveInfinity;

                foreach (int d in new int[] { 50, 100, 150, 200, 250 })
                {
                    p.l_pokolen = d;
                    proj.setParametry(p);
                    proj.run();
                    double result = proj.readResult();
                    if (result < opt_wynik)
                    {
                        opt_wynik = result;
                        optymalny_int = d;
                    }
                    ProjStruPrint(lp++, p, result);
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        static void EstymPrint(int lp, Estym.ParametryGenetyczne p, double[] wyniki)
        {
            Console.Write(lp + ";" +
                p.l_osobnikow + ";" + p.l_pokolen + ";" +
                (p.skalowanie ? 1 : 0) + ";" + (p.elitaryzm ? 1 : 0) + ";" +
                p.p_krzyzowania + ";" + p.p_mutacji + ";");

            foreach (int i in new int[] { 1, 6, 7, 8, 9 })
            {
                Console.Write(wyniki[i] + ";");
            }

            Console.Write(wyniki[0]);
            Console.WriteLine();    
        }

        static void EstymMain()
        {
            Estym estym = new Estym();
            estym.init();

            double optymalne = 0;
            double opt_wynik = Double.PositiveInfinity;

            Estym.ParametryGenetyczne p = new Estym.ParametryGenetyczne();

            int lp = 1;

            Console.WriteLine("lp;liczba osobników;liczba pokoleń;skalowanie;elitaryzm;p krzyżowania;p mutacji;a0;a5;a6;a7;a8;wynik");

            for (byte i = 0; i < 4; i++)
            {
                p.elitaryzm = (i & 0x01) == 1;
                p.skalowanie = ((i >> 1) & 0x01) == 1;
                p.l_osobnikow = 50;
                p.l_pokolen = 1000;
                p.p_mutacji = 0.05;
                p.p_krzyzowania = 0.2;

                optymalne = 0;
                opt_wynik = Double.PositiveInfinity;

                foreach (double d in enumerujDoubla2())
                {
                    p.p_mutacji = d;
                    estym.setParametryGenetyczne(p);
                    estym.wykonajObliczenia();
                    double[] wyniki = estym.sprawdz();
                    if (wyniki[0] < opt_wynik)
                    {
                        optymalne = d;
                        opt_wynik = wyniki[0];
                    }

                    EstymPrint(lp++, p, wyniki);
                    System.Threading.Thread.Sleep(50);
                }

                p.p_mutacji = optymalne;

                optymalne = 0;
                opt_wynik = Double.PositiveInfinity;

                foreach (double d in enumerujDoubla2())
                {
                    p.p_krzyzowania = d;
                    estym.setParametryGenetyczne(p);
                    estym.wykonajObliczenia();
                    double[] wyniki = estym.sprawdz();
                    if (wyniki[0] < opt_wynik)
                    {
                        optymalne = d;
                        opt_wynik = wyniki[0];
                    }

                    EstymPrint(lp++, p, wyniki);
                    System.Threading.Thread.Sleep(50);
                }

                p.p_krzyzowania = optymalne;

                foreach(int d in new int[]{5, 10, 20})
                {
                    p.l_osobnikow = d;
                    estym.setParametryGenetyczne(p);
                    estym.wykonajObliczenia();
                    double[] wyniki = estym.sprawdz();
                    EstymPrint(lp++, p, wyniki);
                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        static void AlgenMain()
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
    }
}
