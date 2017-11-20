using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFantasy.NeuralNetwork.Common
{
    /// <summary>
    /// Use the Neuralnet over Field iterative
    /// </summary>
    public partial class NeuralnetField
    {

        /// <summary>
        /// Neuralnet
        /// </summary>
        public Neuralnet net = new Neuralnet();
        
        public Func<Field, int[], Dictionary<long, double>> GetPointFromField = GetPointFromField_Point;
        public string GetPointFromField_name = "GetPointFromField_Point";

        /// <summary>
        /// steps by dimentions
        /// </summary>
        public List<int> step_count = new List<int>();

        /// <summary>
        /// Output field size
        /// </summary>
        public int[] out_field_size = new int[0];

        public Action<Field, int[], Dictionary<long, double>> SetResultToField = SetResultToField_Point;
        public string SetResultToField_name = "SetResultToField_Point";


        public Field Calc(Field input)
        {
            Field res = new Field(out_field_size);

            int max = step_count.Aggregate((acc, next) => acc.IfDefault(1).Value * next);
            int c = step_count.Count;

            for (int i = 0; i < max; i++)
            {
                int[] p = new int[c];
                int mult = max;
                int z = i;
                for (int k = c - 1; k >= 0; k--)
                {
                    mult = mult / step_count[k];
                    p[k] = z / mult;
                    z = z % mult;
                }

                var d = GetPointFromField(input, p);

                var no = net.Calc(d);

                SetResultToField(res, p, no);

            }
            
            return res;

        }

    }
}
