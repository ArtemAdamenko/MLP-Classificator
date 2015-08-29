namespace Neural
{


    public abstract class ChromosomeBase : IChromosome
    {

        protected double fitness = 0;


        public double Fitness
        {
            get { return fitness; }
        }


        public abstract void Generate( );


        public abstract IChromosome CreateNew( );


        public abstract IChromosome Clone( );


        public abstract void Mutate( );

 
        public abstract void Crossover( IChromosome pair );


        public void Evaluate( IFitnessFunction function )
        {
            fitness = function.Evaluate( this );
        }


        public int CompareTo( object o )
        {
            double f = ( (ChromosomeBase) o ).fitness;

            return ( fitness == f ) ? 0 : ( fitness < f ) ? 1 : -1;
        }
    }
}
