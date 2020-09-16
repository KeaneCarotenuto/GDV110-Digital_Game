﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindScript : MonoBehaviour
{

    public class StoredTransform
    {
        public int it;
        public Vector3 pos;
        public Quaternion rot;

        public StoredTransform(Vector3 _pos, Quaternion _rot)
        {
            pos = _pos;
            rot = _rot;
        }
    }

    public List<StoredTransform> recordedTrans = new List<StoredTransform>();
    public bool playback = false;
    public int itt = 0;

    private void Update()
    {
        //Debug.Log(recordedTrans[0].pos);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider == GetComponent<Collider2D>())
            {
                Debug.Log(hit.collider.gameObject.name);
                if (playback == false)
                {
                    StartPlayback();
                }
                else
                {
                    StopPlayback();
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartPlayback();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && playback == true)
        {
            StopPlayback();
        }        
    }

    void FixedUpdate()
    {
        if (playback)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (itt > 0)
            {
                Debug.Log("Playing " + recordedTrans[itt]);
                transform.position = recordedTrans[itt].pos;
                transform.rotation = recordedTrans[itt].rot;
                itt--;
            }
            else
            {
                StopPlayback();
            }
        }
        else
        {
            if (recordedTrans.Count > 1000)
            {
                recordedTrans.RemoveRange(0, 2);
            }
            recordedTrans.Add(new StoredTransform(transform.position, transform.rotation));
            
        }
    }

    public void StopPlayback()
    {
        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        
        playback = false;
        recordedTrans.Clear();
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void StartPlayback()
    {
        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        playback = true;
        itt = recordedTrans.Count - 1;
        Debug.Log("Started Playback");
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}