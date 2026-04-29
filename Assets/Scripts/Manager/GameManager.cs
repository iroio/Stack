using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _GM;

    int _score = 0;
    int _highScore;

    bool _isGameOver;
    bool _isPlaying;

    public bool IsPlaying => _isPlaying;
    public bool IsGameOver => _isGameOver;

    public void AddScore(int score)
    {
        _score += score;

        if(UIManager._UM != null)
            UIManager._UM.ChangeScore(_score);
    }

    public void StartGame()
    {
        _isPlaying = true;
        _isGameOver = false;
        _score = 0;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        _isGameOver = true;

        // 최고기록
        if(_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.Save();
        }

        // Result 텍스트 출력
        if(UIManager._UM != null)
            UIManager._UM.GameOverResult(_score, _highScore);
    }

    public void ResetGame()
    {
        _isGameOver = false;
        _isPlaying = false;
        _score = 0;
    }

    private void Awake()
    {
        if (_GM == null)
        {
            _GM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}