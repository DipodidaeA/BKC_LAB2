using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Data
{
    public class ProgData
    {
        // результат
        public int[] X;

        // дані що вводять
        public int d;
        public int[] B;
        public int[] Z;
        public int[,] MM;
        public int[,] MX;

        // проміжні дані
        public int[] Q1hT2;
        public int[] Q1hT4;
        public int[] Q2hT3;
        public int[] Q;
        public int a = 999;

        // розмір частини даних що дається для одного потоку
        public int H;

        // семафор для очикування вводу Т2
        public Semaphore sem1 = new Semaphore(0, 3);
        // семафор для очикування вводу Т4
        public Semaphore sem2 = new Semaphore(0, 3);
        // семафор для очикування обчислення а
        public Semaphore sem3 = new Semaphore(0, 4);
        // семафор для очикування обчислення Q1h від Т2 для Т1
        public Semaphore sem4 = new Semaphore(0, 1);
        // семафор для очикування обчислення Q2h від Т3 для Т1
        public Semaphore sem5 = new Semaphore(0, 1);
        // семафор для очикування обчислення Q від Т1
        public Semaphore sem6 = new Semaphore(0, 3);
        // семафор для очикування обчислення T1-T3
        public Semaphore sem7 = new Semaphore(0, 3);
        // семафор для очикування обчислення Q1h від Т4 для Т3
        public Semaphore sem8 = new Semaphore(0, 1);

        // семафор для захисту а
        public Semaphore skd1 = new Semaphore(1, 1);
        // семафор для захисту d
        public Semaphore skd2 = new Semaphore(1, 1);
        // семафор для захисту а
        public Semaphore skd3 = new Semaphore(1, 1);

        public ProgData(int N)
        {
            X = new int[N];
            d = new int();
            B = new int[N];
            Z = new int[N];
            MM = new int[N,N];
            MX = new int[N, N];
            H = N / 4;
        }

        // лічільник потоків що завершили обчислення
        public int c = 0;
        // для перевірки чи всі потоки завершили обчислуння
        public void isCheckCalc(int l, Semaphore sem)
        {
            if (c == l)
            {
                sem.Release(l);
                c = 0;
            }
        }
    }

    // клас математичних функцій
    public class MathFucn
    {
        // множення скаляра на вектор, повертає вектор
        public int[] MulScalVector(int A, int[] B)
        {
            int N = B.Length;
            int[] res = new int[N];

            for (int i = 0; i < N; i++)
            {
                res[i] = A * B[i];
            }

            return res;
        }

        // додає два вектори, повертає вектор
        public int[] AddVector(int[] A, int[] B)
        {
            int N = A.Length;
            int[] res = new int[N];

            for (int i = 0; i < N; i++)
            {
                res[i] = A[i] + B[i];
            }

            return res;
        }

        // Множення вектора на матриць, повертає вектор
        public int[] MulVecMatr(int[] A, int[,] B)
        {
            int N = A.Length;
            int H2 = B.GetLength(1);
            int[] res = new int[H2];

            for (int i = 0; i < H2; i++)
            {
                for (int k = 0; k < N; k++)
                {
                    res[i] += A[k] * B[k, i];
                }
            }

            return res;
        }

        // множення матриць, повертає матрицю
        public int[,] MulMatr(int[,] A, int[,] B)
        {
            int H1 = A.GetLength(0);
            int N = A.GetLength(1);
            int H2 = B.GetLength(1);
            int[,] res = new int[H1, H2];

            for (int i = 0; i < H1; i++)
            {
                for (int j = 0; j < H2; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        res[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return res;
        }

        // пошук мінімального числа у векторі, повертає число
        public int MinNumInVect(int[] A)
        {
            return A.Min();
        }

        // з двох чисел вибирає менше
        public int MinNum(int a, int ai)
        {
            return Math.Min(a, ai);
        }

        // сортує отриманий вектор
        public int[] SortVect(int[] A)
        {
            return A.OrderBy(x => x).ToArray();
        }

        // обєднує два вектори та сортує результат
        public int[] SortVectConcat(int[] A, int[] B)
        {
            var res = A.Concat(B).ToArray();

            return res.OrderBy(x => x).ToArray();
        }

        // отримує частину параметрів вектора для певного потоку
        public int[] GetPartVect(int t, int H, int[] V)
        {
            int[] res = new int[H];

            for (int i = 0; i < H; i++)
            {
                res[i] = V[i+H*t];
            }

            return res;
        }

        // отримує частину параметрів стовпців матриці для певного потоку
        public int[,] GetPartMatrTab(int t, int H, int[,] MA)
        {
            int N = MA.GetLength(0);
            int[,] res = new int[N, H];

            for (int i = 0; i < H; i++)
            {
                for (int k = 0; k < N; k++)
                {
                    res[k, i] = MA[k, i+H*t];
                }
            }

            return res;
        }

        // повертає частину параметрів вектора певного потоку
        public void ReturnPartVect(int t, int H, int[] X, int[] V)
        {
            for (int i = 0; i < H; i++)
            {
                X[i + H * t] = V[i];
            }
        }
    }

    // клас для заповенння векторів та матриць для функцій
    public class FillParam
    {
        // заповнення для вектора, автоматично
        public void FillVectAuto(int[] V)
        {
            int N = V.Length;

            for (int i = 0; i < N; i++)
            {
                V[i] = 1;
            }
        }

        // заповнення для матриці, автоматично
        public void FillMatrAuto(int[,] MA)
        {
            int N = MA.GetLength(0);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    MA[i, j] = 1;
                }
            }
        }
    }

    // клас для виводу векторів
    public class PrintText
    {
        // для виводу вектора
        public void PrintVect(string NameV, int[] V)
        {
            int N = V.Length;

            Console.Write(NameV + ": ");

            for (int i = 0; i < N; i++)
            {
                Console.Write(V[i] + " ");
            }
            Console.WriteLine();
        }
    }
}