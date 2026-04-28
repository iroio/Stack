using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tmpUgui;

    public static UIManager Instance;

    bool _isLoading = false;

    public void OnClickPlay()
    {
        if(_isLoading) return;

        _isLoading = true;
        ScenesManager.Instance.LoadScene("Game");
    }

    public void ChangeScore(int score)
    {
        _tmpUgui.text = score.ToString();
    }

    public void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (_isLoading) return;

        if (SceneManager.GetActiveScene().name != "Title")
            return;

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClickPlay();
        }
    }
}
