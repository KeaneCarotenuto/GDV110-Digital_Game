using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReset : MonoBehaviour
{

    bool resetting = false;
    public float resettime = 1f;
    public Animator anim;
    public AudioSource audio;
    public List<string> HazardTags;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HazardTags.Contains(collision.tag))
        {
            ReloadLevel();
        }

    }

    public void ReloadLevel()
    {
        audio.Play();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(resettime);
        SceneManager.LoadScene(levelIndex);
    }
}
