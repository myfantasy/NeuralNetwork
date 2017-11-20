using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFantasy.NeuralNetwork.Common
{
    public partial class ConvNeuralnet
    {
        public static ConvNeuralnet CreateSimple(params NeuralnetField [] layers)
        {
            ConvNeuralnet cn = new ConvNeuralnet();
            cn.layers = layers.ToList();
            return cn;
        }

        public ConvNeuralnet Copy()
        {
            return FromDSO(ToDSO());
        }

        public DSO ToDSO()
        {
            var r = new DSO();

            r.Add("layers", layers.Select(f => (object)f.ToDSO()).ToList());

            return r;
        }

        public static ConvNeuralnet FromDSO(Dictionary<string, object> r)
        {
            ConvNeuralnet cn = new ConvNeuralnet();

            cn.layers = r.GetElement<List<object>>("layers").Select(f => NeuralnetField.FromDSO((Dictionary<string, object>)f)).ToList();

            return cn;
        }
    }
}
