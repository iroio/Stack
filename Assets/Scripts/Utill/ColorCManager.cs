using UnityEngine;

public class ColorManager : MonoBehaviour
{
    // 색상 변경 값 #######################################
    [SerializeField] Gradient _gradient;
    [SerializeField] float _colorCycle = 100f;

    int _count = 0;

    // 큐브 색상 지정 #####################################
    public Color GetNextColor()
    {
        _count++;

        float t = (_count / _colorCycle) % 1f;
        t = Mathf.SmoothStep(0f, 1f, t);

        return _gradient.Evaluate(t);
    }
}
