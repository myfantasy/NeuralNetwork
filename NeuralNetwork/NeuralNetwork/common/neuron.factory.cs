using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nnet.common
{
    public partial class Neuron
    {
        public static Dictionary<string, Tuple<Func<double, double>, Func<double, double>>> known_funcs = new Dictionary<string, Tuple<Func<double, double>, Func<double, double>>>()
        {   {"line", new Tuple<Func<double, double>, Func<double, double>>(Line_func, Line_func_derivative) },
            {"th", new Tuple<Func<double, double>, Func<double, double>>(Th_func, Th_func_derivative) }
        };


        public static Neuron Create(string a_name, List<long> channels)
        {
            var n = new Neuron();
            n.a_name = a_name;
            n.a = known_funcs[a_name].Item1;
            n.da = known_funcs[a_name].Item2;
            n.w = channels.ToDictionary(f => f, f => 0d);
            return n;
        }
        public static Neuron Create(string a_name, Dictionary<long, double> w)
        {
            var n = new Neuron();
            n.a_name = a_name;
            n.a = known_funcs[a_name].Item1;
            n.da = known_funcs[a_name].Item2;
            n.w = w;
            return n;
        }


        public DSO ToDSO()
        {
            var r = new DSO();
            r.Add("name", this.a_name);
            r.Add("w", this.w.ToDictionary(f => f.Key.ToString(), f => (object)f.Value));
            return r;
        }

        public static Neuron FromDSO(Dictionary<string, object> r)
        {
            string a_name = r.GetElement<string>("name");
            var n = new Neuron();
            n.a_name = a_name;
            n.a = known_funcs[a_name].Item1;
            n.da = known_funcs[a_name].Item2;

            n.w = r.GetElement<Dictionary<string, object>>("w").ToDictionary(f => Convert.ToInt64(f.Key), f => Convert.ToDouble(f.Value));
            return n;
        }


        public static double Line_func(double x)
        {
            return x;
        }
        public static double Line_func_derivative(double x)
        {
            return 1;
        }
        
        public static double Sh_func(double x)
        {
            return ((Math.Exp(x) - Math.Exp(-x)) / 2);
        }
        public static double Ch_func(double x)
        {
            return ((Math.Exp(x) + Math.Exp(-x)) / 2);
        }

        public static double Th_func(double x)
        {
            return (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x)); // = sh_func(x) / ch_func(x);
        }
        public static double Th_func_derivative(double x)
        {
            return 1 / Math.Pow(Ch_func(x), 2);
        }
    }
}
