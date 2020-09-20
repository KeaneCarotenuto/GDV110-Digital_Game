using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindScript : MonoBehaviour
{

    public class StoredTransform
    {
        public bool initial;
        public float time;
        public Vector3 pos;
        public Quaternion rot;

        public StoredTransform(float _time, Vector3 _pos, Quaternion _rot, bool _initial = false)
        {
            time = _time;
            pos = _pos;
            rot = _rot;
            initial = _initial;
        }
    }

    public List<StoredTransform> recordedTrans = new List<StoredTransform>();
    public bool playback = false;
    public int itt = 0;

    private void Awake()
    {
        recordedTrans.Insert(0,new StoredTransform(Time.time, transform.position, transform.rotation));
    }

    private void Update()
    {
        //Debug.Log(recordedTrans[0].pos);

        if (Input.GetMouseButtonDown(0) && false)   ///REMOVE FALSE
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
            Time.timeScale = 1.0f;

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (itt <= recordedTrans.Count-1)
            {
                if (recordedTrans[itt].initial == true)
                {
                    Debug.Log("ORIG!");
                    if (GetComponent<Explodable>() != null)
                    {
                        Debug.Log("ORIG2!");
                        GetComponent<Explodable>().UnhideOriginal();
                    }
                }

                Debug.Log("Playing " + recordedTrans[itt]);
                transform.position =  Vector3.Lerp(transform.position ,recordedTrans[itt].pos, 1f);
                transform.rotation =  Quaternion.Lerp(transform.rotation, recordedTrans[itt].rot, 1f);
                itt++;
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
                recordedTrans.RemoveRange(recordedTrans.Count-2, 2);
            }
            recordedTrans.Insert(0,new StoredTransform(Time.time, transform.position, transform.rotation));
            
        }
    }

    public void StopPlayback()
    {
        Time.timeScale = 1.0f;

        if (GetComponent<Rigidbody2D>())
        {
            if (GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static) GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        
        playback = false;
        recordedTrans.Clear();
        if (gameObject.GetComponent<SpriteRenderer>()) GetComponent<SpriteRenderer>().color = Color.white;
        if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void StartPlayback()
    {
        Time.timeScale = 1.0f;

        if (GetComponent<Rigidbody2D>())
        {
            if (GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static) GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }

        playback = true;
        itt = 0;
        Debug.Log("Started Playback");
        if (gameObject.GetComponent<SpriteRenderer>()) GetComponent<SpriteRenderer>().color = Color.green;
        if (gameObject.GetComponent<MeshRenderer>()) gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
