using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BaseCubeEF : MonoBehaviour
{
    [SerializeField] AnimationCurve _curve;

    [SerializeField] float _time = 1f;

    IEnumerator CubeUpRoutine(float targetY)
    {
        yield return null;

        transform.DOMoveY(targetY, _time).SetEase(_curve);
    }

    IEnumerator CubeDownRoutine(float targetY)
    {
        yield return null;

        transform.DOMoveY(targetY, _time).SetEase(_curve);
    }

    public void CubeUp()
    {
        Debug.Log("CubeUp 실행");
        // DoTween 중복실행 방지
        transform.DOKill();

        float targetY = 0f;

        transform.position = new Vector3(transform.position.x, targetY - 14f, transform.position.z);

        StartCoroutine(CubeUpRoutine(targetY));
    }

    public void CubeDown()
    {
        Debug.Log("CubeDown 실행");
        // DoTween 중복실행 방지
        transform.DOKill();

        float targetY = -14f;

        transform.position = new Vector3(transform.position.x, targetY + 14f, transform.position.z);

        StartCoroutine(CubeDownRoutine(targetY));
    }
}
