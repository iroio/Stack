using UnityEngine;

public class BGColorManager : MonoBehaviour
{
    [SerializeField] Renderer _bgRenderer;

    public void ApplyColor(Color color)
    {
        // 배경은 수가 늘어나거나 Material을 여러개를 사용하지 않음
        // 단일로 지정해도 성능문제 발생하지 않음
        // _bgRenderer에 직접 색상 적용
        _bgRenderer.material.color = color;
    }
}