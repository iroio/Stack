using UnityEngine;

public class CubeColorGenerator : MonoBehaviour
{
    // 색상 변경 값 #######################################
    [SerializeField] Gradient _gradient;
    [SerializeField] float _colorCycle = 100f;

    int _count = 0;

    public void SetCount(int value)
    {
        _count = value;
    }

    public int GetCount()
    {
        return _count;
    }

    // 큐브 색상 지정 #####################################
    public Color GetNextColor()
    {
        _count++;

        float t = (_count / _colorCycle) % 1f;
        t = Mathf.SmoothStep(0f, 1f, t);

        return _gradient.Evaluate(t);
    }
}
