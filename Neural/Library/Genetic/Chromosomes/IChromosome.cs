namespace Neural
{
    using System;

    public interface IChromosome : IComparable
    {

        double Fitness { get; }


        void Generate( );


        IChromosome CreateNew( );


        IChromosome Clone( );


        void Mutate( );

 
        void Crossover( IChromosome pair );


        void Evaluate( IFitnessFunction function );
    }
}
