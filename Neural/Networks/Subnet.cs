using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Neural
{
    public class Subnet
    {
        private Network network;
        public Hashtable inputAssosiated;
        public Hashtable outputAssosiated;
        private List<double> inputValues = new List<double>();
        public double[] res;
        public Boolean inputAll = false;
        //public Boolean outputAll = false;

        public Subnet(int input, int[] hidden) 
        {

            this.network = new ActivationNetwork(new SigmoidFunction(), input, hidden);

        }

        public Subnet(Network network)
        {

            this.network = network;

        }

        public double[] Compute()
        {
            res = this.network.Compute(this.inputValues.ToArray());
            inputAll = false;
            return res;
        }

        public void setInputValue(double input)
        {
            this.inputValues.Add(input);
            if (inputValues.Count == network.InputsCount)
                inputAll = true;
        }

        public int getCurrentInputValuesCount()
        {
            return this.inputValues.Count;
        }


        public Network Network 
        {
            get { return this.network; }
        }

        public Hashtable InputAssosiacted
        {
            get { return this.inputAssosiated; }
        }

        public Hashtable OutputAssosiacted
        {
            get { return this.outputAssosiated; }
        }

        public void setInputAssosiated(Hashtable inputHash)
        {
            this.inputAssosiated = inputHash;
        }

        public void setOutputAssosiated(Hashtable outputHash)
        {
            this.outputAssosiated = outputHash;
        }

        public List<int> getParentInputLayers()
        {
            List<int> layers = new List<int>();

            foreach (DictionaryEntry entry in inputAssosiated)
            {
                layers.Add(Int32.Parse(entry.Value.ToString().Split(':')[0]));
            }
            return layers.Distinct().ToList();
        }

        public List<int> getParentInputNeurons()
        {
            List<int> neurons = new List<int>();

            foreach (DictionaryEntry entry in inputAssosiated)
            {
                neurons.Add(Int32.Parse(((String[])entry.Value)[0].Split(':')[1]));
            }
            return neurons.Distinct().ToList();
        }

        public List<int> getParentOutputLayers()
        {
            List<int> layers = new List<int>();

            foreach (DictionaryEntry entry in outputAssosiated)
            {
                layers.Add(Int32.Parse((entry.Value.ToString().Split(':')[0])));
            }
            return layers.Distinct().ToList();
        }

        public List<int> getParentOutputNeurons()
        {
            List<int> neurons = new List<int>();

            foreach (DictionaryEntry entry in outputAssosiated)
            {
                neurons.Add(Int32.Parse(((String[])entry.Value)[0].Split(':')[1]));
            }
            return neurons.Distinct().ToList();
        }

        public List<int> getChildInputLayers()
        {
            List<int> layers = new List<int>();

            foreach (DictionaryEntry entry in inputAssosiated)
            {
                layers.Add(Int32.Parse(((String[])entry.Value)[1].Split(':')[0]));
            }
            return layers.Distinct().ToList();
        }

        public List<int> getChildInputNeurons()
        {
            List<int> neurons = new List<int>();

            foreach (DictionaryEntry entry in inputAssosiated)
            {
                neurons.Add(Int32.Parse(((String[])entry.Value)[1].Split(':')[1]));
            }
            return neurons.Distinct().ToList();
        }

        public List<int> getChildOutputLayers()
        {
            List<int> layers = new List<int>();

            foreach (DictionaryEntry entry in outputAssosiated)
            {
                layers.Add(Int32.Parse(((String[])entry.Value)[1].Split(':')[0]));
            }
            return layers.Distinct().ToList();
        }

        public List<int> getChildOutputNeurons()
        {
            List<int> neurons = new List<int>();

            foreach (DictionaryEntry entry in outputAssosiated)
            {
                neurons.Add(Int32.Parse(((String[])entry.Value)[1].Split(':')[1]));
            }
            return neurons.Distinct().ToList();
        }
    }
}
