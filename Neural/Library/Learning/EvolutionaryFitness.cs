namespace Neural
{
    using System;
    using System.Diagnostics;

    internal class EvolutionaryFitness : IFitnessFunction
    {

        private ActivationNetwork network;


        private double[][] input;


        private double[][] output;


        public EvolutionaryFitness( ActivationNetwork network, double[][] input, double[][] output )
        {
            if ( ( input.Length == 0 ) || ( input.Length != output.Length ) )
            {
                throw new ArgumentException( "Length of inputs and outputs arrays must be equal and greater than 0." );
            }

            if ( network.InputsCount != input[0].Length )
            {
                throw new ArgumentException( "Length of each input vector must be equal to neural network's inputs count." );
            }

            this.network = network;
            this.input = input;
            this.output = output;
        }


        public double Evaluate( IChromosome chromosome )
        {
            DoubleArrayChromosome daChromosome = (DoubleArrayChromosome) chromosome;
            double[] chromosomeGenes = daChromosome.Value;

            int totalNumberOfWeights = 0;


            for ( int i = 0, layersCount = network.Layers.Length; i < layersCount; i++ )
            {
                Layer layer = network.Layers[i];

                for ( int j = 0; j < layer.Neurons.Length; j++ )
                {
                    ActivationNeuron neuron = layer.Neurons[j] as ActivationNeuron;

                    for ( int k = 0; k < neuron.Weights.Length; k++ )
                    {
                        neuron.Weights[k] = chromosomeGenes[totalNumberOfWeights++];
                    }
                    neuron.Threshold = chromosomeGenes[totalNumberOfWeights++];
                }
            }

            Debug.Assert( totalNumberOfWeights == daChromosome.Length );

            double totalError = 0;

            for ( int i = 0, inputVectorsAmount = input.Length; i < inputVectorsAmount; i++ )
            {
                double[] computedOutput = network.Compute( input[i] );

                for ( int j = 0, outputLength = output[0].Length; j < outputLength; j++ )
                {
                    double error = output[i][j] - computedOutput[j];
                    totalError += error * error;
                }
            }

            if ( totalError > 0 )
                return 1.0 / totalError;


            return double.MaxValue;
        }
    }
}
