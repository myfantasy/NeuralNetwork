using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    public static class Linq_add_nnet
    {
        public static int[] CloneArr(this int[] p)
        {
            if (p == null)
            {
                return null;
            }

            int[] res = new int[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                res[i] = p[i];
            }
            return res;
        }

        public static int[] Change(this int[] p, int index, int value)
        {
            p[index] = value;
            return p;
        }
        public static int[] AddVal(this int[] p, int index, int value)
        {
            p[index] = p[index] + value;
            return p;
        }
        public static int[] Mult(this int[] p, int index, int value)
        {
            p[index] = p[index] * value;
            return p;
        }
    }
}
