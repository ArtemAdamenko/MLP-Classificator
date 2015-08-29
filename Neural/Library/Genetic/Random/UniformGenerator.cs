namespace Neural
{
    
    public class UniformGenerator : IRandomNumberGenerator
    {
        private UniformOneGenerator rand = null;

        private float min;
        private float length;


        public float Mean
        {
            get { return ( min + min + length ) / 2; }
        }


        public float Variance
        {
            get { return length * length / 12; }
        }


        public Range Range
        {
            get { return new Range( min, min + length ); }
        }


        public UniformGenerator( Range range ) :
            this( range, 0 )
        {
        }

 
        public UniformGenerator( Range range, int seed )
        {
            rand = new UniformOneGenerator( seed );

            min     = range.Min;
            length  = range.Length;
        }

        public float Next( )
        {
            return (float) rand.Next( ) * length + min;
        }

 
        public void SetSeed( int seed )
        {
            rand = new UniformOneGenerator( seed );
        }
    }
}
