using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private string titleSceneName;
    [SerializeField] private string mainSceneName;

    public void Restart()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void Home()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
