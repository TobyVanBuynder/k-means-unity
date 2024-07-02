using System;

public static class GlobalEvents
{
    public static Action<int> UpdateNumHerds;
    public static Action<int> UpdateNumCattle;

    public static Action Rescatter;
    public static Action RunKmeans;
}