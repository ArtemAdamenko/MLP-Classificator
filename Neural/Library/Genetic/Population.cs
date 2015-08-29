namespace Neural
{
    using System;
    using System.Collections.Generic;


    public class Population
    {
        private IFitnessFunction fitnessFunction;
        private ISelectionMethod selectionMethod;
        private List<IChromosome> population = new List<IChromosome>( );
        private int			size;
        private double		randomSelectionPortion = 0.0;
        private bool        autoShuffling = false;


        private double		crossoverRate	= 0.75;
        private double		mutationRate	= 0.10;


        private static ThreadSafeRandom rand = new ThreadSafeRandom( );

        //
        private double		fitnessMax = 0;
        private double		fitnessSum = 0;
        private double		fitnessAvg = 0;
        private IChromosome	bestChromosome = null;

 
        public double CrossoverRate
        {
            get { return crossoverRate; }
            set
            {
                crossoverRate = Math.Max( 0.1, Math.Min( 1.0, value ) );
            }
        }


        public double MutationRate
        {
            get { return mutationRate; }
            set
            {
                mutationRate = Math.Max( 0.1, Math.Min( 1.0, value ) );
            }
        }

        public double RandomSelectionPortion
        {
            get { return randomSelectionPortion; }
            set
            {
                randomSelectionPortion = Math.Max( 0, Math.Min( 0.9, value ) );
            }
        }


        public bool AutoShuffling
        {
            get { return autoShuffling; }
            set { autoShuffling = value; }
        }


        public ISelectionMethod SelectionMethod
        {
            get { return selectionMethod; }
            set { selectionMethod = value; }
        }


        public IFitnessFunction FitnessFunction
        {
            get { return fitnessFunction; }
            set
            {
                fitnessFunction = value;

                foreach ( IChromosome member in population )
                {
                    member.Evaluate( fitnessFunction );
                }

                FindBestChromosome( );
            }
        }


        public double FitnessMax
        {
            get { return fitnessMax; }
        }


        public double FitnessSum
        {
            get { return fitnessSum; }
        }


        public double FitnessAvg
        {
            get { return fitnessAvg; }
        }

 
        public IChromosome BestChromosome
        {
            get { return bestChromosome; }
        }


        public int Size
        {
            get { return size; }
        }


        public IChromosome this[int index]
        {
            get { return population[index]; }
        }


        public Population( int size,
                           IChromosome ancestor,
                           IFitnessFunction fitnessFunction,
                           ISelectionMethod selectionMethod )
        {
            if ( size < 2 )
                throw new ArgumentException( "Too small population's size was specified." );

            this.fitnessFunction = fitnessFunction;
            this.selectionMethod = selectionMethod;
            this.size = size;


            ancestor.Evaluate( fitnessFunction );
            population.Add( ancestor.Clone( ) );

            for ( int i = 1; i < size; i++ )
            {

                IChromosome c = ancestor.CreateNew( );

                c.Evaluate( fitnessFunction );

                population.Add( c );
            }
        }


        public void Regenerate( )
        {
            IChromosome ancestor = population[0];


            population.Clear( );

            for ( int i = 0; i < size; i++ )
            {

                IChromosome c = ancestor.CreateNew( );

                c.Evaluate( fitnessFunction );

                population.Add( c );
            }
        }

        public virtual void Crossover( )
        {
            for ( int i = 1; i < size; i += 2 )
            {

                if ( rand.NextDouble( ) <= crossoverRate )
                {

                    IChromosome c1 = population[i - 1].Clone( );
                    IChromosome c2 = population[i].Clone( );


                    c1.Crossover( c2 );


                    c1.Evaluate( fitnessFunction );
                    c2.Evaluate( fitnessFunction );


                    population.Add( c1 );
                    population.Add( c2 );
                }
            }
        }


        public virtual void Mutate( )
        {

            for ( int i = 0; i < size; i++ )
            {

                if ( rand.NextDouble( ) <= mutationRate )
                {

                    IChromosome c = population[i].Clone( );

                    c.Mutate( );

                    c.Evaluate( fitnessFunction );

                    population.Add( c );
                }
            }
        }


        public virtual void Selection( )
        {

            int randomAmount = (int) ( randomSelectionPortion * size );


            selectionMethod.ApplySelection( population, size - randomAmount );


            if ( randomAmount > 0 )
            {
                IChromosome ancestor = population[0];

                for ( int i = 0; i < randomAmount; i++ )
                {

                    IChromosome c = ancestor.CreateNew( );

                    c.Evaluate( fitnessFunction );

                    population.Add( c );
                }
            }

            FindBestChromosome( );
        }


        public void RunEpoch( )
        {
            Crossover( );
            Mutate( );
            Selection( );

            if ( autoShuffling )
                Shuffle( );
        }


        public void Shuffle( )
        {

            int size = population.Count;

            List<IChromosome> tempPopulation = population.GetRange( 0, size );

            population.Clear( );

            while ( size > 0 )
            {
                int i = rand.Next( size );

                population.Add( tempPopulation[i] );
                tempPopulation.RemoveAt( i );

                size--;
            }
        }


        public void AddChromosome( IChromosome chromosome )
        {
            chromosome.Evaluate( fitnessFunction );
            population.Add( chromosome );
        }


        public void Migrate( Population anotherPopulation, int numberOfMigrants, ISelectionMethod migrantsSelector )
        {
            int currentSize = this.size;
            int anotherSize = anotherPopulation.Size;


            List<IChromosome> currentCopy = new List<IChromosome>( );

            for ( int i = 0; i < currentSize; i++ )
            {
                currentCopy.Add( population[i].Clone( ) );
            }


            List<IChromosome> anotherCopy = new List<IChromosome>( );

            for ( int i = 0; i < anotherSize; i++ )
            {
                anotherCopy.Add( anotherPopulation.population[i].Clone( ) );
            }


            migrantsSelector.ApplySelection( currentCopy, numberOfMigrants );
            migrantsSelector.ApplySelection( anotherCopy, numberOfMigrants );


            population.Sort( );
            anotherPopulation.population.Sort( );


            population.RemoveRange( currentSize - numberOfMigrants, numberOfMigrants );
            anotherPopulation.population.RemoveRange( anotherSize - numberOfMigrants, numberOfMigrants );


            population.AddRange( anotherCopy );
            anotherPopulation.population.AddRange( currentCopy );


            FindBestChromosome( );
            anotherPopulation.FindBestChromosome( );
        }


        public void Resize( int newPopulationSize )
        {
            Resize( newPopulationSize, selectionMethod );
        }


        public void Resize( int newPopulationSize, ISelectionMethod membersSelector )
        {
            if ( newPopulationSize < 2 )
                throw new ArgumentException( "Too small new population's size was specified." );

            if ( newPopulationSize > size )
            {

                int toAdd = newPopulationSize - population.Count;

                for ( int i = 0; i < toAdd; i++ )
                {

                    IChromosome c = population[0].CreateNew( );

                    c.Evaluate( fitnessFunction );

                    population.Add( c );
                }
            }
            else
            {

                membersSelector.ApplySelection( population, newPopulationSize );
            }

            size = newPopulationSize;
        }


        private void FindBestChromosome( )
        {
            bestChromosome = population[0];
            fitnessMax = bestChromosome.Fitness;
            fitnessSum = fitnessMax;

            for ( int i = 1; i < size; i++ )
            {
                double fitness = population[i].Fitness;


                fitnessSum += fitness;


                if ( fitness > fitnessMax )
                {
                    fitnessMax = fitness;
                    bestChromosome = population[i];
                }
            }
            fitnessAvg = fitnessSum / size;
        }
    }
}