using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    IEnumerator Load(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            Debug.Log("Loading");
            yield return null;
        }

        Debug.Log("Waiting to Finish Loading");

        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            yield return null;
        }

        Debug.Log("Done");

        yield return null;

        if (sceneName == "Game")
        {
            GameManager._GM.StartGame();
        }
        
        if(sceneName == "Title")
        {
            BaseCubeEF cube = GameObject.Find("BaseCube")?.GetComponent<BaseCubeEF>();

            if (cube != null)
            {
                
            }
            else
            {
                Debug.Log("BaseCube ¸øÃ£À½");
            }
        }
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
