using UnityEngine;

public class RandomHerdFactory : IHerdFactory
{
    readonly float _minHue;
    readonly float _maxHue;
    readonly float _minSaturation;
    readonly float _maxSaturation;
    readonly float _minValue;
    readonly float _maxValue;

    public RandomHerdFactory(float minHue, float maxHue, float minSaturation, float maxSaturation, float minValue, float maxValue)
    {
        _minHue = minHue;
        _maxHue = maxHue;
        _minSaturation = minSaturation;
        _maxSaturation = maxSaturation;
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public Herd Create()
    {
        return new Herd(GetRandomColor());
    }

    private Color GetRandomColor()
    {
        return Random.ColorHSV(_minHue, _maxHue, _minSaturation, _maxSaturation, _minValue, _maxValue);
    }
}