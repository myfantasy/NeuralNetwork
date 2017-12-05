using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFantasy.NeuralNetwork.Common
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


        public static ConvNeuralnet RandomLearn(ConvNeuralnet cn, int iteration, double abs_delta_max_w, double persent_upd
            , List<IOBlockFiled> learn_list, bool verbouse = true, bool is_full_random = false
            , double? sig_persent_upd = null
            , Action<ConvNeuralnet> do_on_learn = null)
        {
            var rnd = new Random();

            var best_cn = cn.Copy();
            var state = Learning.d_sq(best_cn, learn_list) * (is_full_random ? 10 : 1);
            var state_next = state;

            sig_persent_upd = sig_persent_upd ?? persent_upd;

            for (int i = 0; i < iteration; i++)
            {
                if (verbouse)
                {
                    Console.Write("\r");
                    Console.Write(i);
                    Console.Write(" of ");
                    Console.Write(iteration);
                    Console.Write(" res ");
                    Console.Write(state);
                    Console.Write(" ns ");
                    Console.Write(state_next);
                }

                var current_cn = best_cn.Copy();
                foreach (var layers in current_cn.layers)
                {
                    foreach (var n in layers.net.net)
                    {
                        foreach (var w in n.Value.w.Keys.ToList())
                        {
                            if (w != -1 && persent_upd < rnd.NextDouble() || w == -1 && sig_persent_upd < rnd.NextDouble())
                            {
                                if (is_full_random)
                                {
                                    n.Value.w[w] = rnd.NextDouble() * 2 * abs_delta_max_w - abs_delta_max_w;
                                }
                                else
                                {
                                    n.Value.w[w] = n.Value.w[w] + rnd.NextDouble() * 2 * abs_delta_max_w - abs_delta_max_w;
                                }
                            }
                            //else if (is_full_random)
                            //{
                            //    n.Value.w[w] = 0;
                            //}
                        }
                    }
                }
                state_next = Learning.d_sq(current_cn, learn_list);
                if (state_next < state)
                {
                    best_cn = current_cn;
                    state = state_next;

                    if (do_on_learn != null)
                    {
                        do_on_learn(best_cn);
                    }
                }
            }

            if (verbouse)
            {
                Console.WriteLine("complite");
            }

            return best_cn;
        }
    }
}
