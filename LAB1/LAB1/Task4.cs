using Data;
using System;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;

namespace Task
{
    static class T4
    {
        public static void Func4(int N, ProgData pD)
        {
            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуст таймера часу
            stopwatch.Start();

            // 1) ЧЕКАТИ: введення даних B, MX в Т2		– W(4, 2)
            pD.sem1.WaitOne();

            // 2) введення даних d, Z, MM	
            Console.WriteLine("T4. Input d, Z, MM");
            pD.d = 1;
            fP.FillVectAuto(pD.Z);
            fP.FillMatrAuto(pD.MM);

            // 3) СИГНАЛИ: Т1,Т2,Т3 введено d, Z, MM	– S(1, 4), S(2, 4), S(3, 4)
            pD.sem2.Release(3);

            // 4) обчислення 1. a4 = min(Bh)
            int a4 = mF.MinNumInVect(mF.GetPartVect(3, pD.H, pD.B));

            // 5) обчислення 2. a = min(a, a4) 
            pD.skd1.WaitOne();
                pD.a = mF.MinNum(pD.a, a4);     // – КД 1
            pD.skd1.Set();

            // 6) СИГНАЛИ: Т1,T2,T3 про обчислення  а	– S(1, 4), S(2, 4), S(3, 4)
            pD.sem6.Release(3);

            // 7) ЧЕКАТИ: обчислення a від Т1,Т2,Т3  	– W(4, 1), W(4, 2), W(4, 3)
            pD.sem3.WaitOne();
            pD.sem4.WaitOne();
            pD.sem5.WaitOne();

            // 8) копіюємо d у d4
            pD.skd2.WaitOne();
                int d4 = pD.d;      // – КД 2
            pD.skd2.Set();

            // 9) обчислення 3. Rh = (d4 * Bh + Z * (MMн * MXh))
            int[] Rh = mF.AddVector(mF.MulScalVector(d4, mF.GetPartVect(3, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(3, pD.H, pD.MX))));

            // 10) обчислення 4. Qh = sort(Rh)
            pD.Q1hT4 = mF.SortVect(Rh);

            // 11) СИГНАЛ: Qh обчислений для Т3 		– S(3, 4)
            pD.sem8.Release();

            // 12) ЧЕКАТИ: обчислення Q від Т1 		– W(4, 1)
            pD.sem10.WaitOne();

            // 13) копіюємо a у а4
            pD.skd3.WaitOne();
                a4 = pD.a;          // 	– КД 3
            pD.skd3.Set();

            // 14) обчислити Xh = Qh * a4
            mF.ReturnPartVect(3, pD.H, pD.X, mF.MulScalVector(a4, mF.GetPartVect(3, pD.H, pD.Q)));

            // 15) ЧЕКАТИ: Т1-Т3 закінчив обчислення  	– W(4, 1), W(4, 2), W(4, 3)
            pD.sem11.WaitOne();


            // зупинка таймеру часу
            stopwatch.Stop();

            // якщо N менше рівне 4, виводимо повний результат обчислення
            // 16) вивести X
            if (N <= 4)
            {
                Console.WriteLine($"T4 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine("Res: ");
                pT.PrintVect("X", pD.X);
            }
            else
            {
                Console.WriteLine($"T4 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine("Res: ");
                Console.WriteLine("X[0]: " + pD.X[0]);
            }
        }
    }
}