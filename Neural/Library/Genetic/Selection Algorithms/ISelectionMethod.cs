namespace Neural
{

    using System.Collections.Generic;


    public interface ISelectionMethod
    {

        void ApplySelection( List<IChromosome> chromosomes, int size );
    }
}