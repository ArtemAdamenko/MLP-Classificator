namespace Neural
{
   
    using System.Collections.Generic;


    public class EliteSelection : ISelectionMethod
    {

        public EliteSelection( ) { }


        public void ApplySelection( List<IChromosome> chromosomes, int size )
        {

            chromosomes.Sort( );


            chromosomes.RemoveRange( size, chromosomes.Count - size );
        }
    }
}
