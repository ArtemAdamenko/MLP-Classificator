using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Neural
{
    public class InputLayer
    {
        private int _cntOfNeurons = 0;
        private double[] _input;

        private int _heightOfEllipse = 25;
        private int _widthOfEllipse = 50;
        Point[] _inputLinesRight;
        private SolidBrush _myBrush = new SolidBrush(Color.Blue);
        private Pen _myPen = new Pen(Color.Black);

        public InputLayer(double[] values)
        {
            this._cntOfNeurons = values.Length;
            this._input = new double[this._cntOfNeurons];

            for (int i = 0; i < this._cntOfNeurons; i++)
            {
                this._input[i] = values[i];
            }

            this._inputLinesRight = new Point[this._cntOfNeurons];
        }

        public void drawInputLayer(int x, Graphics gr)
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


                this._inputLinesRight[i].X = x + this._widthOfEllipse;
                this._inputLinesRight[i].Y = this._heightOfEllipse * i + (this._heightOfEllipse / 2);
            }
        }

        public Point[] getRightPoint()
        {
            return this._inputLinesRight;
        }
    }
}
