using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    public Slider LoadSlide;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Menu");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            LoadSlide.value = async.progress;
            if (async.progress == 0.9f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
