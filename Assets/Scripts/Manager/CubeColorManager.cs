using UnityEngine;

public class CubeColorManager : MonoBehaviour
{
    // =========================================================
    // MPB
    // =========================================================
    MaterialPropertyBlock _mpb;

    void Awake()
    {
        _mpb = new MaterialPropertyBlock();
    }

    public void ApplyColor(Renderer renderer, Color color)
    {
        // 현재 renderer가 가지고있는 MPB값 가져오기
        renderer.GetPropertyBlock(_mpb);
        // "_BaseColor" 에 color값 적용
        _mpb.SetColor("_BaseColor", color);
        // 현재 renderer에 MPB값 적용
        renderer.SetPropertyBlock(_mpb);
    }
}