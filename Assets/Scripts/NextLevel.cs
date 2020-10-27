using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class NextLevel : MonoBehaviour
{

    public float resettime = 1;
    public float delay = 0;
    public AudioSource sound = null;
    public Animator anim;
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
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
    }

    private void Update()
    {
        if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.leftShiftKey.isPressed && Keyboard.current.nKey.wasPressedThisFrame)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(delay);
        anim.SetTrigger("Start");
        if(sound != null)
        {
            sound.Play();
        }
        yield return new WaitForSeconds(resettime);
        SceneManager.LoadScene(levelIndex);
    }
}
