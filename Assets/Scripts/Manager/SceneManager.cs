using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    IEnumerator Load(string sceneName)
    {
        //yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        yield return null;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }

    void Awake()
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
