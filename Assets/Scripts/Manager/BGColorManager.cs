using UnityEngine;

public class BGColorManager : MonoBehaviour
{
    [SerializeField] Renderer _bgRenderer;

    public void ApplyColor(Color color)
    {
        _bgRenderer.material.color = color;
    }
}