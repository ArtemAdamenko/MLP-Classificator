namespace Neural
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ActivationNeuron : Neuron
    {

        private List<Record> relationsValues;
        public List<Record> RelationsValues
        {
            get { return relationsValues; }
        }

        protected double threshold = 0.0;


        protected IActivationFunction function = null;


        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }


        public IActivationFunction ActivationFunction
        {
            get { return function; }
            set { function = value; }
        }


        public ActivationNeuron( int inputs, IActivationFunction function )
            : base( inputs )
        {
            this.function = function;
        }


        public override void Randomize( )
        {

            base.Randomize( );

            threshold = rand.NextDouble( ) * ( randRange.Length ) + randRange.Min;
        }


        public override double Compute( double[] input )
        {
            this.relationsValues = new List<Record>();

            if ( input.Length != inputsCount )
                throw new ArgumentException( "Wrong length of the input vector." );


            double sum = 0.0;


            for ( int i = 0; i < weights.Length; i++ )
            {
                sum += weights[i] * input[i];
                Record elem = new Record();
                elem.numberWeight = i;
                elem.value = weights[i] * input[i];
                this.relationsValues.Add(elem);
            }
            sum += threshold;


            double output = function.Function( sum );

            this.output = output;

            return output;
        }

        public override double Compute(double[] input, Subnet subnet, int numberLayer, int numberNeuron, List<Record> MinMaxValues)
        {
            Random rnd = new Random();

            if (input.Length != inputsCount)
                throw new ArgumentException("Wrong length of the input vector.");


            double sum = 0.0;

            for (int i = 0; i < weights.Length; i++)
            {
                if (subnet.outputAssosiated.Contains(numberLayer.ToString() + ":" + numberNeuron.ToString() + ":" + i.ToString()))
                {
                    double min = 0.0;
                    double max = 0.0;
                    foreach (Record elem in MinMaxValues)
                    {
                        if ((elem.numberLayer == numberLayer) && (elem.numberNeuron == numberNeuron) && (elem.numberWeight == i))
                        {
                            min = elem.min;
                            max = elem.max;
                        }
                    }
                    //double normValue = (((subnet.res[0] - (-1)) * (max - min)) / (1 - (-1))) + min;
                    //sum += (weights[i] * normValue ) * input[i];
                    sum += weights[i] + input[i];
                    subnet.res.RemoveAt(0);
                }
                else sum += weights[i] * input[i];
            }


            sum += threshold;


            double output = function.Function(sum);
            this.output = output;

            return output;


        }

    }
}
