namespace Neural
{

    public interface IFitnessFunction
    {

        double Evaluate( IChromosome chromosome );
    }
}