using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nnet.common
{
    public partial class NeuralnetField
    {
        public static Dictionary<string, Func<Field, int[], Dictionary<long, double>>> known_GetPointFromField_funcs = new Dictionary<string, Func<Field, int[], Dictionary<long, double>>>()
        {
            { "GetPointFromField_Point", GetPointFromField_Point },
            { "GetPointFromField_2Point", GetPointFromField_2Point },
        };

        public static Dictionary<string, Action<Field, int[], Dictionary<long, double>>> known_SetResultToField_funcs = new Dictionary<string, Action<Field, int[], Dictionary<long, double>>>()
        {
            { "SetResultToField_Point", SetResultToField_Point }
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

        public static void SetResultToField_Point(Field f, int[] p, Dictionary<long, double> res)
        {
            f[p] = res[0];
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
