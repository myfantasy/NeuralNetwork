using System;
using System.Collections.Generic;
using System.Text;

namespace nnet.common
{
    public partial class Learning
    {
        public class IOBlock
        {
            public Dictionary<long, double> input = new Dictionary<long, double>();
            public Dictionary<long, double> output = new Dictionary<long, double>();

            public static IOBlock Create(int ic, int oc, params double[] p)
            {
                IOBlock b = new IOBlock();

                for (int i = 0; i < ic; i++)
                {
                    b.input.Add(i, p[i]);
                }

                for (int i = 0; i < oc; i++)
                {
                    b.output.Add(i, p[i + ic]);
                }

                return b;
            }
        }

        public static double d_sq(Dictionary<long, double> r1, Dictionary<long, double> r2)
        {
            double res = 0;
            foreach (var v in r1)
            {
                res += Math.Pow(v.Value - r2[v.Key], 2);
            }

            return Math.Pow(res / r2.Count, 0.5);
        }

        public static double d_sq(Neuralnet net, List<IOBlock> check_list)
        {
            double res = 0;

            foreach (var v in check_list)
            {
                res += Math.Pow(d_sq(v.output, net.Calc(v.input)), 2);
            }

            return Math.Pow(res / check_list.Count, 0.5);
        }
    }
}
