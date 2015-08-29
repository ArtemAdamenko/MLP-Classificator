namespace Neural
{
    using System;

    using System.Collections.Generic;

    [Serializable]
    public abstract class Neuron
    {

        protected int inputsCount = 0;


        protected double[] weights = null;


        protected double output = 0;


        protected static ThreadSafeRandom rand = new ThreadSafeRandom( );


        protected static Range randRange = new Range( 0.0f, 1.0f );


        public static ThreadSafeRandom RandGenerator
        {
            get { return rand; }
            set
            {
                if ( value != null )
                {
                    rand = value;
                }
            }
        }


        public static Range RandRange
        {
            get { return randRange; }
            set { randRange = value; }
        }


        public int InputsCount
        {
            get { return inputsCount; }
        }


        public double Output
        {
            get { return output; }
        }


        public double[] Weights
        {
            get { return weights; }
        }


        protected Neuron( int inputs )
        {

            inputsCount = Math.Max( 1, inputs );
            weights = new double[inputsCount];

            Randomize( );
        }


        public virtual void Randomize( )
        {
            double d = randRange.Length;


            for ( int i = 0; i < inputsCount; i++ )
                weights[i] = rand.NextDouble( ) * d + randRange.Min;
        }


        public abstract double Compute( double[] input );
        public abstract double Compute(double[] input, Subnet subnet, int numberLayer, int numberNeuron, List<Record> MinMaxValues);
    }
}
