
namespace Neural
{
    using System;

    [Serializable]
    public class ActivationNetwork : Network
    {

        public ActivationNetwork( IActivationFunction function, int inputsCount, params int[] neuronsCount )
            : base( inputsCount, neuronsCount.Length )
        {

            for ( int i = 0; i < layers.Length; i++ )
            {
                layers[i] = new ActivationLayer(

                    neuronsCount[i],

                    ( i == 0 ) ? inputsCount : neuronsCount[i - 1],

                    function );
            }
        }

        public void SetActivationFunction( IActivationFunction function )
        {
            for ( int i = 0; i < layers.Length; i++ )
            {
                ( (ActivationLayer) layers[i] ).SetActivationFunction( function );
            }
        }
    }
}
