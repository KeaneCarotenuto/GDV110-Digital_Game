using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditsButton ()
    {
        Debug.Log ("Credits");
    }

    public void QuitGame ()
    {
        Debug.Log ("Quit");
        Application.Quit();
    }
}
