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

            // 2) СИГНАЛИ: B, MX введено	– S(1, 2), S(3, 2), S(4, 2)
            pD.sem1.Release(3);

            // 3) ЧЕКАТИ: введення даних d, Z, MM    – W(2, 4)
            pD.sem2.WaitOne();

            // 4) обчислення 1. aі = min(Bh)
            int ai = mF.MinNumInVect(mF.GetPartVect(1, pD.H, pD.B));

            // якщо а хтось вже використовує, чекаємо
            pD.skd1.WaitOne();

            // 5) обчислення 2.  a = min(a, a1)   – КД 1
            pD.a = mF.MinNum(pD.a, ai);

            // збільшуємо кількість потоків що обчилили а
            pD.c++;

            // 6) СИГНАЛИ: Т1,T3,T4 про обчислення  а	– S(1, 2), S(3, 2), S(4, 2)
            pD.skd1.Release();

            // перевіряємо чи всі потоки обчислили а
            pD.isCheckCalc(4, pD.sem3);

            // 7) ЧЕКАТИ: обчислення a від Т1,Т3,Т4  	– W(2, 1), W(2, 3), W(2, 4)
            pD.sem3.WaitOne();

            // якщо d хтось вже використовує, чекаємо
            pD.skd2.WaitOne();

            // 8) копіювати d1 = d	 – КД 2
            int d1 = pD.d;

            // дозволяємо використовувати d
            pD.skd2.Release();

            // 9) обчислення 3. Rh = (d1 * Bh + Z * (MMн * MXh))
            int[] Rh = mF.AddVector(mF.MulScalVector(d1, mF.GetPartVect(1, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(1, pD.H, pD.MX))));

            // 10) обчислення 4. Q1h = sort(Rh)
            pD.Q1hT2 = mF.SortVect(Rh);

            // 11) СИГНАЛ: Q1h обчислений для Т1 	– S(1, 2)
            pD.sem4.Release();

            // 12) ЧЕКАТИ: обчислення Q від Т1 		– W(2, 1)
            pD.sem6.WaitOne();

            // якщо a хтось вже використовує, чекаємо
            pD.skd3.WaitOne();

            // 13) копіювати а1 = a		– КД 3
            ai = pD.a;

            // дозволяємо використовувати a
            pD.skd3.Release();

            // 14) обчислення 7. Xh = Qh * a1
            mF.ReturnPartVect(1, pD.H, pD.X, mF.MulScalVector(ai, mF.GetPartVect(1, pD.H, pD.Q)));

            // зупинка таймеру часу
            stopwatch.Stop();

            Console.WriteLine($"T2 end; H: {pD.H}; Time: {stopwatch.ElapsedMilliseconds} ms");

            // 15) СИГНАЛ: Т4 про Т2 закінчив обчислення	– S(4, 2)
            pD.c++;
            pD.isCheckCalc(3, pD.sem7);
        }
    }
}