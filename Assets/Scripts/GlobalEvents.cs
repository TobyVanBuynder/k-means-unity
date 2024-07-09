using System;

public static class GlobalEvents
{
    public static Action<int> UpdateNumHerds;
    public static Action<int> UpdateNumCattle;

    public static Action Rescatter;
    public static Action RunKmeans;

    public static Action BeforeKmeans;
    public static Action<KMeans.Stats, string> AfterKmeans;
    public static Action<KMeans.Stats, string, int> KmeansTimeTaken;
}