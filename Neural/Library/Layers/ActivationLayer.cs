namespace Neural
{
    using System;


    [Serializable]
    public class ActivationLayer : Layer
    {


        public ActivationLayer( int neuronsCount, int inputsCount, IActivationFunction function )
            : base( neuronsCount, inputsCount )
        {
            for ( int i = 0; i < neurons.Length; i++ )
                neurons[i] = new ActivationNeuron( inputsCount, function );
        }

        public void SetActivationFunction( IActivationFunction function )
        {
            for ( int i = 0; i < neurons.Length; i++ )
            {
                ( (ActivationNeuron) neurons[i] ).ActivationFunction = function;
            }
        }
    }
}
