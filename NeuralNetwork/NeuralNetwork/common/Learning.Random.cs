using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nnet.common
{
    public partial class Learning
    {
        public static Neuralnet RandomLearn(Neuralnet net, int iteration, double abs_max_w, List<IOBlock> learn_list)
        {
            var rnd = new Random();

            var best_net = net.Copy();            
            var state = Learning.d_sq(best_net, learn_list);

            for (int i = 0; i < iteration; i++)
            {
                var current_net = net.Copy();
                foreach (var n in current_net.net)
                {
                    foreach (var w in n.Value.w.Keys.ToList())
                    {
                        n.Value.w[w] = rnd.NextDouble() * 2 * abs_max_w - abs_max_w;
                    }
                }
                var state_next = Learning.d_sq(current_net, learn_list);
                if (state_next < state)
                {
                    best_net = current_net;
                    state = state_next;
                }
            }

            return best_net;
        }

        public static Neuralnet RandomLearn(Neuralnet net, int iteration, double abs_delta_max_w, double persent_upd, List<IOBlock> learn_list)
        {
            var rnd = new Random();

            var best_net = net.Copy();
            var state = Learning.d_sq(best_net, learn_list);

            for (int i = 0; i < iteration; i++)
            {
                var current_net = best_net.Copy();
                foreach (var n in current_net.net)
                {
                    foreach (var w in n.Value.w.Keys.ToList())
                    {
                        if (persent_upd < rnd.NextDouble())
                        {
                            n.Value.w[w] = n.Value.w[w] + rnd.NextDouble() * 2 * abs_delta_max_w - abs_delta_max_w;
                        }
                    }
                }
                var state_next = Learning.d_sq(current_net, learn_list);
                if (state_next < state)
                {
                    best_net = current_net;
                    state = state_next;
                }
            }

            return best_net;
        }


        public static ConvNeuralnet RandomLearn(ConvNeuralnet cn, int iteration, double abs_delta_max_w, double persent_upd, List<IOBlockFiled> learn_list)
        {
            var rnd = new Random();

            var best_cn = cn.Copy();
            var state = Learning.d_sq(best_cn, learn_list);

            for (int i = 0; i < iteration; i++)
            {
                var current_cn = best_cn.Copy();
                foreach (var layers in current_cn.layers)
                {
                    foreach (var n in layers.net.net)
                    {
                        foreach (var w in n.Value.w.Keys.ToList())
                        {
                            if (persent_upd < rnd.NextDouble())
                            {
                                n.Value.w[w] = n.Value.w[w] + rnd.NextDouble() * 2 * abs_delta_max_w - abs_delta_max_w;
                            }
                        }
                    }
                }
                var state_next = Learning.d_sq(current_cn, learn_list);
                if (state_next < state)
                {
                    best_cn = current_cn;
                    state = state_next;
                }
            }

            return best_cn;
        }
    }
}
