using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingBar;

    void Start()
    {
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        float value = Random.Range(0.4f, 0.7f);

        while (!operation.isDone)
        {
            /*
            fake progress bar becasue I couldn't figure out the real one yet
            */
            loadingBar.GetComponent<Slider>().value = value;
            yield return null;
        }

    }
}
