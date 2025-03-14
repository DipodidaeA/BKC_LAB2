using Data;
using System;
using System.Diagnostics;
using System.Threading;

namespace Task
{
    static class T2
    {
        public static void Func2(int N, ProgData pD)
        {
            FillParam fP = new FillParam();
            PrintText pT = new PrintText();
            MathFucn mF = new MathFucn();

            // таймер часу
            Stopwatch stopwatch = new Stopwatch();
            // запуст таймера часу
            stopwatch.Start();

            // 1) введення даних B, MX
            Console.WriteLine("T2. Input B, MX");
            fP.FillVectAuto(pD.B);
            fP.FillMatrAuto(pD.MX);

            // 2) СИГНАЛИ: Т1  Т3  Т4 введено B, MX 	– S(1, 2), S(3, 2), S(4, 2)
            pD.sem1.Release(3);

            // 3) ЧЕКАТИ: введення даних d, Z, MM в Т4  		– W(2, 4)
            pD.sem2.WaitOne();

            // 4) обчислення 1. a2 = min(Bh)
            int a2 = mF.MinNumInVect(mF.GetPartVect(1, pD.H, pD.B));

            // 5) обчислення 2. a = min(a, a2)
            pD.skd1.WaitOne();
                pD.a = mF.MinNum(pD.a, a2); // – КД 1
            pD.skd1.Set();

            // 6) СИГНАЛИ: Т1,T3,T4 про обчислення  а	– S(1, 2), S(3, 2), S(4, 2)
            pD.sem4.Release(3);

            // 7) ЧЕКАТИ: обчислення a від Т1,Т3,Т4  	– W(2, 1), W(2, 3), W(2, 4)
            pD.sem3.WaitOne();
            pD.sem5.WaitOne();
            pD.sem6.WaitOne();

            // 8) копіюємо d у d2
            pD.skd2.WaitOne();
                int d2 = pD.d; // – КД 2
            pD.skd2.Set();

            // 9) обчислення 3. Rh = (d2 * Bh + Z * (MMн * MXh))
            int[] Rh = mF.AddVector(mF.MulScalVector(d2, mF.GetPartVect(1, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(1, pD.H, pD.MX))));

            // 10) обчислення 4. Qh = sort(Rh)
            pD.Q1hT2 = mF.SortVect(Rh);

            // 11) СИГНАЛ: Т1 про обчислений Qh 		– S(1, 2)
            pD.sem7.Release();

            // 12) ЧЕКАТИ: обчислення Q від Т1 		– W(2, 1)
            pD.sem10.WaitOne();

            // 13) копіюємо a у а2
            pD.skd3.WaitOne();
                a2 = pD.a;          // – КД 3
            pD.skd3.Set();

            // 14) обчислення 7. Xh = Qh * a2
            mF.ReturnPartVect(1, pD.H, pD.X, mF.MulScalVector(a2, mF.GetPartVect(1, pD.H, pD.Q)));

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"T2 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");

            // 15) СИГНАЛ: Т4 про закінчення обчислення		– S(4, 2)
            pD.sem11.Release();
        }
    }
}