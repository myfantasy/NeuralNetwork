using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFantasy.NeuralNetwork.Common
{
    public partial class NeuralnetField
    {
        public static Dictionary<string, Func<Field, int[], Dictionary<long, double>>> known_GetPointFromField_funcs = new Dictionary<string, Func<Field, int[], Dictionary<long, double>>>()
        {
            { "GetPointFromField_Point", GetPointFromField_Point },
            { "GetPointFromField_2Point", GetPointFromField_2Point },
            { "GetPointFromField_Point_5x5", GetFromField_Point_x(5, 5) },
            { "GetPointFromField_Point_8x8", GetFromField_Point_x(8, 8) },
            { "GetPointFromField_Point_10x10", GetFromField_Point_x(10, 10) },
            { "GetPointFromField_Point_16x16", GetFromField_Point_x(16, 16) },
            { "GetPointFromField_Point_20x20", GetFromField_Point_x(20, 20) },
            { "GetPointFromField_Point_40x40", GetFromField_Point_x(40, 40) },
            { "GetPointFromField_Point_half_5x5", GetFromField_Point_x_half(5, 5) },
            { "GetPointFromField_Point_half_8x8", GetFromField_Point_x_half(8, 8) },
            { "GetPointFromField_Point_half_10x10", GetFromField_Point_x_half(10, 10) },
            { "GetPointFromField_Point_half_16x16", GetFromField_Point_x_half(16, 16) },
            { "GetPointFromField_Point_half_20x20", GetFromField_Point_x_half(20, 20) },
            { "GetPointFromField_Point_half_40x40", GetFromField_Point_x_half(40, 40) },
        };

        public static Dictionary<string, Action<Field, int[], Dictionary<long, double>>> known_SetResultToField_funcs = new Dictionary<string, Action<Field, int[], Dictionary<long, double>>>()
        {
            { "SetResultToField_Point", SetResultToField_Point },
            { "SetResultToField_Point_1x1", SetResultToField_Point_x(1, 1) },
            { "SetResultToField_Point_2x2", SetResultToField_Point_x(2, 2) },
            { "SetResultToField_Point_3x3", SetResultToField_Point_x(3, 3) },
            { "SetResultToField_Point_4x4", SetResultToField_Point_x(4, 4) },
            { "SetResultToField_Point_5x5", SetResultToField_Point_x(5, 5) },
            { "SetResultToField_Point_8x8", SetResultToField_Point_x(8, 8) },
        };


        public static NeuralnetField CreateSimple(List<int> layers_size, List<int> out_field_size, List<int> step_count, string getPointFromField_name, string setResultToField_name)
        {
            NeuralnetField nf = new NeuralnetField();
            nf.net = Neuralnet.CreateSimple(layers_size);

            nf.out_field_size = out_field_size.ToArray();
            nf.step_count = step_count;

            nf.GetPointFromField_name = getPointFromField_name;
            nf.GetPointFromField = known_GetPointFromField_funcs[nf.GetPointFromField_name];

            nf.SetResultToField_name = setResultToField_name;
            nf.SetResultToField = known_SetResultToField_funcs[nf.SetResultToField_name];

            return nf;
        }


        public static Dictionary<long, double> GetPointFromField_Point(Field f, int[] p)
        {
            return new Dictionary<long, double>() { { 0, f[p] } };
        }
        public static Dictionary<long, double> GetPointFromField_2Point(Field f, int[] p)
        {
            return new Dictionary<long, double>() { { 0, f[p] }, { 1, f[p.CloneArr().Change(1, 1)] } };
        }


        //public static Dictionary<long, double> GetFromField_Point_16x16(Field f, int[] p)
        //{
        //    var res = new Dictionary<long, double>();
        //    int k = 0;

        //    for (int i = 0; i < 16; i++)
        //    {
        //        for (int j = 0; j < 16; j++)
        //        {
        //            res[k] = f[p.CloneArr().Mult(0, 16).Mult(1, 16).AddVal(0, i).AddVal(1, j)];

        //            k++;
        //        }
        //    }

        //    return res;
        //}

        public static Func<Field, int[], Dictionary<long, double>> GetFromField_Point_x(int x, int y)
        {
            return (f, p) =>
            {
                var res = new Dictionary<long, double>();
                int k = 0;

                var p0 = p.CloneArr().Mult(0, x).Mult(1, y);

                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        res[k] = f[p0.CloneArr().AddVal(0, i).AddVal(1, j)];

                        k++;
                    }
                }

                return res;
            };
        }

        public static Func<Field, int[], Dictionary<long, double>> GetFromField_Point_x_half(int x, int y)
        {
            return (f, p) =>
            {
                var res = new Dictionary<long, double>();
                int k = 0;

                var p0 = p.CloneArr().Mult(0, x / 2).Mult(1, y / 2);

                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        res[k] = f[p0.CloneArr().AddVal(0, i).AddVal(1, j)];

                        k++;
                    }
                }

                return res;
            };
        }


        public static void SetResultToField_Point(Field f, int[] p, Dictionary<long, double> res)
        {
            f[p] = res[0];
        }

        public static Action<Field, int[], Dictionary<long, double>> SetResultToField_Point_x(int x, int y)
        {
            return (f, p, res) =>
            {
                int k = 0;

                var p0 = p.CloneArr().Mult(0, x).Mult(1, y);

                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        f[p0.CloneArr().AddVal(0, i).AddVal(1, j)] = res[k];

                        k++;
                    }
                }
            };
        }


        public NeuralnetField Copy()
        {
            return FromDSO(ToDSO());
        }

        public DSO ToDSO()
        {
            var r = new DSO();
            r.Add("net", net.ToDSO());
            r.Add("gpff", GetPointFromField_name);
            r.Add("srtf", SetResultToField_name);

            r.Add("step_count", step_count.Select(f => (object)f).ToList());
            r.Add("out_field_size", out_field_size.Select(f => (object)f).ToList());
            
            return r;
        }

        public static NeuralnetField FromDSO(Dictionary<string, object> r)
        {
            NeuralnetField nf = new NeuralnetField();

            nf.net = Neuralnet.FromDSO(r.GetElement<Dictionary<string, object>>("net"));

            nf.GetPointFromField_name = r.GetElement<string>("gpff");
            nf.GetPointFromField = known_GetPointFromField_funcs[nf.GetPointFromField_name];

            nf.SetResultToField_name = r.GetElement<string>("srtf");
            nf.SetResultToField = known_SetResultToField_funcs[nf.SetResultToField_name];

            nf.step_count = r.GetElement<List<object>>("step_count").Select(f=>(int)f).ToList();

            nf.out_field_size = r.GetElement<List<object>>("out_field_size").Select(f => (int)f).ToArray();
            
            return nf;
        }
    }
}
