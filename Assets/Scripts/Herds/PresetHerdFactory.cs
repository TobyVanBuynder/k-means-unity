using System.Collections.Generic;
using UnityEngine;

public class PresetHerdFactory : IHerdFactory
{
    private readonly Color[] _colors;
    private HashSet<int> _chosenColorIndices;

    public PresetHerdFactory()
    {
        _colors = new Color[]
        {
            Color.green,
            Color.red,
            Color.blue,
            Color.cyan,
            Color.magenta,
            Color.yellow,
            Utils.Colors.purple,
            Utils.Colors.orange,
            Utils.Colors.lime,
            Utils.Colors.pink,
            Utils.Colors.brown,
            Utils.Colors.olive
        };

        _chosenColorIndices = new HashSet<int>(_colors.Length);
    }

    public PresetHerdFactory(Color[] colors)
    {
        _colors = colors;
        _chosenColorIndices = new HashSet<int>(_colors.Length);
    }

    public Herd Create()
    {
        return new Herd(GetNewColor());
    }

    private Color GetNewColor()
    {
        int maxIterations = _colors.Length * 2;
        int numIterations = 0;

        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, _colors.Length);
            numIterations++;
        }
        while (_chosenColorIndices.Contains(randomIndex) && numIterations < maxIterations);

        _chosenColorIndices.Add(randomIndex);

        return _colors[randomIndex];
    }
}