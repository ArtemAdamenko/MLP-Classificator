namespace Neural{
    using System;


    public class ExponentialGenerator : IRandomNumberGenerator
    {
        private UniformOneGenerator rand = null;

        private float rate = 0;

        public float Rate
        {
            get { return rate; }
        }

        public float Mean
        {
            get { return 1.0f / rate; }
        }


        public float Variance
        {
            get { return 1f / ( rate * rate ); }
        }
        

        public ExponentialGenerator( float rate ) :
            this( rate, 0 )
        {
        }


        public ExponentialGenerator( float rate, int seed )
        {

            if ( rate <= 0 )
                throw new ArgumentException( "Rate value should be greater than zero." );

            this.rand = new UniformOneGenerator( seed );
            this.rate = rate;
        }


        public float Next( )
        {
            return - (float) Math.Log( rand.Next( ) ) / rate;
        }


        public void SetSeed( int seed )
        {
            rand = new UniformOneGenerator( seed );
        }
    }
}
