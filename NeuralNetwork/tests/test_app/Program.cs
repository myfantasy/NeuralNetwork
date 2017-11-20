using FreeImageAPI;
using MyFantasy.NeuralNetwork.Common;
using MyFantasy.NeuralNetwork.PictureTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace test_app
{
    class Program
    {
        static void Main(string[] args)
        {
            PictureTest();
            return;
            //XorTest();
        }

        public static void PictureTest()
        {
            Console.WriteLine(320 / 20 * 2 - 1);
            Console.WriteLine(240 / 20 * 2 - 1);

            MemoryBasedStream mbs = new MemoryBasedStream(File.ReadAllBytes(@"C:\Users\scherbina\Pictures\Тест\IMG_20171118_182026_1.jpg"));

            FreeImageBitmap fib = new FreeImageBitmap(mbs);

            mbs.Position = 0;

            var f = FieldExtention.LoadPictureGray(mbs, 320, 240);

            var f_res = FieldExtention.ResultObjectTypeFiled(fib.Width, fib.Height, 320, 240, 31 * 5, 23 * 5, new List<Tuple<int, int, int, int>>() {
                new Tuple<int, int, int, int>(480, 1192, 940, 1751),
                new Tuple<int, int, int, int>(1050,1100, 1500, 1628),
                new Tuple<int, int, int, int>(1576, 1483, 1925, 1990)
            });



            ConvNeuralnet cn = ConvNeuralnet.CreateSimple(NeuralnetField.CreateSimple(
                new List<int>() { 20 * 20, 20 * 20, 5 * 5 },
                new List<int>() { (320 / 20 * 2 - 1) * 5, (240 / 20 * 2 - 1) * 5 },
                new List<int>() { 320 / 20 * 2 - 1, 240 / 20 * 2 - 1 },
                "GetPointFromField_Point_half_20x20",
                "SetResultToField_Point_5x5"));

            cn = Learning.RandomLearn(cn, 10, 0.5, 0.2, new List<Learning.IOBlockFiled>() { new Learning.IOBlockFiled(f, f_res) });

            var f2 = cn.Calc(f);


            FieldDisplay_2D(f_res);
            Console.WriteLine();
            Console.WriteLine();
            FieldDisplay_2D(f2);

            Console.ReadKey();

        }

        public static void PictureResizeAndSave()
        {
            MemoryBasedStream mbs = new MemoryBasedStream(File.ReadAllBytes(@"C:\Users\scherbina\Pictures\Тест\IMG_20171118_182026_1.jpg"));

            var f = FieldExtention.LoadPictureGray(mbs, 320, 240);

            FreeImageBitmap fib = new FreeImageBitmap(320, 240);

            for (int i = 0; i < 320; i++)
            {
                for (int j = 0; j < 240; j++)
                {
                    fib.SetPixel(i, j, System.Drawing.Color.FromArgb(255 - (int)((f[i,j]+1)*127), 255 - (int)((f[i, j] + 1) * 127), 255 - (int)((f[i, j] + 1) * 127)));
                }
            }

            fib.Save(@"C:\Users\scherbina\Pictures\Тест\tst.jpg");

        }

        public static void XorTest()
        {
            var net = Neuralnet.CreateSimple(new System.Collections.Generic.List<int>() { 2,5,2 });

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


            Field input_filed = new Field(4, 2)
                .AddOrUpdate(-1, 0, 0)
                .AddOrUpdate(-1, 0, 1)

                .AddOrUpdate(1, 1, 0)
                .AddOrUpdate(-1, 1, 1)

                .AddOrUpdate(-1, 2, 0)
                .AddOrUpdate(1, 2, 1)

                .AddOrUpdate(1, 3, 0)
                .AddOrUpdate(1, 3, 1);

            Field output_filed = new Field(4)
                .AddOrUpdate(0.8, 0)
                .AddOrUpdate(-0.8, 1)
                .AddOrUpdate(-0.8, 2)
                .AddOrUpdate(0.8, 3);

            NeuralnetField nf = new NeuralnetField();
            nf.net = n4;
            nf.GetPointFromField = NeuralnetField.GetPointFromField_2Point;
            nf.GetPointFromField_name = "GetPointFromField_2Point";
            nf.out_field_size = new int[1] { 4 };
            nf.step_count.Add(4);
            nf.step_count.Add(1);

            var nf2 = nf.Copy();

            var f = nf2.Calc(input_filed);


            ConvNeuralnet cn = ConvNeuralnet.CreateSimple(NeuralnetField.CreateSimple(
                new List<int>() { 2, 5, 1 },
                new List<int>() { 4 },
                new List<int>() { 4, 1 },
                "GetPointFromField_2Point",
                "SetResultToField_Point"));

            cn = Learning.RandomLearn(cn, 1000, 0.5, 0.2, new List<Learning.IOBlockFiled>() { new Learning.IOBlockFiled(input_filed, output_filed) });

            var f2 = cn.Calc(input_filed);

            //NeuralnetField nf = new NeuralnetField();
            //nf.step_count = new List<int>() { 2, 3, 4 };

            //var v = nf.Calc(null);

            Console.WriteLine("0, 0, " + n3.Calc(new Dictionary<long, double>() { { 0, -0.8 }, { 1, -0.8 } })[0]);
            Console.WriteLine("0, 1, " + n3.Calc(new Dictionary<long, double>() { { 0, -0.8 }, { 1, 0.8 } })[0]);
            Console.WriteLine("1, 0, " + n3.Calc(new Dictionary<long, double>() { { 0, 0.8 }, { 1, -0.8 } })[0]);
            Console.WriteLine("1, 1, " + n3.Calc(new Dictionary<long, double>() { { 0, 0.8 }, { 1, 0.8 } })[0]);

            Console.WriteLine();

            Console.WriteLine("0, 0, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, -1 }, { 1, -1 } })));
            Console.WriteLine("0, 1, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, -1 }, { 1, 1 } })));
            Console.WriteLine("1, 0, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, 1 }, { 1, -1 } })));
            Console.WriteLine("1, 1, " + DictDisplay(n4.Calc(new Dictionary<long, double>() { { 0, 1 }, { 1, 1 } })));

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(FieldDisplay(input_filed));

            Console.WriteLine();

            Console.WriteLine(FieldDisplay(f2));

            Console.WriteLine();
            Console.WriteLine(cn.ToDSO().TryGetJson());

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


        public static string FieldDisplay(Field f)
        {
            StringBuilder sb = new StringBuilder("");

            int max = f.size.Aggregate((acc, next) => acc.IfDefault(1).Value * next);
            int c = f.size.Count;
            bool b = false;

            foreach (var point in f.values.OrderBy(g=>g.Key))
            {
                if (b)
                {
                    sb.AppendLine();
                }
                int[] p = new int[c];
                int mult = max;
                int z = (int)point.Key;
                for (int k = c - 1; k >= 0; k--)
                {
                    mult = mult / f.size[k];
                    p[k] = z / mult;
                    z = z % mult;
                }

                for (int i = 0; i < p.Length; i++)
                {
                    sb.Append(p[i]);
                    sb.Append("\t");
                }
                sb.Append(point.Value.ToString("0.000"));

                b = true;
            }

            return sb.ToString();
        }

        public static void FieldDisplay_2D(Field f)
        {

            for (int j = 0; j < f.size[1]; j++)
            {
                for (int i = 0; i < f.size[0]; i++)
                {                
                    if (f[i, j] > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(f[i, j].ToString("0.0") + "");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write((-f[i, j]).ToString("0.0") + "");
                    }
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
            
        }
    }
}
