using System.Collections.Generic;
using UnityEngine;

public class Herd
{
    readonly Color _color;
    CattleVisitor _visitor;
    ICollection<Cattle> _cattleList;

    public Herd(Color color)
    {
        _color = color;
        _cattleList = new HashSet<Cattle>();
        _visitor = new CattleVisitor(_color);
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
        foreach(Cattle cattle in _cattleList)
        {
            cattle.Reset();
        }

        _cattleList.Clear();
    }

    public void Update()
    {
        foreach(Cattle cattle in _cattleList)
        {
            cattle.Accept(_visitor);
        }
    }
}