using System;
using System.Collections.Generic;
using System.Text;

namespace nnet.common
{
    public partial class Neuron
    {
        /// <summary>
        /// input_signals_weight: id -> weight
        /// </summary>
        public Dictionary<long, double> w = new Dictionary<long, double>();

        /// <summary>
        /// the activation function
        /// </summary>
        public Func<double, double> a = Line_func;

        /// <summary>
        /// the derivative of the activation function (производная)
        /// </summary>
        public Func<double, double> da = Line_func_derivative;

        /// <summary>
        /// name of the activation function
        /// </summary>
        public string a_name = "line";
        
        /// <summary>
        /// calculates the value at the output of the neuron
        /// </summary>
        /// <param name="vals">id input signal -> value input signal</param>
        /// <returns></returns>
        public virtual double Calc(Dictionary<long, double> vals)
        {
            double res = 0;
            foreach (var v in w)
            {
                if (vals.ContainsKey(v.Key))
                {
                    res += vals[v.Key] * v.Value;
                }
            }
            return a(res);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("");
            bool b = false;
            foreach (var v in w)
            {
                if (b)
                {
                    sb.Append(", ");
                }
                sb.Append(v.Key);
                sb.Append(": ");
                sb.Append(v.Value.ToString("0.000"));
                b = true;
            }

            return a_name + ":" + sb.ToString();
        }
    }
}
