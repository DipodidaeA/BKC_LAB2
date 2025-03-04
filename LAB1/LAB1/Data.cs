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
        public int d;
        public int[] B;
        public int[] Z;
        public int[] X; // Q * m
        public int[,] MM;
        public int[,] MX;

        public int[,] MK; // MM * MX
        public int[] K; // d * B
        public int m; // min(B)
        public int[] Y; // Z * MK
        public int[] Q; // K + Y and sort(Q)

        public Semaphore semaphore_P1; // семафор першого потоку
        public Semaphore semaphore_P2; // семафор другого потоку

        public ProgData(int N)
        {
            B = Z = X = K = Y = Q = new int[N];
            MM = MX = MK = new int[N,N];

            semaphore_P1 = new Semaphore(0, 1);  // перший потік чекає
            semaphore_P2 = new Semaphore(1, 1); // другий потік починає
        }
    }

    // клас математичних функцій
    public class MathFucn
    {
        // множення скаляра на вектор, повертає вектор
        public int[] MulScalVector(int A, int[] B)
        {
            int N = B.Length;
            int[] R = new int[N];

            for (int i = 0; i < N; i++)
            {
                R[i] = A * B[i];
            }

            return R;
        }

        // додає два вектори, повертає вектор
        public int[] AddVector(int[] A, int[] B)
        {
            int N = A.Length;
            int[] R = new int[N];

            for (int i = 0; i < N; i++)
            {
                R[i] = A[i] + B[i];
            }

            return R;
        }

        // Множення вектора на матриць, повертає вектор
        public int[] MulVecMatr(int[] A, int[,] B)
        {
            int N = A.Length;
            int[] R = new int[N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    R[i] += A[j] * B[j, i];
                }
            }

            return R;
        }

        // множення матриць, повертає матрицю
        public int[,] MulMatr(int[,] A, int[,] B)
        {
            int N = A.GetLength(0);
            int[,] R = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        R[i, j] += A[i, k] * B[k, j];
                    }
                }
            }

            return R;
        }

        // пошук мінімального числа у векторі, повертає число
        public int MinNumInVect(int[] A)
        {
            return A.Min();
        }

        // сортує отриманий вектор
        public void SortVect(int[] A)
        {
            Array.Sort(A);
        }
    }

    // клас для заповенння векторів та матриць для функцій
    public class FillParam
    {
        Random random = new Random();

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
        public void FillNumRand(int num)
        {
            num = random.Next(1, 10);
        }

        // випадкове заповнення для вектора
        public void FillVectRand(int[] V)
        {
            int N = V.Length;

            for (int i = 0; i < N; i++)
            {
                V[i] = random.Next(1, 10);
            }
        }

        // випадкове заповнення для матриці
        public void FillMatrRand(int[,] MA)
        {
            int N = MA.GetLength(0);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    MA[i, j] = random.Next(1, 10);
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