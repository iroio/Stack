using UnityEngine;

public class BGColorSelector : MonoBehaviour
{
    MaterialPropertyBlock _mpb;
    Renderer[] _renderers;

    void Awake()
    {
        _mpb = new MaterialPropertyBlock();

        _renderers = GetComponentsInChildren<Renderer>();
    }

    public void ApplyColor(Color color)
    {
        if (_renderers == null || _renderers.Length == 0) return;

        for (int i = 0; i < _renderers.Length; i++)
        {
            var r = _renderers[i];

            r.GetPropertyBlock(_mpb);
            _mpb.SetColor("_BaseColor", color);
            r.SetPropertyBlock(_mpb);
        }
    }
}