using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Object.DontDestroyOnLoad example.
//
// This script example manages the playing audio. The GameObject with the
// "music" tag is the BackgroundMusic GameObject. The AudioSource has the
// audio attached to the AudioClip.

public class DontDestroy : MonoBehaviour
{
    public int priority;
    public bool dontDellOnTitle;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        int topPriority = -10;
        foreach (GameObject _object in objs)
        {
            if (_object.GetComponent<DontDestroy>().priority > topPriority)
            {
                topPriority = _object.GetComponent<DontDestroy>().priority;
            }
        }

        int desCount = 0;
        foreach (GameObject _object in objs)
        {
            if (_object.GetComponent<DontDestroy>().priority < topPriority)
            {
                desCount++;
                Destroy(_object);
                Debug.Log("Destroyed " + _object.name + ", by " + gameObject.name + " bc more important exists");
            }
        }

        if (objs.Length - desCount > 1)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed " + gameObject.name + ", by " + gameObject.name + " bc too many");
        }

    }

    void Update()
    {
        if (!dontDellOnTitle)
        {
            if (SceneManager.GetActiveScene().name.Contains("Title"))
            {
                Destroy(gameObject);
                Debug.Log("Destroyed " + gameObject.name + ", by " + gameObject.name + " bc more on title");
            }
        }
        
    }
}
