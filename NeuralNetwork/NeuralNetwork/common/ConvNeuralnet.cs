using System;
using System.Collections.Generic;
using System.Text;

namespace MyFantasy.NeuralNetwork.Common
{
    /// <summary>
    /// convolutional neural network
    /// </summary>
    public partial class ConvNeuralnet
    {
        /// <summary>
        /// layers NeuralnetField of ConvNeuralnet
        /// </summary>
        public List<NeuralnetField> layers = new List<NeuralnetField>();
        

        public Field Calc(Field input)
        {
            Field res = input;

            for (int i = 0; i < layers.Count; i++)
            {
                res = layers[i].Calc(res);
            }

            return res;
        }

    }
}
