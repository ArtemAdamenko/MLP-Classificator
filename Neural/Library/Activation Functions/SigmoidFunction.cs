namespace Neural
{
    using System;

    [Serializable]
    public class SigmoidFunction : IActivationFunction, ICloneable
    {

        private double alpha = 2;

        public double Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public SigmoidFunction( ) { }

        public SigmoidFunction( double alpha )
        {
            this.alpha = alpha;
        }

        public double Function( double x )
        {
            return ( 1 / ( 1 + Math.Exp( -alpha * x ) ) );
        }

        public double Derivative( double x )
        {
            double y = Function( x );

            return ( alpha * y * ( 1 - y ) );
        }

        public double Derivative2( double y )
        {
            return ( alpha * y * ( 1 - y ) );
        }

        public object Clone( )
        {
            return new SigmoidFunction( alpha );
        }
    }
}
