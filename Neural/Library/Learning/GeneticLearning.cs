namespace Neural
{
    using System;
    using System.Diagnostics;
    
    public class GeneticLearning : ISupervisedLearning
    {

        private ActivationNetwork network;

        private int numberOfNetworksWeights;


        private Population population;

        private int populationSize;


        private IRandomNumberGenerator chromosomeGenerator;

        private IRandomNumberGenerator mutationMultiplierGenerator;
        private IRandomNumberGenerator mutationAdditionGenerator;


        private ISelectionMethod selectionMethod;


        private double crossOverRate;

        private double mutationRate;

        private double randomSelectionRate;


        public GeneticLearning( ActivationNetwork activationNetwork, int populationSize,
            IRandomNumberGenerator chromosomeGenerator,
            IRandomNumberGenerator mutationMultiplierGenerator,
            IRandomNumberGenerator mutationAdditionGenerator,
            ISelectionMethod selectionMethod,
            double crossOverRate, double mutationRate, double randomSelectionRate )
        {

            Debug.Assert( activationNetwork != null );
            Debug.Assert( populationSize > 0 );
            Debug.Assert( chromosomeGenerator != null );
            Debug.Assert( mutationMultiplierGenerator != null );
            Debug.Assert( mutationAdditionGenerator != null );
            Debug.Assert( selectionMethod != null );
            Debug.Assert( crossOverRate >= 0.0 && crossOverRate <= 1.0 );
            Debug.Assert( mutationRate >= 0.0 && crossOverRate <= 1.0 );
            Debug.Assert( randomSelectionRate >= 0.0 && randomSelectionRate <= 1.0 );


            this.network = activationNetwork;
            this.numberOfNetworksWeights = CalculateNetworkSize( activationNetwork );


            this.populationSize = populationSize;
            this.chromosomeGenerator = chromosomeGenerator;
            this.mutationMultiplierGenerator = mutationMultiplierGenerator;
            this.mutationAdditionGenerator = mutationAdditionGenerator;
            this.selectionMethod = selectionMethod;
            this.crossOverRate = crossOverRate;
            this.mutationRate = mutationRate;
            this.randomSelectionRate = randomSelectionRate;
        }

        public GeneticLearning(ActivationNetwork activationNetwork, int populationSize)
        {

            Debug.Assert( activationNetwork != null );
            Debug.Assert( populationSize > 0 );


            this.network = activationNetwork;
            this.numberOfNetworksWeights = CalculateNetworkSize( activationNetwork );


            this.populationSize = populationSize;
            this.chromosomeGenerator = new UniformGenerator( new Range( -1, 1 ) );
            this.mutationMultiplierGenerator = new ExponentialGenerator( 1 );
            this.mutationAdditionGenerator = new UniformGenerator( new Range( -0.5f, 0.5f ) );
            this.selectionMethod = new EliteSelection( );
            this.crossOverRate = 0.75;
            this.mutationRate = 0.25;
            this.randomSelectionRate = 0.2;
        }


        private int CalculateNetworkSize( ActivationNetwork activationNetwork )
        {

            int networkSize = 0;

            for ( int i = 0; i < network.Layers.Length; i++ )
            {
                Layer layer = network.Layers[i];

                for ( int j = 0; j < layer.Neurons.Length; j++ )
                {

                    networkSize += layer.Neurons[j].Weights.Length + 1;
                }
            }

            return networkSize;
        }


        public double Run( double[] input, double[] output )
        {
            throw new NotImplementedException( "The method is not implemented by design." );
        }


        public double RunEpoch( double[][] input, double[][] output )
        {

            if ( population == null )
            {

                DoubleArrayChromosome chromosomeExample = new DoubleArrayChromosome(
                    chromosomeGenerator, mutationMultiplierGenerator, mutationAdditionGenerator,
                    numberOfNetworksWeights );


                population = new Population( populationSize, chromosomeExample,
                    new EvolutionaryFitness(network, input, output), selectionMethod);

                population.CrossoverRate = crossOverRate;
                population.MutationRate = mutationRate;
                population.RandomSelectionPortion = randomSelectionRate;
            }

            population.RunEpoch( );


            DoubleArrayChromosome chromosome = (DoubleArrayChromosome) population.BestChromosome;
            double[] chromosomeGenes = chromosome.Value;


            int v = 0;

            for ( int i = 0; i < network.Layers.Length; i++ )
            {
                Layer layer = network.Layers[i];

                for ( int j = 0; j < layer.Neurons.Length; j++ )
                {
                    ActivationNeuron neuron = layer.Neurons[j] as ActivationNeuron;

                    for ( int k = 0; k < neuron.Weights.Length; k++ )
                    {
                        neuron.Weights[k] = chromosomeGenes[v++];
                    }
                    neuron.Threshold = chromosomeGenes[v++];
                }
            }


            return 1.0 / chromosome.Fitness;
        }
    }
}