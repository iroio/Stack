using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreTmp;
    [SerializeField] TextMeshProUGUI _resultTmp;
    [SerializeField] Canvas _canvas;
    [SerializeField] BaseCubeEF _BC;

    public static UIManager _UM;

    bool _isLoading = false;
    bool _isGameOver = false;

    public void OnClickPlay()
    {
        if(_isLoading) return;

        _isLoading = true;

        GameManager._GM.ResetGame();

        ScenesManager.Instance.LoadScene("Game");
    }

    public void ChangeScore(int score)
    {
        _scoreTmp.text = score.ToString();
    }

    public void GameOverResult(int score, int highScore)
    {
        _canvas.gameObject.SetActive(true);
        _resultTmp.text = highScore.ToString();
        _isGameOver = true;
    }

    public void TitleAnim()
    {
        if (_BC != null)
            _BC.CubeUp();
    }

    public void Awake()
    {
        _UM = this;

        _isLoading = false;
        _isGameOver = false;

        if(_canvas != null)
            _canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_isLoading) return;

        if (_isGameOver)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                GameManager._GM.ResetGame();
                ScenesManager.Instance.LoadScene("Title");
            }
            return;
        }

        if (SceneManager.GetActiveScene().name == "Title")
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                OnClickPlay();
            }
        }
    }
}
