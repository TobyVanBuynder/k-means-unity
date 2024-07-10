public class KMeans2DStrategyFactory : IKMeansStrategyFactory
{
    public IKMeansStrategy Create(KMeansType type)
    {
        switch(type)
        {
            case KMeansType.Naive:
                return new KMeansNaive2DStrategy();
            
            case KMeansType.PlusPlus:
                return new KMeansPlusPlus2DStrategy();
            
            default:
                return new KMeansNullStrategy();
        }
    }
}