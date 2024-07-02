using UnityEngine;

public class Cattle : MonoBehaviour
{
    ICattleVisual _visual;

    void Awake()
    {
        _visual = GetComponentInChildren<CattleVisual>();

        if (_visual == null)
        {
            enabled = false;
        }
    }

    public void Reset()
    {
        _visual.Reset();
    }

    public void Accept(ICattleVisitor visitor)
    {
        visitor.VisitVisual(_visual);
    }
}
