using UnityEngine;

public class GameManager : MonoBehaviour
{
    //UIManager _uiManager;
    public static GameManager Instance;

    int _score = 0;

    bool _isGameOver;
    bool _isPlaying;

    public bool IsPlaying => _isPlaying;
    public bool IsGameOver => _isGameOver;

    public void AddScore(int score)
    {
        _score += score;

        if(UIManager.Instance != null)
            UIManager.Instance.ChangeScore(_score);
    }

    public void StartGame()
    {
        _isPlaying = true;

        FindObjectOfType<CubeSpawnManager>().SpawnCube();
    }

    public void GameOver()
    {
        _isGameOver = true;


        // 게임오버 UI 보여주기
    }

    private void Awake()
    {
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