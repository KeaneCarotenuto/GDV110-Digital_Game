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

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        int topPriority = 0;
        foreach (GameObject _object in objs)
        {
            if (_object.GetComponent<DontDestroy>().priority > topPriority)
            {
                topPriority = _object.GetComponent<DontDestroy>().priority;
            }
        }

        foreach (GameObject _object in objs)
        {
            if (_object.GetComponent<DontDestroy>().priority < topPriority)
            {
                Destroy(_object);
            }
        }

        
    }

    void Update()
    {

        if (SceneManager.GetActiveScene().name.Contains("Title"))
        {
            Destroy(this.gameObject);
        }
    }
}
