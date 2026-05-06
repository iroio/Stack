using UnityEngine;

public class TitleColorInitializer : MonoBehaviour
{
    [SerializeField] CubeColorGenerator _cubeGen;
    [SerializeField] BGColorGenerator _bgGen;

    [SerializeField] CubeColorManager _cubeColorManager;
    [SerializeField] BGColorManager _bgColorManager;

    [SerializeField] Renderer _cubeRenderer; // 타이틀 큐브
    [SerializeField] Renderer _gradientCubeRenderer;

    void Start()
    {
        if (ColorState.Initialized) return;

        // 색 생성
        Color cubeColor = _cubeGen.GetNextColor();
        Color bgColor = _bgGen.GetNextColor();

        // 저장
        ColorState.cubeColor = cubeColor;
        ColorState.bgColor = bgColor;

        ColorState.cubeCount = _cubeGen.GetCount();
        ColorState.bgCount = _bgGen.GetCount();

        ColorState.Initialized = true;

        // 타이틀에도 적용
        _cubeColorManager.ApplyColor(_cubeRenderer, cubeColor);
        _bgColorManager.ApplyColor(bgColor);
        _cubeColorManager.ApplyColor(_gradientCubeRenderer, cubeColor);
    }
}