
namespace Neural
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public abstract class Layer
    {
        protected List<Record> relationsValues;
        public List<Record> RelationsValues
        {
            get { return relationsValues; }
        }

        protected int inputsCount = 0;


        protected int neuronsCount = 0;


        protected Neuron[] neurons;


        protected double[] output;


        public int InputsCount
        {
            get { return inputsCount; }
        }


        public Neuron[] Neurons
        {
            get { return neurons; }
        }

        public double[] Output
        {
            get { return output; }
        }

        protected Layer( int neuronsCount, int inputsCount )
        {
            this.inputsCount = Math.Max( 1, inputsCount );
            this.neuronsCount = Math.Max( 1, neuronsCount );

            neurons = new Neuron[this.neuronsCount];
        }

      
        public virtual double[] Compute( double[] input )
        {
            relationsValues = new List<Record>();

            double[] output = new double[neuronsCount];


            for (int i = 0; i < neurons.Length; i++)
            {
                output[i] = neurons[i].Compute(input);

                List<Record> weightsOfNeuron = (neurons[i] as ActivationNeuron).RelationsValues;
                foreach (Record weight in weightsOfNeuron)
                {
                    weight.numberNeuron = i;
                }
                relationsValues.AddRange(weightsOfNeuron);
            }

            this.output = output;

            return output;
        }

        public virtual double[] Compute(double[] input, Subnet subnet, int numberLayer, List<Record> MinMaxValues)
        {

            double[] output = new double[neuronsCount];


            for (int i = 0; i < neurons.Length; i++)
            {
                output[i] = neurons[i].Compute(input, subnet, numberLayer, i, MinMaxValues);
            }


            this.output = output;
            return output;
        }


        public virtual void Randomize( )
        {
            foreach ( Neuron neuron in neurons )
                neuron.Randomize( );
        }
    }
}
