using UnityEngine;

public class CubeColorManager : MonoBehaviour
{
    MaterialPropertyBlock _mpb;

    void Awake()
    {
        _mpb = new MaterialPropertyBlock();
    }

    public void ApplyColor(Renderer renderer, Color color)
    {
        renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor("_BaseColor", color);
        renderer.SetPropertyBlock(_mpb);
    }
}