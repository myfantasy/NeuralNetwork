using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nnet.common
{
    public class Field
    {
        public Field()
        { }

        public Field(int[] ds)
        {
            for (int i = 0; i < ds.Length; i++)
            {
                size.Add(ds[i]);
            }
        }

        public Field(int d1, params int[] ds)
        {
            size.Add(d1);
            for (int i = 0; i < ds.Length; i++)
            {
                size.Add(ds[i]);
            }
        }

        List<int> size = new List<int>();

        Dictionary<long, double> values = new Dictionary<long, double>();

        public long GetIndex(params int[] pos)
        {
            long res = 0;

            long mult = 1;

            for (int i = 0; i < pos.Length; i++)
            {
                if (pos[i] >= size[i])
                {
                    res += (size[i] - 1) * mult;
                }
                else if (pos[i] > 0)
                {
                    res += pos[i] * mult;
                }
                mult = mult * size[i];
            }

            return res;
        }

        public double this[params int[] pos]
        {
            get
            {
                long index = GetIndex(pos);
                return values[index];
            }
            set
            {
                long index = GetIndex(pos);
                values.AddOrUpdate(index, value);
            }
        }


    }
}
