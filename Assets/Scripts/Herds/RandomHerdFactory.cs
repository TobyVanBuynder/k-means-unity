using UnityEngine;

public class RandomHerdFactory : IHerdFactory
{
    readonly float _minHue;
    readonly float _maxHue;

    public RandomHerdFactory(float minHue, float maxHue)
    {
        _minHue = minHue;
        _maxHue = maxHue;
    }

    public Herd Create()
    {
        return new Herd(GetRandomColor());
    }

    private Color GetRandomColor()
    {
        return Random.ColorHSV(_minHue, _maxHue, 1f, 1f, 1f, 1f, 1f, 1f);
    }
}