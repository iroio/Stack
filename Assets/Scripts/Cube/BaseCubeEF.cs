using UnityEngine;
using DG.Tweening;

public class BaseCubeEF : MonoBehaviour
{
    [SerializeField] AnimationCurve _curve;

    [SerializeField] float _time = 1f;

    public void CubeUp()
    {
        // DoTween 중복실행 방지
        transform.DOKill();

        float targetY = 0f;

        transform.position = new Vector3(transform.position.x, targetY - 14f, transform.position.z);

        transform.DOMoveY(targetY, _time).SetEase(_curve);
    }
}
