using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFantasy.NeuralNetwork.Common
{
    public class Field
    {
        #region Common
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

        public List<int> size = new List<int>();

        public Dictionary<long, double> values = new Dictionary<long, double>();

        public long GetIndex(params int[] pos)
        {
            long res = 0;

            long mult = 1;

            for (int i = 0; i < size.Count; i++)
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

        public double GetValueOrDefault(int[] pos)
        {
            long index = GetIndex(pos);
            return values.GetValueOrDefault(index);
        }

        public Field AddOrUpdate(double value, params int[] pos)
        {
            this[pos] = value;

            return this;
        }

        public void SetValue(double value = 0)
        {
            long res = 0;

            long mult = 1;

            for (int i = 0; i < size.Count; i++)
            {
                    res += (size[i] - 1) * mult;
                mult = mult * size[i];
            }

            for (long i = 0; i < res + 1; i++)
            {
                values.AddOrUpdate(i, value);
            }
        }
        #endregion     
        
        public static Field do_by_element(Field f1, Field f2, Func<double, double, double> func)
        {
            Field res_arr = new Field(f1.size.ToArray());
            long res = 0;

            long mult = 1;

            for (int i = 0; i < f1.size.Count; i++)
            {
                res += (f1.size[i] - 1) * mult;
                mult = mult * f1.size[i];
            }

            for (long i = 0; i < res + 1; i++)
            {
                res_arr.values.Add(i, func(f1.values.GetValueOrDefault(i), f2.values.GetValueOrDefault(i)));
            }

            return res_arr;
        }
    }
}
