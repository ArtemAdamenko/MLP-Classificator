using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Neural
{
    public class OutputLayer
    {
        private int _cntOfNeurons = 0;
        private double[] _output;
        private double[] _bias;

        private int _heightOfEllipse = 25;
        private int _widthOfEllipse = 50;
        Point[] _outputLinesLeft;
        private SolidBrush _myBrush = new SolidBrush(Color.Blue);
        private Pen _myPen = new Pen(Color.Black);

        public OutputLayer(int cntOfNeurons)
        {
            this._cntOfNeurons = cntOfNeurons;
            this._output = new double[this._cntOfNeurons];
            this._bias = new double[this._cntOfNeurons];

            for (int i = 0; i < this._cntOfNeurons; i++)
            {
                this._output[i] = 0.0;
                this._bias[i] = 0.0;
            }

            this._outputLinesLeft = new Point[this._cntOfNeurons];
        }

        public void drawOutputLayer(int x, Graphics gr)
        {
            for (int i = 0; i < this._cntOfNeurons; i++)
            {
                Rectangle ellipse = new Rectangle(x, this._heightOfEllipse * i, this._widthOfEllipse, this._heightOfEllipse);
                gr.FillEllipse(this._myBrush, ellipse);
                gr.DrawEllipse(this._myPen, ellipse);
                gr.DrawString(i.ToString(),
                                        new Font("Arial", 7),
                                        new SolidBrush(Color.Black),
                                        new Point(x + 20, this._heightOfEllipse * i + 7));


                this._outputLinesLeft[i].X = x;
                this._outputLinesLeft[i].Y = this._heightOfEllipse * i + (this._heightOfEllipse / 2);
            }
        }

        public Point[] getLeftPoints()
        {
            return this._outputLinesLeft;
        }
    }
}
