using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class NextLevel : MonoBehaviour
{
    enum Scenes
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GotoNextLevel();
    }

    private void Update()
    {
        if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.leftShiftKey.isPressed && Keyboard.current.nKey.wasPressedThisFrame)
        {
            GotoNextLevel();
        }
    }

    public void GotoNextLevel()
    {
        string levelName = SceneManager.GetActiveScene().name.Substring(0, 5) + (int.Parse(SceneManager.GetActiveScene().name.Substring(5, 1)) + 1).ToString();
        if (levelName == "Level8") levelName = "Title Screen";

        SceneManager.LoadScene(levelName);
    }
}
