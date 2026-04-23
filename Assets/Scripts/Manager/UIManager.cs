using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public void OnClickPlay()
    {
        ScenesManager.Instance.LoadScene("Game");
    }
}
