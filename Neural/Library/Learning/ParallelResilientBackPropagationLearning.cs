namespace Neural
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ParallelResilientBackpropagationLearning : ISupervisedLearning, IDisposable
    {
        private ActivationNetwork network;

        private double initialStep = 0.0125;
        private double deltaMax = 50.0;
        private double deltaMin = 1e-6;

        private double etaMinus = 0.5;
        private double etaPlus = 1.2;

        private Object lockNetwork = new Object();
        private ThreadLocal<double[][]> networkErrors;
        private ThreadLocal<double[][]> networkOutputs;


        private double[][][] weightsUpdates;
        private double[][] thresholdsUpdates;


        private double[][][] weightsDerivatives;
        private double[][] thresholdsDerivatives;

        private double[][][] weightsPreviousDerivatives;
        private double[][] thresholdsPreviousDerivatives;


        public double UpdateUpperBound
        {
            get { return deltaMax; }
            set { deltaMax = value; }
        }

        public double UpdateLowerBound
        {
            get { return deltaMin; }
            set { deltaMin = value; }
        }


        public double DecreaseFactor
        {
            get { return etaMinus; }
            set
            {
                if (value <= 0 || value >= 1)
                    throw new ArgumentOutOfRangeException("value", "Value should be between 0 and 1.");
                etaMinus = value;
            }
        }


        public double IncreaseFactor
        {
            get { return etaPlus; }
            set
            {
                if (value <= 1)
                    throw new ArgumentOutOfRangeException("value", "Value should be higher than 1.");
                etaPlus = value;
            }
        }

 
        public ParallelResilientBackpropagationLearning(ActivationNetwork network)
        {
            this.network = network;

            networkOutputs = new ThreadLocal<double[][]>(() => new double[network.Layers.Length][]);

            networkErrors = new ThreadLocal<double[][]>(() =>
            {
                var e = new double[network.Layers.Length][];
                for (int i = 0; i < e.Length; i++)
                    e[i] = new double[network.Layers[i].Neurons.Length];
                return e;
            });


            weightsDerivatives = new double[network.Layers.Length][][];
            thresholdsDerivatives = new double[network.Layers.Length][];

            weightsPreviousDerivatives = new double[network.Layers.Length][][];
            thresholdsPreviousDerivatives = new double[network.Layers.Length][];

            weightsUpdates = new double[network.Layers.Length][][];
            thresholdsUpdates = new double[network.Layers.Length][];

            for (int i = 0; i < network.Layers.Length; i++)
            {
                Layer layer = network.Layers[i];

                weightsDerivatives[i] = new double[layer.Neurons.Length][];
                weightsPreviousDerivatives[i] = new double[layer.Neurons.Length][];
                weightsUpdates[i] = new double[layer.Neurons.Length][];


                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    weightsDerivatives[i][j] = new double[layer.InputsCount];
                    weightsPreviousDerivatives[i][j] = new double[layer.InputsCount];
                    weightsUpdates[i][j] = new double[layer.InputsCount];
                }

                thresholdsDerivatives[i] = new double[layer.Neurons.Length];
                thresholdsPreviousDerivatives[i] = new double[layer.Neurons.Length];
                thresholdsUpdates[i] = new double[layer.Neurons.Length];
            }


            Reset(initialStep);
        }



        public double Run(double[] input, double[] output)
        {

            ResetGradient();


            network.Compute(input);


            var networkOutputs = this.networkOutputs.Value;
            for (int j = 0; j < networkOutputs.Length; j++)
                networkOutputs[j] = network.Layers[j].Output;


            double error = CalculateError(output);


            CalculateGradient(input);


            UpdateNetwork();

            return error;
        }


        public double RunEpoch(double[][] input, double[][] output)
        {

            ResetGradient();

            Object lockSum = new Object();
            double sumOfSquaredErrors = 0;



            Parallel.For(0, input.Length,


                () => 0.0,


                (i, loopState, partialSum) =>
                {

                    lock (lockNetwork)
                    {

                        network.Compute(input[i]);


                        var networkOutputs = this.networkOutputs.Value;
                        for (int j = 0; j < networkOutputs.Length; j++)
                            networkOutputs[j] = network.Layers[j].Output;
                    }


                    partialSum += CalculateError(output[i]);


                    CalculateGradient(input[i]);

                    return partialSum;
                },


                (partialSum) =>
                {
                    lock (lockSum) sumOfSquaredErrors += partialSum;
                }
            );



            UpdateNetwork();


            return sumOfSquaredErrors;
        }


        private void UpdateNetwork()
        {

            for (int i = 0; i < weightsUpdates.Length; i++)
            {
                ActivationLayer layer = this.network.Layers[i] as ActivationLayer;
                double[][] layerWeightsUpdates = weightsUpdates[i];
                double[] layerThresholdUpdates = thresholdsUpdates[i];

                double[][] layerWeightsDerivatives = weightsDerivatives[i];
                double[] layerThresholdDerivatives = thresholdsDerivatives[i];

                double[][] layerPreviousWeightsDerivatives = weightsPreviousDerivatives[i];
                double[] layerPreviousThresholdDerivatives = thresholdsPreviousDerivatives[i];


                for (int j = 0; j < layerWeightsUpdates.Length; j++)
                {
                    ActivationNeuron neuron = layer.Neurons[j] as ActivationNeuron;

                    double[] neuronWeightUpdates = layerWeightsUpdates[j];
                    double[] neuronWeightDerivatives = layerWeightsDerivatives[j];
                    double[] neuronPreviousWeightDerivatives = layerPreviousWeightsDerivatives[j];

                    double S;


                    for (int k = 0; k < neuronPreviousWeightDerivatives.Length; k++)
                    {
                        S = neuronPreviousWeightDerivatives[k] * neuronWeightDerivatives[k];

                        if (S > 0.0)
                        {
                            neuronWeightUpdates[k] = Math.Min(neuronWeightUpdates[k] * etaPlus, deltaMax);
                            neuron.Weights[k] -= Math.Sign(neuronWeightDerivatives[k]) * neuronWeightUpdates[k];
                            neuronPreviousWeightDerivatives[k] = neuronWeightDerivatives[k];
                        }
                        else if (S < 0.0)
                        {
                            neuronWeightUpdates[k] = Math.Max(neuronWeightUpdates[k] * etaMinus, deltaMin);
                            neuronPreviousWeightDerivatives[k] = 0.0;
                        }
                        else
                        {
                            neuron.Weights[k] -= Math.Sign(neuronWeightDerivatives[k]) * neuronWeightUpdates[k];
                            neuronPreviousWeightDerivatives[k] = neuronWeightDerivatives[k];
                        }
                    }

                    S = layerPreviousThresholdDerivatives[j] * layerThresholdDerivatives[j];

                    if (S > 0.0)
                    {
                        layerThresholdUpdates[j] = Math.Min(layerThresholdUpdates[j] * etaPlus, deltaMax);
                        neuron.Threshold -= Math.Sign(layerThresholdDerivatives[j]) * layerThresholdUpdates[j];
                        layerPreviousThresholdDerivatives[j] = layerThresholdDerivatives[j];
                    }
                    else if (S < 0.0)
                    {
                        layerThresholdUpdates[j] = Math.Max(layerThresholdUpdates[j] * etaMinus, deltaMin);
                        layerThresholdDerivatives[j] = 0.0;
                    }
                    else
                    {
                        neuron.Threshold -= Math.Sign(layerThresholdDerivatives[j]) * layerThresholdUpdates[j];
                        layerPreviousThresholdDerivatives[j] = layerThresholdDerivatives[j];
                    }
                }
            }
        }

        public double ComputeError(double[][] input, double[][] output)
        {
            Object lockSum = new Object();
            double sumOfSquaredErrors = 0;


            Parallel.For(0, input.Length,


                () => 0.0,

                // Map
                (i, loopState, partialSum) =>
                {
                    double[] y = network.Compute(input[i]);

                    for (int j = 0; j < y.Length; j++)
                    {
                        double e = (y[j] - output[i][j]);
                        partialSum += e * e;
                    }

                    return partialSum;
                },


                (partialSum) =>
                {
                    lock (lockSum) sumOfSquaredErrors += partialSum;
                }
            );

            return sumOfSquaredErrors / 2.0;
        }

        public void Reset(double rate)
        {
            Parallel.For(0, weightsUpdates.Length, i =>
            {
                for (int j = 0; j < weightsUpdates[i].Length; j++)
                    for (int k = 0; k < weightsUpdates[i][j].Length; k++)
                        weightsUpdates[i][j][k] = rate;

                for (int j = 0; j < thresholdsUpdates[i].Length; j++)
                    thresholdsUpdates[i][j] = rate;
            });
        }

        private void ResetGradient()
        {
            Parallel.For(0, weightsDerivatives.Length, i =>
            {
                for (int j = 0; j < weightsDerivatives[i].Length; j++)
                    Array.Clear(weightsDerivatives[i][j], 0, weightsDerivatives[i][j].Length);
                Array.Clear(thresholdsDerivatives[i], 0, thresholdsDerivatives[i].Length);
            });
        }


        private double CalculateError(double[] desiredOutput)
        {
            double sumOfSquaredErrors = 0.0;
            int layersCount = network.Layers.Length;

            double[][] networkErrors = this.networkErrors.Value;
            double[][] networkOutputs = this.networkOutputs.Value;


            var function = (this.network.Layers[0].Neurons[0] as ActivationNeuron)
                .ActivationFunction;


            double[] layerOutputs = networkOutputs[layersCount - 1];
            double[] errors = networkErrors[layersCount - 1];

            for (int i = 0; i < errors.Length; i++)
            {
                double output = layerOutputs[i];
                double e = output - desiredOutput[i];
                errors[i] = e * function.Derivative2(output);
                sumOfSquaredErrors += e * e;
            }


            for (int j = layersCount - 2; j >= 0; j--)
            {
                errors = networkErrors[j];
                layerOutputs = networkOutputs[j];

                ActivationLayer nextLayer = network.Layers[j + 1] as ActivationLayer;
                double[] nextErrors = networkErrors[j + 1];


                for (int i = 0; i < errors.Length; i++)
                {
                    double sum = 0.0;


                    for (int k = 0; k < nextErrors.Length; k++)
                        sum += nextErrors[k] * nextLayer.Neurons[k].Weights[i];

                    errors[i] = sum * function.Derivative2(layerOutputs[i]);
                }
            }

            return sumOfSquaredErrors / 2.0;
        }


        private void CalculateGradient(double[] input)
        {
            double[][] networkErrors = this.networkErrors.Value;
            double[][] networkOutputs = this.networkOutputs.Value;


            double[] errors = networkErrors[0];
            double[] outputs = networkOutputs[0];
            double[][] layerWeightsDerivatives = weightsDerivatives[0];
            double[] layerThresholdDerivatives = thresholdsDerivatives[0];


            for (int i = 0; i < errors.Length; i++)
            {
                double[] neuronWeightDerivatives = layerWeightsDerivatives[i];

                lock (neuronWeightDerivatives)
                {

                    for (int j = 0; j < input.Length; j++)
                        neuronWeightDerivatives[j] += errors[i] * input[j];
                    layerThresholdDerivatives[i] += errors[i];
                }
            }


            for (int k = 1; k < weightsDerivatives.Length; k++)
            {
                errors = networkErrors[k];
                outputs = networkOutputs[k];

                layerWeightsDerivatives = weightsDerivatives[k];
                layerThresholdDerivatives = thresholdsDerivatives[k];

                double[] layerPrev = networkOutputs[k - 1];


                for (int i = 0; i < layerWeightsDerivatives.Length; i++)
                {
                    double[] neuronWeightDerivatives = layerWeightsDerivatives[i];

                    lock (neuronWeightDerivatives)
                    {

                        for (int j = 0; j < neuronWeightDerivatives.Length; j++)
                            neuronWeightDerivatives[j] += errors[i] * layerPrev[j];
                        layerThresholdDerivatives[i] += errors[i];
                    }
                }
            }
        }

        #region IDisposable Members


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        ~ParallelResilientBackpropagationLearning()
        {
            Dispose(false);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (networkErrors != null)
                {
                    networkErrors.Dispose();
                    networkErrors = null;
                }
                if (networkOutputs != null)
                {
                    networkOutputs.Dispose();
                    networkOutputs = null;
                }
            }
        }

        #endregion

    }

}