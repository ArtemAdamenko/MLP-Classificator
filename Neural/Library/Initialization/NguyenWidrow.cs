namespace Neural
{
    using System;

    public class NguyenWidrow
    {
        private ActivationNetwork network;
        private Range randRange;
        private double beta;

        public NguyenWidrow(ActivationNetwork network)
        {
            this.network = network;

            int hiddenNodes = network.Layers[0].Neurons.Length;
            int inputNodes = network.Layers[0].InputsCount;

            randRange = new Range(-0.5f, 0.5f);
            beta = 0.7 * Math.Pow(hiddenNodes, 1.0 / inputNodes);
        }


        public void Randomize(int layerIndex)
        {
            Neuron.RandRange = randRange;

            for (int j = 0; j < network.Layers[layerIndex].Neurons.Length; j++)
            {
                ActivationNeuron neuron = network.Layers[layerIndex].Neurons[j] as ActivationNeuron;

                neuron.Randomize();

                double norm = 0.0;


                for (int k = 0; k < neuron.Weights.Length; k++)
                    norm += neuron.Weights[k] * neuron.Weights[k];
                norm += neuron.Threshold * neuron.Threshold;

                norm = System.Math.Sqrt(norm);


                for (int k = 0; k < neuron.InputsCount; k++)
                    neuron.Weights[k] = beta * neuron.Weights[k] / norm;
                neuron.Threshold = beta * neuron.Threshold / norm;
            }
        }


        public void Randomize()
        {
            for (int i = 0; i < network.Layers.Length; i++)
                Randomize(i);
        }

    }
}
