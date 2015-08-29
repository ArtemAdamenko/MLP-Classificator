namespace Neural
{
  
    public interface ISupervisedLearning
    {
       
        double Run( double[] input, double[] output );

        
        double RunEpoch( double[][] input, double[][] output );
    }
}
