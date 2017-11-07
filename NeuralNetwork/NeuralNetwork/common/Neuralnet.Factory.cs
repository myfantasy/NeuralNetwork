using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nnet.common
{
    public partial class Neuralnet
    {
        public static Neuralnet CreateSimple(List<int> layers_size)
        {
            Neuralnet net = new Neuralnet();

            long id = 0;

            net.layers.Add(new List<long>());

            for (int j = 0; j < layers_size[0]; j++)
            {
                net.layers[0].Add(id);
                id++;
            }

            for (int i = 1; i < layers_size.Count; i++)
            {
                net.layers.Add(new List<long>());
                for (int j = 0; j < layers_size[i]; j++)
                {
                    Neuron n = Neuron.Create("th", net.layers[i - 1]);
                    n.w.Add(-1, 0);

                    net.net.Add(id, n);
                    net.layers[i].Add(id);

                    id++;
                }
            }

            int k = 0;
            for (int j = 0; j < net.layers[net.layers.Count - 1].Count; j++)
            {
                net.out_layer.Add(k, Neuron.Create("line", new Dictionary<long, double>() { { net.layers[net.layers.Count - 1][j], 1 } }));
                k++;
            }

            return net;
        }

        public Neuralnet Copy()
        {
            return FromDSO(ToDSO());
        }

        public DSO ToDSO()
        {
            var r = new DSO();
            r.Add("net", (object)this.net.ToDictionary(f => f.Key.ToString(), f => (object)f.Value.ToDSO()));
            DSO l = new DSO();
            for (int i = 0; i < layers.Count; i++)
            {
                l.Add(i.ToString(), this.layers[i].Select(f=>(object)f).ToList());
            }
            r.Add("layers", l);
            r.Add("layers_count", layers.Count);
            r.Add("out_layer", this.out_layer.ToDictionary(f => f.Key.ToString(), f => (object)f.Value.ToDSO()));
            return r;
        }

        public static Neuralnet FromDSO(Dictionary<string, object> r)
        {
            Neuralnet net = new Neuralnet();

            net.net = r.GetElement<Dictionary<string, object>>("net").ToDictionary(f => Convert.ToInt64(f.Key), f => Neuron.FromDSO((DSO)f.Value));

            long lc = Convert.ToInt64(r.GetElement<object>("layers_count"));
            for (int i = 0; i < lc; i++)
            {
                net.layers.Add(r.GetElement<List<object>>("layers", i.ToString()).Select(f => (long)f).ToList());
            }

            net.out_layer = r.GetElement<Dictionary<string, object>>("out_layer").ToDictionary(f => Convert.ToInt64(f.Key), f => Neuron.FromDSO((DSO)f.Value));
            
            return net;
        }
    }
}
