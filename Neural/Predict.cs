using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Neural
{
    public class Predict
    {
        static SolidBrush myBrush = new SolidBrush(Color.Blue);
        static SolidBrush offBrush = new SolidBrush(Color.Gray);
        private static Pen myPen = new Pen(Color.Black);
        private static Graphics formGraphics;

        private static int[] work = new int[50];
        private static double[,] __statist_i_h_wts = new double[50, 1];
        private static double[,] __statist_h_o_wts = new double[10, 50];
        private static double[] __statist_hidden_bias = new double[50];
        private static double[] __statist_max_input = new double[1];
        private static double[] __statist_min_input = new double[1];
        private static double[] __statist_max_target = new double[10];
        private static double[] __statist_min_target = new double[10];
        private static double[] __statist_output_bias = new double[10];
        private static double[] __statist_inputs = new double[1];
        private static double[] __statist_hidden = new double[50];
        private static double[] __statist_outputs = new double[10];

        public static void setWork(int[] working)
        {
            work = working;
        }

        public static int getWorkHiddenNeuron(int i)
        {
            return work[i];
        }

        public static double getValueHiddenNeuron(int i)
        {
            return __statist_hidden[i];
        }

        public static double getBiasHiddenNeuron(int i)
        {
            return __statist_hidden_bias[i];
        }

        public static double getOutputWeightHiddenNeuron(int i)
        {
            return __statist_h_o_wts[0, i];
        }

        public static double getInputWeightHiddenNeuron(int i)
        {
            return __statist_i_h_wts[i, 0];
        }

        public static int getCountInputNeurons()
        {
            return __statist_inputs.Length;
        }

        public static int getCountHiddenNeurons()
        {
            return __statist_hidden.Length;
        }

        public static int getCountOutputNeurons()
        {
            return __statist_outputs.Length;
        }

        private static void drawLines(Graphics gr, Pen myPen, Point[] layer1, Point[] layer2)
        {
            for (int i = 0; i < layer1.Length; i++)
            {
                for (int j = 0; j < layer2.Length; j++)
                {
                    gr.DrawLine(myPen, layer1[i], layer2[j]);
                }
            }
        }

        public static void drawLayers(Graphics gr)
        {
            formGraphics = gr;
            HiddenLayer[] hiddenLayers = new HiddenLayer[5];
            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                hiddenLayers[i] = new HiddenLayer((i+1)*2);
                hiddenLayers[i].drawHiddenLayer(200 * (i + 1), gr);
            }

            double[] test = new double[5] { 0.0, 0.0, 0.0, 0.0, 0.0 };
            InputLayer inputLayer = new InputLayer(test);
            inputLayer.drawInputLayer(0, gr);

            drawLines(formGraphics, myPen, inputLayer.getRightPoint(), hiddenLayers[0].getLeftPoints());

            for (int i = 0; i < hiddenLayers.Length - 1; i++)
            {
                drawLines(formGraphics, myPen, hiddenLayers[i].getRightPoints(), hiddenLayers[i+1].getLeftPoints());
            }

            OutputLayer output = new OutputLayer(5);
            output.drawOutputLayer(1200, gr);

            drawLines(formGraphics, myPen, hiddenLayers[hiddenLayers.Length - 1].getRightPoints(), output.getLeftPoints());
        }

        public static void init()
        {
            work[0] = 1;
            work[1] = 1;
            work[2] = 1;
            work[3] = 1;
            work[4] = 1;
            work[5] = 1;
            work[6] = 1;
            work[7] = 1;
            work[8] = 1;
            work[9] = 1;

            work[10] = 1;
            work[11] = 1;
            work[12] = 1;
            work[13] = 1;
            work[14] = 1;
            work[15] = 1;
            work[16] = 1;
            work[17] = 1;
            work[18] = 1;
            work[19] = 1;

            work[20] = 1;
            work[21] = 1;
            work[22] = 1;
            work[23] = 1;
            work[24] = 1;
            work[25] = 1;
            work[26] = 1;
            work[27] = 1;
            work[28] = 1;
            work[29] = 1;

            work[30] = 1;
            work[31] = 1;
            work[32] = 1;
            work[33] = 1;
            work[34] = 1;
            work[35] = 1;
            work[36] = 1;
            work[37] = 1;
            work[38] = 1;
            work[39] = 1;

            work[40] = 1;
            work[41] = 1;
            work[42] = 1;
            work[43] = 1;
            work[44] = 1;
            work[45] = 1;
            work[46] = 1;
            work[47] = 1;
            work[48] = 1;
            work[49] = 1;

            __statist_max_input[0] = 6.99100000000000e+003;
            __statist_min_input[0] = 4.50000000000000e+001;
            __statist_max_target[0] = 3.84453930212901e+000;
            __statist_min_target[0] = 1.65321251377534e+000;
            __statist_max_target[1] = 3.84453930212901e+000;
            __statist_min_target[1] = 1.65321251377534e+000;
            __statist_max_target[2] = 3.84453930212901e+000;
            __statist_min_target[2] = 1.65321251377534e+000;
            __statist_max_target[3] = 3.84453930212901e+000;
            __statist_min_target[3] = 1.65321251377534e+000;
            __statist_max_target[4] = 3.84453930212901e+000;
            __statist_min_target[4] = 1.65321251377534e+000;
            __statist_max_target[5] = 3.84453930212901e+000;
            __statist_min_target[5] = 1.65321251377534e+000;
            __statist_max_target[6] = 3.84453930212901e+000;
            __statist_min_target[6] = 1.65321251377534e+000;
            __statist_max_target[7] = 3.84453930212901e+000;
            __statist_min_target[7] = 1.65321251377534e+000;
            __statist_max_target[8] = 3.84453930212901e+000;
            __statist_min_target[8] = 1.65321251377534e+000;
            __statist_max_target[9] = 3.84453930212901e+000;
            __statist_min_target[9] = 1.65321251377534e+000;

            __statist_output_bias[0] = 1.70704376211404e+001;
            __statist_output_bias[1] = 1.70704376211404e+001;
            __statist_output_bias[2] = 1.70704376211404e+001;
            __statist_output_bias[3] = 1.70704376211404e+001;
            __statist_output_bias[4] = 1.70704376211404e+001;
            __statist_output_bias[5] = 1.70704376211404e+001;
            __statist_output_bias[6] = 1.70704376211404e+001;
            __statist_output_bias[7] = 1.70704376211404e+001;
            __statist_output_bias[8] = 1.70704376211404e+001;
            __statist_output_bias[9] = 1.70704376211404e+001;
            __statist_outputs[0] = -1.0e+307;
            __statist_outputs[1] = -1.0e+307;
            __statist_outputs[2] = -1.0e+307;
            __statist_outputs[3] = -1.0e+307;
            __statist_outputs[4] = -1.0e+307;
            __statist_outputs[5] = -1.0e+307;
            __statist_outputs[6] = -1.0e+307;
            __statist_outputs[7] = -1.0e+307;
            __statist_outputs[8] = -1.0e+307;
            __statist_outputs[9] = -1.0e+307;

            __statist_i_h_wts[0, 0] = 6.22462175104843e+001;
            __statist_i_h_wts[1, 0] = -7.68772513908550e-001;
            __statist_i_h_wts[2, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[3, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[4, 0] = 8.10156714643927e+000;
            __statist_i_h_wts[5, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[6, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[7, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[8, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[9, 0] = 8.10156714643927e+000;

            __statist_i_h_wts[10, 0] = 6.22462175104843e+001;
            __statist_i_h_wts[11, 0] = -7.68772513908550e-001;
            __statist_i_h_wts[12, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[13, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[14, 0] = 8.10156714643927e+000;
            __statist_i_h_wts[15, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[16, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[17, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[18, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[19, 0] = 8.10156714643927e+000;

            __statist_i_h_wts[20, 0] = 6.22462175104843e+001;
            __statist_i_h_wts[21, 0] = -7.68772513908550e-001;
            __statist_i_h_wts[22, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[23, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[24, 0] = 8.10156714643927e+000;
            __statist_i_h_wts[25, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[26, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[27, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[28, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[29, 0] = 8.10156714643927e+000;

            __statist_i_h_wts[30, 0] = 6.22462175104843e+001;
            __statist_i_h_wts[31, 0] = -7.68772513908550e-001;
            __statist_i_h_wts[32, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[33, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[34, 0] = 8.10156714643927e+000;
            __statist_i_h_wts[35, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[36, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[37, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[38, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[39, 0] = 8.10156714643927e+000;

            __statist_i_h_wts[40, 0] = 6.22462175104843e+001;
            __statist_i_h_wts[41, 0] = -7.68772513908550e-001;
            __statist_i_h_wts[42, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[43, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[44, 0] = 8.10156714643927e+000;
            __statist_i_h_wts[45, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[46, 0] = 2.57082653381536e+001;
            __statist_i_h_wts[47, 0] = 8.74155308185379e+000;
            __statist_i_h_wts[48, 0] = 7.70524202468283e+000;
            __statist_i_h_wts[49, 0] = 8.10156714643927e+000;

            __statist_h_o_wts[0, 0] = 2.65368351018916e+001;
            __statist_h_o_wts[0, 1] = -3.74320427104935e+001;
            __statist_h_o_wts[0, 2] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 3] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 4] = -2.35232457002890e+001;
            __statist_h_o_wts[0, 5] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 6] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 7] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 8] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 9] = -2.35232457002890e+001;

            __statist_h_o_wts[0, 10] = 2.65368351018916e+001;
            __statist_h_o_wts[0, 11] = -3.74320427104935e+001;
            __statist_h_o_wts[0, 12] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 13] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 14] = -2.35232457002890e+001;
            __statist_h_o_wts[0, 15] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 16] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 17] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 18] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 19] = -2.35232457002890e+001;

            __statist_h_o_wts[0, 20] = 2.65368351018916e+001;
            __statist_h_o_wts[0, 21] = -3.74320427104935e+001;
            __statist_h_o_wts[0, 22] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 23] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 24] = -2.35232457002890e+001;
            __statist_h_o_wts[0, 25] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 26] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 27] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 28] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 29] = -2.35232457002890e+001;

            __statist_h_o_wts[0, 30] = 2.65368351018916e+001;
            __statist_h_o_wts[0, 31] = -3.74320427104935e+001;
            __statist_h_o_wts[0, 32] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 33] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 34] = -2.35232457002890e+001;
            __statist_h_o_wts[0, 35] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 36] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 37] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 38] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 39] = -2.35232457002890e+001;

            __statist_h_o_wts[0, 40] = 2.65368351018916e+001;
            __statist_h_o_wts[0, 41] = -3.74320427104935e+001;
            __statist_h_o_wts[0, 42] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 43] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 44] = -2.35232457002890e+001;
            __statist_h_o_wts[0, 45] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 46] = 1.79704229093719e+001;
            __statist_h_o_wts[0, 47] = -3.39273294726821e+001;
            __statist_h_o_wts[0, 48] = 1.02773605778837e+001;
            __statist_h_o_wts[0, 49] = -2.35232457002890e+001;

            for (int i = 1; i < 10; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    __statist_h_o_wts[i, j] = __statist_h_o_wts[0, j];
                }
            }

            __statist_hidden_bias[0] = 4.53418422126028e+000;
            __statist_hidden_bias[1] = 4.97135995947592e+000;
            __statist_hidden_bias[2] = 5.56336018933705e+000;
            __statist_hidden_bias[3] = 2.81722814442101e+000;
            __statist_hidden_bias[4] = -1.42370300256486e+001;
            __statist_hidden_bias[5] = 7.70621434018933e+000;
            __statist_hidden_bias[6] = 7.70621434018933e+000;
            __statist_hidden_bias[7] = 5.56336018933705e+000;
            __statist_hidden_bias[8] = 2.81722814442101e+000;
            __statist_hidden_bias[9] = -1.42370300256486e+001;

            __statist_hidden_bias[10] = 4.53418422126028e+000;
            __statist_hidden_bias[11] = 4.97135995947592e+000;
            __statist_hidden_bias[12] = 5.56336018933705e+000;
            __statist_hidden_bias[13] = 2.81722814442101e+000;
            __statist_hidden_bias[14] = -1.42370300256486e+001;
            __statist_hidden_bias[15] = 7.70621434018933e+000;
            __statist_hidden_bias[16] = 7.70621434018933e+000;
            __statist_hidden_bias[17] = 5.56336018933705e+000;
            __statist_hidden_bias[18] = 2.81722814442101e+000;
            __statist_hidden_bias[19] = -1.42370300256486e+001;

            __statist_hidden_bias[20] = 4.53418422126028e+000;
            __statist_hidden_bias[21] = 4.97135995947592e+000;
            __statist_hidden_bias[22] = 5.56336018933705e+000;
            __statist_hidden_bias[23] = 2.81722814442101e+000;
            __statist_hidden_bias[24] = -1.42370300256486e+001;
            __statist_hidden_bias[25] = 7.70621434018933e+000;
            __statist_hidden_bias[26] = 7.70621434018933e+000;
            __statist_hidden_bias[27] = 5.56336018933705e+000;
            __statist_hidden_bias[28] = 2.81722814442101e+000;
            __statist_hidden_bias[29] = -1.42370300256486e+001;

            __statist_hidden_bias[30] = 4.53418422126028e+000;
            __statist_hidden_bias[31] = 4.97135995947592e+000;
            __statist_hidden_bias[32] = 5.56336018933705e+000;
            __statist_hidden_bias[33] = 2.81722814442101e+000;
            __statist_hidden_bias[34] = -1.42370300256486e+001;
            __statist_hidden_bias[35] = 7.70621434018933e+000;
            __statist_hidden_bias[36] = 7.70621434018933e+000;
            __statist_hidden_bias[37] = 5.56336018933705e+000;
            __statist_hidden_bias[38] = 2.81722814442101e+000;
            __statist_hidden_bias[39] = -1.42370300256486e+001;

            __statist_hidden_bias[40] = 4.53418422126028e+000;
            __statist_hidden_bias[41] = 4.97135995947592e+000;
            __statist_hidden_bias[42] = 5.56336018933705e+000;
            __statist_hidden_bias[43] = 2.81722814442101e+000;
            __statist_hidden_bias[44] = -1.42370300256486e+001;
            __statist_hidden_bias[45] = 7.70621434018933e+000;
            __statist_hidden_bias[46] = 7.70621434018933e+000;
            __statist_hidden_bias[47] = 5.56336018933705e+000;
            __statist_hidden_bias[48] = 2.81722814442101e+000;
            __statist_hidden_bias[49] = -1.42370300256486e+001;
        }

        public static double[] _MLP_1_6_1(double[] ContInputs)
        {
            int Cont_idx = 0;

            double N = ContInputs[Cont_idx++]; //Input Variable

            __statist_inputs[0] = N;

            double __statist_delta = 0;

            double __statist_maximum = 1;

            double __statist_minimum = 0;

            int __statist_ncont_inputs = 1;



            /*scale continuous inputs*/

            for (int __statist_i = 0; __statist_i < __statist_ncont_inputs; __statist_i++)
            {

                __statist_delta = (__statist_maximum - __statist_minimum) / (__statist_max_input[__statist_i] - __statist_min_input[__statist_i]);

                __statist_inputs[__statist_i] = __statist_minimum - __statist_delta * __statist_min_input[__statist_i] + __statist_delta * __statist_inputs[__statist_i];

            }



            int __statist_ninputs = 1;

            int __statist_nhidden = 50;



            /*Compute feed forward signals from Input layer to hidden layer*/

            for (int __statist_row = 0; __statist_row < __statist_nhidden; __statist_row++)
            {

                __statist_hidden[__statist_row] = 0.0;

                for (int __statist_col = 0; __statist_col < __statist_ninputs; __statist_col++)
                {

                    __statist_hidden[__statist_row] = __statist_hidden[__statist_row] + (__statist_i_h_wts[__statist_row, __statist_col] * __statist_inputs[__statist_col]);

                }

                __statist_hidden[__statist_row] = __statist_hidden[__statist_row] + __statist_hidden_bias[__statist_row];

            }



            for (int __statist_row = 0; __statist_row < __statist_nhidden; __statist_row++)
            {

                if (__statist_hidden[__statist_row] > 100.0)
                {

                    __statist_hidden[__statist_row] = 1.0;

                }

                else
                {

                    if (__statist_hidden[__statist_row] < -100.0)
                    {

                        __statist_hidden[__statist_row] = 0.0;

                    }

                    else
                    {

                        __statist_hidden[__statist_row] = 1.0 / (1.0 + Math.Exp(-__statist_hidden[__statist_row]));

                    }

                }

            }

            int __statist_noutputs = 10;

            /*Compute feed forward signals from hidden layer to output layer*/

            for (int __statist_row2 = 0; __statist_row2 < __statist_noutputs; __statist_row2++)
            {

                __statist_outputs[__statist_row2] = 0.0;

                for (int __statist_col2 = 0; __statist_col2 < __statist_nhidden; __statist_col2++)
                {
                    
                       /* int[,] temp = new int[1, 4];
                        temp[0, 0] = hiddenEllipses[__statist_col2, 0];
                        temp[0, 1] = hiddenEllipses[__statist_col2, 1];
                        temp[0, 2] = hiddenEllipses[__statist_col2, 2];
                        temp[0, 3] = hiddenEllipses[__statist_col2, 3];
                        Rectangle tempRec = new Rectangle(temp[0, 0], temp[0, 1], temp[0, 2], temp[0, 3]);*/
                        if (work[__statist_col2] != 0)
                        {
                        //    formGraphics.FillEllipse(myBrush, new Rectangle(temp[0, 0], temp[0, 1], temp[0, 2], temp[0, 3]));
                            __statist_outputs[__statist_row2] = __statist_outputs[__statist_row2] + (__statist_h_o_wts[__statist_row2, __statist_col2] * __statist_hidden[__statist_col2]);
                        }else{
                         //   formGraphics.FillEllipse(offBrush, new Rectangle(temp[0, 0], temp[0, 1], temp[0, 2], temp[0, 3]));
                        }
                        //Thread.Sleep(1000);
                        //formGraphics.FillEllipse(myBrush, new Rectangle(temp[0, 0], temp[0, 1], temp[0, 2], temp[0, 3]));
                        //formGraphics.DrawEllipse(myPen, tempRec);
                        
                    
                }

                __statist_outputs[__statist_row2] = __statist_outputs[__statist_row2] + __statist_output_bias[__statist_row2];

            }

            /*Unscale continuous targets*/

            __statist_delta = 0;

            for (int __statist_i = 0; __statist_i < __statist_noutputs; __statist_i++)
            {

                __statist_delta = (__statist_maximum - __statist_minimum) / (__statist_max_target[__statist_i] - __statist_min_target[__statist_i]);

                __statist_outputs[__statist_i] = (__statist_outputs[__statist_i] - __statist_minimum + __statist_delta * __statist_min_target[__statist_i]) / __statist_delta;

            }

            for (int __statist_ii = 0; __statist_ii < __statist_noutputs; __statist_ii++)
            {

                Console.WriteLine(" Prediction{0} = {1}", __statist_ii + 1, __statist_outputs[__statist_ii]);

            }
            return __statist_outputs;

        }

    }

}