using UnityEngine;

public class CattleVisitor : ICattleVisitor
{
    Color _color;

    public CattleVisitor(Color color)
    {
        _color = color;
    }

    public void VisitVisual(ICattleVisual visual)
    {
        visual.SetColor(_color);
    }
}