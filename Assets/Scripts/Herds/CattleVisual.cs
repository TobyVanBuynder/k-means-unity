using UnityEngine;

public class CattleVisual : MonoBehaviour, ICattleVisual
{
    Renderer _renderer;

    void Awake()
    {
        if (TryGetComponent(out Renderer renderer))
        {
            _renderer = renderer;
            Reset();
        }
        else
        {
            enabled = false;
        }
    }

    public void Reset()
    {
        SetRingColor(Color.clear);
    }

    public void SetColor(Color color)
    {
        SetRingColor(color);
    }

    private void SetRingColor(Color color)
    {
        _renderer.material.SetColor("_RingColor", color);
    }
}