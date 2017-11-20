using System;
using System.Collections.Generic;
using System.Text;

namespace MyFantasy.NeuralNetwork.Common
{
    public partial class Neuralnet
    {
        /// <summary>
        /// neurons of network
        /// </summary>
        public Dictionary<long, Neuron> net = new Dictionary<long, Neuron>();

        /// <summary>
        /// 0 layer is intput other are
        /// calculated layers
        /// </summary>
        public List<List<long>> layers = new List<List<long>>();

        /// <summary>
        /// Output layer is layer with outpu signal id
        /// </summary>
        public Dictionary<long, Neuron> out_layer = new Dictionary<long, Neuron>();
        
        /// <summary>
        /// Calculate
        /// </summary>
        /// <param name="input">input stream</param>
        /// <returns></returns>
        public Dictionary<long, double> Calc(Dictionary<long, double> input)
        {
            Dictionary<long, double> vals = new Dictionary<long, double>() { { -1, 1 } };

            foreach (var j in layers[0])
            {
                vals.Add(j, input[j]);
            }

            for (int i = 1; i < layers.Count; i++)
            {
                foreach (var j in layers[i])
                {
                    vals.Add(j, net[j].Calc(vals));
                }
            }

            Dictionary<long, double> out_res = new Dictionary<long, double>();
            foreach (var o in out_layer)
            {
                out_res.Add(o.Key, o.Value.Calc(vals));
            }

            return out_res;
        }
        
    }
}
