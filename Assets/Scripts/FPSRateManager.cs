using UnityEngine;

public class FPSRateManager : MonoBehaviour
{
    public static FPSRateManager Instance;

    [SerializeField] int targetFPS = 60;
    [SerializeField] bool useVSync = false;

    void Awake()
    {
        QualitySettings.vSyncCount = useVSync ? 1 : 0;
        Application.targetFrameRate = targetFPS;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
