using UnityEngine;

public class CattleVisual : MonoBehaviour, ICattleVisual
{
    Renderer _renderer;

    void Awake()
    {
        if (TryGetComponent(out Renderer renderer))
        {
            _renderer = renderer;
        }
        else
        {
            enabled = false;
        }
    }

    public void Reset()
    {
        _renderer.material.color = Color.white;
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }
}