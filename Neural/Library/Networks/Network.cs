namespace Neural
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Collections.Generic;

    [Serializable]
    public abstract class Network
    {
        protected List<Record> relationsValues;
        public List<Record> RelationsValues
        {
            get { return relationsValues; }
        }

        protected int inputsCount;


        protected int layersCount;


        protected Layer[] layers;


        protected double[] output;


        public int InputsCount
        {
            get { return inputsCount; }
        }


        public Layer[] Layers
        {
            get { return layers; }
        }


        public double[] Output
        {
            get { return output; }
        }

        protected Network( int inputsCount, int layersCount )
        {
            this.inputsCount = Math.Max( 1, inputsCount );
            this.layersCount = Math.Max( 1, layersCount );

            this.layers = new Layer[this.layersCount];
        }


        public virtual double[] Compute( double[] input )
        {
            this.relationsValues = new List<Record>();

            double[] output = input;


            for ( int i = 0; i < layers.Length; i++ )
            {
                output = layers[i].Compute( output );

                List<Record> neuronsOfLayer = layers[i].RelationsValues;

                foreach (Record neuron in neuronsOfLayer)
                {
                    neuron.numberLayer = i;
                }

                this.relationsValues.AddRange(neuronsOfLayer);
            }


            this.output = output;

            return output;
        }

        private double[] getWeights(List<String> requireWeights, double[] input)
        {
            double[] weights = new double[requireWeights.Count];
            int i = 0;
            foreach (String item in requireWeights)
            {
                String[] elem = item.Split(':');

                int layer = Int32.Parse(elem[0]);
                int neuron = Int32.Parse(elem[1]);
                int weight = Int32.Parse(elem[2]);

                weights[i] = layers[layer].Neurons[neuron].Weights[weight];
                if (layer >= 1)
                    weights[i] *= layers[layer - 1].Neurons[weight].Output;
                else
                    weights[i] *= input[weight];

                i++;

            }

            return weights;
        }




        public virtual double[] Compute(double[] input, Subnet subnet, List<Record> MinMaxValues)
        {
            List<String> inputs = subnet.inputAssosiated;
            double[] weights = this.getWeights(inputs, input);
            subnet.Compute(weights);

            double[] output = input;


            for (int i = 0; i < layers.Length; i++)
            {
                output = layers[i].Compute(output, subnet, i, MinMaxValues);
            }


            this.output = output;

            return output;
        }


        public virtual void Randomize( )
        {
            foreach ( Layer layer in layers )
            {
                layer.Randomize( );
            }
        }

        public void Save( string fileName )
        {
            FileStream stream = new FileStream( fileName, FileMode.Create, FileAccess.Write, FileShare.None );
            Save( stream );
            stream.Close( );
        }

        public void Save( Stream stream )
        {
            IFormatter formatter = new BinaryFormatter( );
            formatter.Serialize( stream, this );
        }


        public static Network Load( string fileName )
        {
            FileStream stream = new FileStream( fileName, FileMode.Open, FileAccess.Read, FileShare.Read );
            Network network = Load( stream );
            stream.Close( );

            return network;
        }


        public static Network Load( Stream stream )
        {
            IFormatter formatter = new BinaryFormatter( );
            Network network = (Network) formatter.Deserialize( stream );
            return network;
        }
    }
}
