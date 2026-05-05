using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class BaseCubeEF : MonoBehaviour
{
    [SerializeField] AnimationCurve _curve;
    [SerializeField] float _time = 1f;
    [SerializeField] float _moveOffset = 14f; // 이동 거리

    void MoveTo(float targetY, float startOffset)
    {
        transform.DOKill();

        // 시작 위치 설정
        Vector3 pos = transform.position;
        pos.y = targetY + startOffset;
        transform.position = pos;

        // 목표 위치로 이동
        transform.DOMoveY(targetY, _time)
                 .SetEase(_curve);
    }

    public void CubeUp()
    {
        Debug.Log("CubeUp 실행");

        MoveTo(0f, -_moveOffset); // 아래에서 올라옴
    }

    public void CubeDown()
    {
        Debug.Log("CubeDown 실행");

        MoveTo(-_moveOffset, +_moveOffset); // 위에서 내려감
    }

    bool IsTitleScene()
    {
        return SceneManager.GetActiveScene().name == "Title";
    }

    void Awake()
    {
        if (IsTitleScene())
        {
            Vector3 pos = transform.position;
            pos.y -= _moveOffset;
            transform.position = pos;
        }
    }

    void Start()
    {
        if (IsTitleScene())
        {
            CubeUp();
        }
    }
}
