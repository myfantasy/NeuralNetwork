using System;
using System.Collections.Generic;
using System.Text;

namespace nnet.common
{
    /// <summary>
    /// convolutional neural network
    /// </summary>
    public partial class ConvNeuralnet
    {
        /// <summary>
        /// network of Neuralnet
        /// </summary>
        public Dictionary<long, Neuralnet> net = new Dictionary<long, Neuralnet>();

        

        /// <summary>
        /// layers Neuralnet of ConvNeuralnet
        /// </summary>
        public List<long> layers = new List<long>();




        public Field Calc(Field input)
        {
            return null;
        }

    }
}
