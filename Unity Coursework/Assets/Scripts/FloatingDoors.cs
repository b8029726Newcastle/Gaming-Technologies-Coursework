using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //to access Scene Manager

public class FloatingDoors : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    public float speed = 2.5f, duration = 5f;
    public bool repeatable;
    Vector3 startPos, minScale;

    [SerializeField]
    Vector3 maxScale;

    // Start is called before the first frame update
    IEnumerator  Start() //changed into IEnumerator to accomodate for doorRight Coroutine
                         //"if statement" (left door) below is like normal code except it now "yield returns null" because of the IEnumerator
    {
        if (door.tag == "DoorLeft1")
        {
            //get current (initial, starting) position of object -- the door in which this script is attached to
            startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            yield return null;
        }
        else if (door.tag == "DoorRight1")
        {
            //get current (initial, starting) position of object -- the door in which this script is attached to
            startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            minScale = transform.localScale / 2; //make the minimum scale or size of the door to half than the original

            while (repeatable)
            {
                //reset the scale or size of the door using minScale & maxScale values
                //lerp the scale up and then lerp down
                yield return RepeatLerp(minScale, maxScale, duration);
                yield return RepeatLerp(maxScale, minScale, duration);
            }
        }


    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        //start at 0 and slowly increment until 1
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while(i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(door.tag == "DoorLeft1")
        {
            //A combination of these Ping Pongs (into multiple directions at varying lengths) exhibits a door moving diagonally upwards and downwards ---
            //as well forwards and backwards
            transform.position = new Vector3(startPos.x + (Mathf.PingPong(Time.time * speed, 6)), transform.position.y, transform.position.z); //move from {55, 0, 27} to {61, 0, 27} and back
            transform.position = new Vector3(transform.position.x, startPos.y + Mathf.PingPong(Time.time * speed, 2), transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z + Mathf.PingPong(Time.time * speed, 4));
        }
        else if (door.tag == "DoorRight1")
        {
            //move the door forwards and backwards
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z + Mathf.PingPong(Time.time * speed, 10));
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //using plain "gameObject" over "other.gameObject" because I want the tag of the current object itself
        // (the object in which this script is attached to)
        if (gameObject.CompareTag("DoorLeft1"))
        {
            Debug.Log($"Colliding with {gameObject.tag} , loading Level 1!");
            SceneManager.LoadScene("Level1");
        }

        if (gameObject.CompareTag("DoorRight1"))
        {
            Debug.Log($"Colliding with {gameObject.tag} , loading Level 2!");
            SceneManager.LoadScene("Level2");
        }
    }
}
