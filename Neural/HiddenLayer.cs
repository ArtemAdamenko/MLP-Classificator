using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Neural
{
    public 
        class HiddenLayer
    {
        private int _cntOfNeurons = 0; 

        private bool[] _hidden;
        private int[] _work;
        private static int _numberLayers = 0;
        private int _currentLayer = 0;

        private int _heightOfEllipse = 25;
        private int _widthOfEllipse = 50;
        Point[] _hiddenLinesLeft;
        Point[] _hiddenLinesRight;
        private SolidBrush _myBrush = new SolidBrush(Color.Blue);
        private Pen _myPen = new Pen(Color.Black);

        public int getWorkHiddenNeuron(int i)
        {
            return this._work[i];
        }

        public static int getNumberLayers()
        {
            return _numberLayers;
        }

        public static void RollbackNumberLayers()
        {
            _numberLayers = 0;
        }

        public int getCurrentLayer()
        {
            return this._currentLayer;
        }

        public void setOffNeuron(int i)
        {
            this._hidden[i] = false;
        }

        public void setOnNeuron(int i)
        {
            this._hidden[i] = true;
        }

        public HiddenLayer(int cntOfNeurons)
        {
            _numberLayers++;
            this._currentLayer = _numberLayers;
            this._cntOfNeurons = cntOfNeurons;
            this._hidden = new bool[cntOfNeurons];
            this._work = new int[cntOfNeurons];

            for (int i = 0; i < cntOfNeurons; i++)
            {
                this._hidden[i] = true;
            }

            this._hiddenLinesLeft = new Point[cntOfNeurons];
            this._hiddenLinesRight = new Point[cntOfNeurons];
        }

        public void drawHiddenLayer(int x, Graphics gr)
        {
            for (int i = 0; i < this._cntOfNeurons; i++)
            {
                if (this._hidden[i] == false)
                    this._myBrush.Color = Color.Gray;
                else
                    this._myBrush.Color = Color.Blue;
                Rectangle ellipse = new Rectangle(x, this._heightOfEllipse * i, this._widthOfEllipse, this._heightOfEllipse);
                gr.FillEllipse(this._myBrush, ellipse);
                gr.DrawEllipse(this._myPen, ellipse);
                gr.DrawString(this._currentLayer.ToString() + "-" + i.ToString(), 
                                        new Font("Arial", 7),
                                        new SolidBrush(Color.Black),
                                        new Point(x + 20, this._heightOfEllipse * i + 7));

                this._hiddenLinesLeft[i].X = x;
                this._hiddenLinesLeft[i].Y = this._heightOfEllipse * i + (this._heightOfEllipse / 2);

                this._hiddenLinesRight[i].X = x + this._widthOfEllipse;
                this._hiddenLinesRight[i].Y = this._heightOfEllipse * i + (this._heightOfEllipse / 2);
            }
        }

        public Point[] getLeftPoints() 
        {
            return this._hiddenLinesLeft;
        }

        public Point[] getRightPoints()
        {
            return this._hiddenLinesRight;
        }

        public int getCountNeurons()
        {
            return this._cntOfNeurons;
        }
    }
}
