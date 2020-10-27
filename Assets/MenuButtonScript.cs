using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject QuitButton;

    public string Level1Name;

    private void Update()
    {
        if (StartButton.GetComponent<Button>().image.sprite == StartButton.GetComponent<Button>().spriteState.highlightedSprite)
        {
            Debug.Log("Highlighted");
            StartButton.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        else
        {
            StartButton.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Level1Name, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
