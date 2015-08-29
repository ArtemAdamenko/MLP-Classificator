namespace Neural
{
    using System;
    using System.Text;

    public class DoubleArrayChromosome : ChromosomeBase
    {

        protected IRandomNumberGenerator chromosomeGenerator;


        protected IRandomNumberGenerator mutationMultiplierGenerator;


        protected IRandomNumberGenerator mutationAdditionGenerator;


        protected static ThreadSafeRandom rand = new ThreadSafeRandom( );


        public const int MaxLength = 65536;
        

        private int length;


        protected double[] val = null;


        private double mutationBalancer = 0.5;
        private double crossoverBalancer = 0.5;


        public int Length
        {
            get { return length; }
        }


        public double[] Value
        {
            get { return val; }
        }


        public double MutationBalancer
        {
            get { return mutationBalancer; }
            set { mutationBalancer = Math.Max( 0.0, Math.Min( 1.0, value ) ); }
        }

        public double CrossoverBalancer
        {
            get { return crossoverBalancer; }
            set { crossoverBalancer = Math.Max( 0.0, Math.Min( 1.0, value ) ); }
        }


        public DoubleArrayChromosome(
            IRandomNumberGenerator chromosomeGenerator,
            IRandomNumberGenerator mutationMultiplierGenerator,
            IRandomNumberGenerator mutationAdditionGenerator,
            int length )
        {


            this.chromosomeGenerator = chromosomeGenerator;
            this.mutationMultiplierGenerator = mutationMultiplierGenerator;
            this.mutationAdditionGenerator = mutationAdditionGenerator;
            this.length = Math.Max( 2, Math.Min( MaxLength, length ) ); ;


            val = new double[length];


            Generate( );
        }


        public DoubleArrayChromosome(
            IRandomNumberGenerator chromosomeGenerator,
            IRandomNumberGenerator mutationMultiplierGenerator,
            IRandomNumberGenerator mutationAdditionGenerator,
            double[] values )
        {
            if ( ( values.Length < 2 ) || ( values.Length > MaxLength ) )
                throw new ArgumentOutOfRangeException( "Invalid length of values array." );


            this.chromosomeGenerator = chromosomeGenerator;
            this.mutationMultiplierGenerator = mutationMultiplierGenerator;
            this.mutationAdditionGenerator = mutationAdditionGenerator;
            this.length = values.Length;


            val = (double[]) values.Clone( );
        }

        public DoubleArrayChromosome( DoubleArrayChromosome source )
        {
            this.chromosomeGenerator = source.chromosomeGenerator;
            this.mutationMultiplierGenerator = source.mutationMultiplierGenerator;
            this.mutationAdditionGenerator = source.mutationAdditionGenerator;
            this.length  = source.length;
            this.fitness = source.fitness;
            this.mutationBalancer = source.mutationBalancer;
            this.crossoverBalancer = source.crossoverBalancer;

            val = (double[]) source.val.Clone( );
        }

        public override string ToString( )
        {
            StringBuilder sb = new StringBuilder( );

            sb.Append( val[0] );

            for ( int i = 1; i < length; i++ )
            {
                sb.Append( ' ' );
                sb.Append( val[i] );
            }

            return sb.ToString( );
        }


        public override void Generate( )
        {
            for ( int i = 0; i < length; i++ )
            {

                val[i] = chromosomeGenerator.Next( );
            }
        }


        public override IChromosome CreateNew( )
        {
            return new DoubleArrayChromosome( chromosomeGenerator, mutationMultiplierGenerator, mutationAdditionGenerator, length );
        }


        public override IChromosome Clone( )
        {
            return new DoubleArrayChromosome( this );
        }

        public override void Mutate( )
        {
            int mutationGene = rand.Next( length );

            if ( rand.NextDouble( ) < mutationBalancer )
            {
                val[mutationGene] *= mutationMultiplierGenerator.Next( );
            }
            else
            {
                val[mutationGene] += mutationAdditionGenerator.Next( );
            }
        }

       
        public override void Crossover( IChromosome pair )
        {
            DoubleArrayChromosome p = (DoubleArrayChromosome) pair;

            if ( ( p != null ) && ( p.length == length ) )
            {
                if ( rand.NextDouble( ) < crossoverBalancer )
                {
                    int crossOverPoint = rand.Next( length - 1 ) + 1;
                    int crossOverLength = length - crossOverPoint;
                    double[] temp = new double[crossOverLength];

                    Array.Copy( val, crossOverPoint, temp, 0, crossOverLength );

                    Array.Copy( p.val, crossOverPoint, val, crossOverPoint, crossOverLength );

                    Array.Copy( temp, 0, p.val, crossOverPoint, crossOverLength );
                }
                else
                {
                    double[] pairVal = p.val;

                    double factor = rand.NextDouble( );
                    if ( rand.Next( 2 ) == 0 )
                        factor = -factor;

                    for ( int i = 0; i < length; i++ )
                    {
                        double portion = ( val[i] - pairVal[i] ) * factor;

                        val[i] -= portion;
                        pairVal[i] += portion;
                    }
                }
            }
        }
    }
}
