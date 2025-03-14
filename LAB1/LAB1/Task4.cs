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

            // 1) ЧЕКАТИ: введення даних B, MX		– W(4, 2)
            pD.sem1.WaitOne();

            // 2) введення даних d, Z, MM	
            Console.WriteLine("T4. Input d, Z, MM");
            pD.d = 1;
            fP.FillVectAuto(pD.Z);
            fP.FillMatrAuto(pD.MM);


            // 3) СИГНАЛИ: d, Z, MM введено		– S(1, 4), S(2, 4), S(3, 4)
            pD.sem2.Release(3);

            // 4) обчислення 1. aі = min(Bh)
            int ai = mF.MinNumInVect(mF.GetPartVect(3, pD.H, pD.B));

            // якщо а хтось вже використовує, чекаємо
            pD.skd1.WaitOne();

            // 5) обчислення 2. a = min(a, aі)  	– КД 1
            pD.a = mF.MinNum(pD.a, ai);

            // збільшуємо кількість потоків що обчилили а
            pD.c++;

            // 6) СИГНАЛИ: Т1,T2,T3 про обчислення  а	– S(1, 4), S(2, 4), S(3, 4)
            pD.skd1.Release();

            // перевіряємо чи всі потоки обчислили а
            pD.isCheckCalc(4, pD.sem3);

            // 7) ЧЕКАТИ: обчислення a від Т1, Т2, Т3  	– W(4, 1), W(4, 2), W(4, 3)
            pD.sem3.WaitOne();

            // якщо d хтось вже використовує, чекаємо
            pD.skd2.WaitOne();

            // 8) копіювати d1 = d		– КД 2
            int d1 = pD.d;

            // дозволяємо використовувати d
            pD.skd2.Release();

            // 9) обчислення 3. Rh = (d1 * Bh + Z * (MMн * MXh))
            int[] Rh = mF.AddVector(mF.MulScalVector(d1, mF.GetPartVect(3, pD.H, pD.B)), mF.MulVecMatr(pD.Z, mF.MulMatr(pD.MM, mF.GetPartMatrTab(3, pD.H, pD.MX))));

            // 10) обчислення 4.Q1h = sort(Rh)
            pD.Q1hT4 = mF.SortVect(Rh);

            // 11) СИГНАЛ: Q1h обчислений для Т3 		– S(3, 4)
            pD.sem8.Release();

            // 12) ЧЕКАТИ: обчислення Q від Т1 		– W(4, 1)
            pD.sem6.WaitOne();

            // якщо a хтось вже використовує, чекаємо
            pD.skd3.WaitOne();

            // 13) копіювати а1 = a		– КД 3
            ai = pD.a;

            // дозволяємо використовувати a
            pD.skd3.Release();

            // 14) обчислити Xh = Qh * a1
            mF.ReturnPartVect(3, pD.H, pD.X, mF.MulScalVector(ai, mF.GetPartVect(3, pD.H, pD.Q)));

            // 15) ЧЕКАТИ: Т1-Т3 закінчив обчислення  	– W(4, 1), W(4, 2), W(4, 3)
            pD.sem7.WaitOne();


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