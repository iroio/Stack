using UnityEngine;

public class TitleColorInitializer : MonoBehaviour
{
    // =========================================================
    // 참조 설정
    // =========================================================
    [SerializeField] CubeColorGenerator _cubeGen; // 큐브 색 생성 스크립트 참조
    [SerializeField] BGColorGenerator _bgGen;       // 배경 색 생성 스크립트 참조

    // =========================================================
    // 색 적용 대상 설정
    // =========================================================
    [SerializeField] CubeColorManager _cubeColorManager; // 색 적용할 큐브 설정
    [SerializeField] BGColorManager _bgColorManager;       // 색 적용할 배경 설정

    // =========================================================
    // Renderer 설정
    // =========================================================
    // 기준 큐브 위아래 두 조각 중 실제 큐브가 쌓일 상단 큐브
    [SerializeField] Renderer _cubeRenderer;              // 타이틀 화면 큐브의 Renderer
    
    // 기준 큐브 위아래 두 조각 중 알파그라데이션을 적용한 하단 큐브
    [SerializeField] Renderer _gradientCubeRenderer; // 그라데이션 큐브 Renderer
    

    void Start()
    {
        // 색상 초기화가 끝났다면 Return
        if (ColorState.Initialized) return;

        // 큐브, 배경 색 생성
        Color cubeColor = _cubeGen.GetNextColor();
        Color bgColor = _bgGen.GetNextColor();

        // 생성된 색 저장
        ColorState.cubeColor = cubeColor;
        ColorState.bgColor = bgColor;

        // 현재 적용된 색 인덱스 저장
        ColorState.cubeCount = _cubeGen.GetCount();
        ColorState.bgCount = _bgGen.GetCount();

        // 초기화 완료 표시
        ColorState.Initialized = true;

        // 생성한 색상 적용
        _cubeColorManager.ApplyColor(_cubeRenderer, cubeColor);
        _bgColorManager.ApplyColor(bgColor);
        _cubeColorManager.ApplyColor(_gradientCubeRenderer, cubeColor);
    }
}