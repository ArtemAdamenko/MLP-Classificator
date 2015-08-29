namespace Neural
{
    using System;

    
    public class BackPropagationLearning : ISupervisedLearning
    {

        private ActivationNetwork network;

        private double learningRate = 0.1;

        private double momentum = 0.0;


        private double[][] neuronErrors = null;

        private double[][][] weightsUpdates = null;

        private double[][] thresholdsUpdates = null;


        public double LearningRate
        {
            get { return learningRate; }
            set
            {
                learningRate = Math.Max( 0.0, Math.Min( 1.0, value ) );
            }
        }


        public double Momentum
        {
            get { return momentum; }
            set
            {
                momentum = Math.Max( 0.0, Math.Min( 1.0, value ) );
            }
        }


        public BackPropagationLearning( ActivationNetwork network )
        {
            this.network = network;


            neuronErrors = new double[network.Layers.Length][];
            weightsUpdates = new double[network.Layers.Length][][];
            thresholdsUpdates = new double[network.Layers.Length][];


            for ( int i = 0; i < network.Layers.Length; i++ )
            {
                Layer layer = network.Layers[i];

                neuronErrors[i] = new double[layer.Neurons.Length];
                weightsUpdates[i] = new double[layer.Neurons.Length][];
                thresholdsUpdates[i] = new double[layer.Neurons.Length];


                for ( int j = 0; j < weightsUpdates[i].Length; j++ )
                {
                    weightsUpdates[i][j] = new double[layer.InputsCount];
                }
            }
        }


        public double Run( double[] input, double[] output )
        {

            network.Compute( input );


            double error = CalculateError( output );


            CalculateUpdates( input );


            UpdateNetwork( );

            return error;
        }


        public double RunEpoch( double[][] input, double[][] output )
        {
            double error = 0.0;


            for ( int i = 0; i < input.Length; i++ )
            {
                error += Run( input[i], output[i] );
            }


            return error;
        }


        private double CalculateError( double[] desiredOutput )
        {

            Layer layer, layerNext;

            double[] errors, errorsNext;

            double error = 0, e, sum;

            double output;

            int layersCount = network.Layers.Length;


            IActivationFunction function = ( network.Layers[0].Neurons[0] as ActivationNeuron ).ActivationFunction;


            layer = network.Layers[layersCount - 1];
            errors = neuronErrors[layersCount - 1];

            for ( int i = 0; i < layer.Neurons.Length; i++ )
            {
                output = layer.Neurons[i].Output;

                e = desiredOutput[i] - output;

                errors[i] = e * function.Derivative2( output );

                error += ( e * e );
            }


            for ( int j = layersCount - 2; j >= 0; j-- )
            {
                layer = network.Layers[j];
                layerNext = network.Layers[j + 1];
                errors = neuronErrors[j];
                errorsNext = neuronErrors[j + 1];


                for ( int i = 0; i < layer.Neurons.Length; i++ )
                {
                    sum = 0.0;

                    for ( int k = 0; k < layerNext.Neurons.Length; k++ )
                    {
                        sum += errorsNext[k] * layerNext.Neurons[k].Weights[i];
                    }
                    errors[i] = sum * function.Derivative2( layer.Neurons[i].Output );
                }
            }


            return error / 2.0;
        }


        private void CalculateUpdates( double[] input )
        {

            Neuron neuron;

            Layer layer, layerPrev;

            double[][] layerWeightsUpdates;

            double[] layerThresholdUpdates;

            double[] errors;

            double[] neuronWeightUpdates;

            layer = network.Layers[0];
            errors = neuronErrors[0];
            layerWeightsUpdates = weightsUpdates[0];
            layerThresholdUpdates = thresholdsUpdates[0];


            double cachedMomentum = learningRate * momentum;
            double cached1mMomentum = learningRate * ( 1 - momentum );
            double cachedError;


            for ( int i = 0; i < layer.Neurons.Length; i++ )
            {
                neuron = layer.Neurons[i];
                cachedError = errors[i] * cached1mMomentum;
                neuronWeightUpdates = layerWeightsUpdates[i];


                for ( int j = 0; j < neuronWeightUpdates.Length; j++ )
                {

                    neuronWeightUpdates[j] = cachedMomentum * neuronWeightUpdates[j] + cachedError * input[j];
                }


                layerThresholdUpdates[i] = cachedMomentum * layerThresholdUpdates[i] + cachedError;
            }


            for ( int k = 1; k < network.Layers.Length; k++ )
            {
                layerPrev = network.Layers[k - 1];
                layer = network.Layers[k];
                errors = neuronErrors[k];
                layerWeightsUpdates = weightsUpdates[k];
                layerThresholdUpdates = thresholdsUpdates[k];


                for ( int i = 0; i < layer.Neurons.Length; i++ )
                {
                    neuron = layer.Neurons[i];
                    cachedError = errors[i] * cached1mMomentum;
                    neuronWeightUpdates = layerWeightsUpdates[i];


                    for ( int j = 0; j < neuronWeightUpdates.Length; j++ )
                    {

                        neuronWeightUpdates[j] = cachedMomentum * neuronWeightUpdates[j] + cachedError * layerPrev.Neurons[j].Output;
                    }


                    layerThresholdUpdates[i] = cachedMomentum * layerThresholdUpdates[i] + cachedError;
                }
            }
        }


        private void UpdateNetwork( )
        {

            ActivationNeuron neuron;

            Layer layer;

            double[][] layerWeightsUpdates;

            double[] layerThresholdUpdates;

            double[] neuronWeightUpdates;


            for ( int i = 0; i < network.Layers.Length; i++ )
            {
                layer = network.Layers[i];
                layerWeightsUpdates = weightsUpdates[i];
                layerThresholdUpdates = thresholdsUpdates[i];


                for ( int j = 0; j < layer.Neurons.Length; j++ )
                {
                    neuron = layer.Neurons[j] as ActivationNeuron;
                    neuronWeightUpdates = layerWeightsUpdates[j];


                    for ( int k = 0; k < neuron.Weights.Length; k++ )
                    {

                        neuron.Weights[k] += neuronWeightUpdates[k];
                    }

                    neuron.Threshold += layerThresholdUpdates[j];
                }
            }
        }
    }
}
