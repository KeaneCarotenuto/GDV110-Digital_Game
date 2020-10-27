using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PedestalCollide : MonoBehaviour
{

    public UnityEvent OnPedestalTouch;
    bool untouched = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (untouched)
        {
            OnPedestalTouch.Invoke();
            untouched = false;
        }

    }
}
