using nnet.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace test_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var net = Neuralnet.CreateSimple(new System.Collections.Generic.List<int>() { 2,5,10, 5,2 });

            var d = net.ToDSO().TryGetJson();

            var n2 = Neuralnet.FromDSO(d.TryGetFromJson());

            var x = n2.Calc(new Dictionary<long, double>() { { 0, -0.8 }, { 1, -0.8 } });

            var n3 = Learning.RandomLearn(n2, 0, 5, 
                new List<Learning.IOBlock>()
                {
                    Learning.IOBlock.Create(2,1,-0.8, -0.8, 0.8, -0.8),
                    Learning.IOBlock.Create(2,1,0.8, 0.8, 0.8, -0.8),
                    Learning.IOBlock.Create(2,1,0.8, -0.8, -0.8, -0.8),
                    Learning.IOBlock.Create(2,1,-0.8, 0.8, -0.8, 0.8),
                }
                );

            var n4 = Learning.RandomLearn(n3, 1000, 0.5, 0.3,
                new List<Learning.IOBlock>()
                {
                    Learning.IOBlock.Create(2,2, -1, -1,  0.8,  0),
                    Learning.IOBlock.Create(2,2,  1,  1,  0.8, -0.8),
                    Learning.IOBlock.Create(2,2,  1, -1, -0.8, -0.8),
                    Learning.IOBlock.Create(2,2, -1,  1, -0.8,  0.8),
                }
                );


            NeuralnetField nf = new NeuralnetField();
            nf.step_count = new List<int>() { 2, 3, 4 };

            var v = nf.Calc(null);

            Console.WriteLine("0, 0, " + n3.Calc(new Dictionary<long, double>() { { 0, -0.8 }, { 1, -0.8 } })[0]);
            Console.WriteLine("0, 1, " + n3.Calc(new Dictionary<long, double>() { { 0, -0.8 }, { 1, 0.8 } })[0]);
            Console.WriteLine("1, 0, " + n3.Calc(new Dictionary<long, double>() { { 0, 0.8 }, { 1, -0.8 } })[0]);
            Console.WriteLine("1, 1, " + n3.Calc(new Dictionary<long, double>() { { 0, 0.8 }, { 1, 0.8 } })[0]);

            Console.WriteLine();

            Console.WriteLine("0, 0, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, -1 }, { 1, -1 } })));
            Console.WriteLine("0, 1, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, -1 }, { 1, 1 } })));
            Console.WriteLine("1, 0, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, 1 }, { 1, -1 } })));
            Console.WriteLine("1, 1, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, 1 }, { 1, 1 } })));
            
            Console.ReadKey();
        }

        public static string DictDisplay(Dictionary<long, double> d)
        {
            StringBuilder sb = new StringBuilder("");
            bool b = false;
            foreach (var v in d)
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
            return sb.ToString();
        }
    }
}
