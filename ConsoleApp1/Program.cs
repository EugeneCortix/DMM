using System;
using System.Globalization; ///////////////t+t+t+t///////////////////
class DMM
{
    static void Main()
    {
       string[] num = Console.ReadLine().Split(' ');
        int N = int.Parse(num[0]);
        int M = int.Parse(num[1]);
        int[,] Matrix = new int[N+M, M];
        int[] b = new int[N +M];
        for(int i = 0; i < N; i++)
        {
            string[] str = Console.ReadLine().Split(' ');
            for(int j = 0; j < M; j++) 
            {
                Matrix[i,j] = int.Parse(str[j]);
            }
            b[i] = int.Parse(str[M]) * (-1);
        }
        for(int i = N; i < N + M; i++)
        {
            for (int j = 0; j < M; j++)
            {
                if( i - N == j )
                Matrix[i, j] = 1;
            }
        }
        int[,] q;
        if ( N == 1 )
        {
             q = Solveone(Matrix, b, M);
        }
        else
        {
             q = Solvesyst(Matrix, b, M, N);
        }
        if (q == null)
        {
            Console.Clear();
            Console.WriteLine("NO SOLUTIONS");
            Console.ReadLine();
        }
        else
        {
            int frees = tval(q, M);
            Console.Clear();
            Console.WriteLine(frees);
            for(int i = 0; i < M; i++)
                Console.WriteLine(Show(q[i,0], q[i,1], frees));
            Console.ReadLine();
        }
    }

    static int tval(int[,] mat, int M)
    {
        int res = 0;
        for(int i = 0; i < M; i++)
            if (mat[i,1] != 0)
                res = 1;
        return res;
    }
    static string Show(int x, int t, int f)
    {
        string res = "";
        /* if(x== 0)
         {
             if (t == 0) return "0";
             res += Convert.ToString(t); // + "t";
             return res;
         }
         else
         {
             res += Convert.ToString(x);
             if (t == 0) return res;
             if (t > 0) res += "+";
             res += Convert.ToString(t); // + "t";
             return res;
         }*/
        res += Convert.ToString(x);
        if(f != 0)
        res+= ' ' + Convert.ToString(t);
        return res;
    }
    static int[,] Solveone(int[,] B, int[] c, int M)
    {
        c[0] *= -1;
    int[,] res = new int[M, 2];
        int cont = 0;
        for (int j = 0; j < M; j++)
            if (B[0, j] != 0) cont++;
        int itt = 0; // stop itterations
        while (cont > 1)
        {
            // 1
            int mind = -1;
            for (int j = 0; j < M; j++)
            {
                if (B[0,j] !=0)
                {
                    mind = j;
                    break;
                }
            }
            
            for (int j = mind; j < M; j++)
            {
                if (Math.Abs(B[0, j]) < Math.Abs(B[0, mind]) && Math.Abs(B[0, j]) != 0)
                {
                    mind = j;
                }
            }
            // 2
            int jnd = -1;  
            for (int j = 0; j < M; j++)
            {
                if (B[0, j] != 0 && j != mind)
                {
                    jnd = j;
                    break;
                }
            }
            // 3
            int q = B[0, jnd] / B[0, mind];
            // 4
            for (int i = 0; i < M + 1; i++)
            {
                B[i, jnd] -= q * B[i, mind];
            }
            // repeat
            cont = 0;
            for (int j = 0; j < M; j++)
            {
                if (B[0, j] != 0)
                    cont++;
            }
            // stop
            itt++;
            if (itt == 100) return null;
        }
        // d
        int d = 0;
        for (int j = 0; j < M; j++)
        {
            if (B[0, j] != 0)
                d = B[0, j];
        }
        if (c[0] % d > 0)
            return null;
        for(int i = 1; i < M+1; i++)
        {
            res[i - 1,0] = B[i,0] *c[0]/d;
            res[i - 1, 1] = 0;
            for (int j = 1; j < M; j++)
                res[i - 1, 1] += B[i, j];
        }
        return res;
    }
    static int[,] Solvesyst(int[,] B, int[] c, int M, int N)
    {
        for(int i = 0; i < N; i++)
        {
            int cont = 0;
            for(int j = 0; j < M; j++)
                if (B[i,j] != 0) cont++;
            int itt = 0;
            while(cont > i+1)
            {
                // min
                int jnd = -1;
                for(int j = i; j < M; j++)
                    if (B[i,j] != 0)
                    {
                        jnd = j;
                        break;
                    }
                if (jnd == -1) return null; 
                for (int j = i; j < M; j++)
                    if (Math.Abs(B[i, j] ) < Math.Abs(B[i, jnd]) && B[i, j] != 0) jnd = j;
                // second min
                int jnd2 = -1;
                for (int j = i; j < M; j++)
                    if (B[i, j] != 0 && j != jnd)
                    {
                        jnd2 = j;
                        break;
                    }
                if (jnd2 == -1) break;
                for (int j = i; j < M; j++)
                    if (Math.Abs(B[i, j]) < Math.Abs(B[i, jnd2]) && j != jnd && B[i, j] != 0) jnd2 = j;
                int q = B[i, jnd2] / B[i, jnd];
                // subtract 
                for( int str = 0; str < M +2; str++)
                    B[str, jnd2] -= B[str, jnd]*q;

                if (c[i] !=0)
                c = substractc(B, c, i, N);

                // if continue
                cont = 0;
                for (int j = 0; j < M; j++)
                    if (B[i, j] != 0) cont++;
                // if no solutions
                itt++;
                if (itt == 100)
                    return null;
            }
            B = SortCols(B, i, M, c.Length);
            
        }
        
        // turnung c into 0
        c = killc(B, M, c, N);
        if (c == null) return null;
        // if not 0 in the last column
        for (int i = 0; i < N; i++)
            if (c[i] != 0)
                return null;

        // diagonal shaping
        B = diagonal(B, N, M);
        if (B == null) return B;
            
            // a clif
            int[,] answer = new int[M, 2];
        for(int i = 0; i < M; i++)
        {
            for(int j = N; j < M; j++)
                answer[i, 1] += B[i + N, j];

        }
        for(int i = 0; i < M; i++)
        {
            answer[i, 0] = c[i+N];
            
        }
        return answer;
    }

    static int[,] diagonal(int[,]mat, int N, int M)
    {
        for( int i = 0; i < N; i++)
            for(int j = i; j < M; j++)
                if(i < j && mat[i,j] !=0)
                {
                    mat = annihilate(mat, M, N, j, i);
                    if (mat == null) return mat;
                }
        return mat;
    }
   static int[,] annihilate(int[,] mat, int M, int N, int scol, int str)
    {
        while(mat[str, scol] != 0)
        {
            int jb = -1;
            for (int j = 0; j < M; j++)
                if (j != scol && mat[str, j] != 0 && Math.Abs(mat[str, j]) <= Math.Abs(mat[str, scol]))
                {
                    jb = j;
                    break;
                }
            if (jb == -1) return null;
            int q = mat[str, scol] / mat[str, jb];
            for(int i = 0; i < N+M; i++)
                mat[i, scol] -= mat[i, jb]*q;
        }
        return mat;
    }
    static int[] substractc(int[,] B, int[] c, int i, int N)
    {
        // substract from c
        int ind = -1;
        for (int j = 0; j < N; j++)
            if (B[i, j] != 0) ind = j;
        for (int j = 0; j < N; j++)
            if (Math.Abs(B[i, j]) > Math.Abs(B[i, ind]) && Math.Abs(B[i, j]) < Math.Abs(c[i]))
                ind = j;
        int qcol = c[i] / B[i, ind];
        // sustract columns
        for (int str = 0; str < c.Length; str++)
            c[str] -= qcol * B[str, ind];
        return c;
    }
    static int[] killc(int[,] B, int M, int[] c, int N)
    {

        // kill c
        for(int i = 0; i < N;i++)
            {
                int itt = 0;
                while (c[i] != 0)
                {
                c = substractc(B, c, i, M);

                // if can't reach 0
                itt++;
                if(itt  == 100) { return null; }
                }
            }
        return c;
    }

    static int[,] SortCols(int[,] mat, int i, int M, int strnum)
    {
        int jcurr = i;
        for(int j = i + 1; j < M; j++)
            if (mat[i, j] != 0)
            {
                jcurr = j;
                break;
            }
        if(jcurr == i) return mat;
        for(int st = 0; st < strnum; st++)
        {
            int t = mat[st, jcurr];
            mat[st, jcurr] = mat[st, i];
            mat[st, i] = t;
        }
        return mat;
    }

}
