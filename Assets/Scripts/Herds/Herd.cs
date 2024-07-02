using System.Collections.Generic;
using UnityEngine;

public class Herd
{
    readonly Color _color;
    ICollection<Cattle> _cattleList;

    public Herd(Color color)
    {
        _color = color;
        _cattleList = new HashSet<Cattle>();
    }

    public Color GetColor()
    {
        return _color;
    }

    public void Assign(Cattle cattle)
    {
        _cattleList.Add(cattle);
    }

    public void Unassign(Cattle cattle)
    {
        _cattleList.Remove(cattle);
    }

    public void Clear()
    {
        _cattleList.Clear();
    }

    public void Update()
    {
        ICattleVisitor visitor = new CattleVisitor(_color);
        foreach(Cattle cattle in _cattleList)
        {
            cattle.Accept(visitor);
        }
    }
}