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

        ColorState.Reset();

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

    public void LoadTitle()
    {
        GameManager._GM.ResetGame();
        ScenesManager.Instance.LoadScene("Title");
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
        if (_isGameOver)
        {
            if (_isLoading) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _isLoading = true;

                if (_BC == null)
                    _BC = GameObject.Find("BaseCube")?.GetComponent<BaseCubeEF>();

                _BC.CubeDown();

                Invoke(nameof(LoadTitle), 1.2f);
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
