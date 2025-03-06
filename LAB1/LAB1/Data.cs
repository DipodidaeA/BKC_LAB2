using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Data
{
    //F: X = sort(d * B + Z * (MM * MX)) * min(B)
    public class ProgData
    {
        public int[] X;

        public int d;         // СР
        public int[] B;
        public int[] Z;
        public int[,] MM;
        public int[,] MX;

        public int[] Q1hT2;
        public int[] Q1hT4;
        public int[] Q2hT3;
        public int[] Q;

        public int a;   // СР

        public int H;

        public Semaphore semaphore_P1 = new Semaphore(0, 3); // семафор, перший потік чекає
        public Semaphore semaphore_P2 = new Semaphore(0, 3); // семафор, другий потік чекає
        public Semaphore semaphore_P3 = new Semaphore(0, 3); // семафор, третій потік чекає
        public Semaphore semaphore_P4 = new Semaphore(0, 3); // семафор, четвертий потік чекає

        public ProgData(int N)
        {
            d = new int();
            B = new int[N];
            Z = new int[N];
            MM = new int[N,N];
            MX = new int[N, N];
            H = N / 4;
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

        // сортує отриманий вектор
        public int[] SortVect(int[] A)
        {
            return A.OrderBy(x => x).ToArray();
        }

        public int[] SortVectConcat(int[] A, int[] B)
        {
            var res = A.Concat(B).ToArray();

            return res.OrderBy(x => x).ToArray();
        }

        public int[] VectConcat(int[] A, int[] B)
        {
            var res = A.Concat(B).ToArray();

            return res;
        }

        public int MinNum(int a, int ai)
        {
            return Math.Min(a, ai);
        }

        public int[] GetPartVect(int t, int H, int[] V)
        {
            int[] res = new int[H];

            for (int i = 0; i < H; i++)
            {
                res[i] = V[i+H*t];
            }

            return res;
        }

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
    }

    // клас для заповенння векторів та матриць для функцій
    public class FillParam
    {

        // заповнення для числа
        public void FillNum(string nameN, int num)
        {
            Console.Write($"{nameN}: ");

            num = int.Parse(Console.ReadLine());

            Console.WriteLine();
        }

        // заповнення для вектора
        public void FillVect(string nameV, int[] V)
        {
            int N = V.Length;
            Console.WriteLine($"{nameV}:");

            for (int i = 0; i < N; i++)
            {
                Console.Write($"{i}: ");
                V[i] = int.Parse(Console.ReadLine());
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        // заповнення для матриці
        public void FillMatr(string nameMA, int[,] MA)
        {
            int N = MA.GetLength(0);
            Console.WriteLine($"{nameMA}:");

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write($"{i},{j}: ");
                    MA[i, j] = int.Parse(Console.ReadLine());
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        // заповнення для числа
        public void FillNumAuto(int num)
        {
            num = 1;
        }

        // випадкове заповнення для вектора
        public void FillVectAuto(int[] V)
        {
            int N = V.Length;

            for (int i = 0; i < N; i++)
            {
                V[i] = 1;
            }
        }

        // випадкове заповнення для матриці
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

    // клас для виводу векторів та матриць
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