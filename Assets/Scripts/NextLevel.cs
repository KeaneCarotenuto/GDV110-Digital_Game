using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        string levelName = SceneManager.GetActiveScene().name.Substring(0, 5) + (int.Parse(SceneManager.GetActiveScene().name.Substring(5, 1)) + 1).ToString();
        SceneManager.LoadScene(levelName);
    }
}
