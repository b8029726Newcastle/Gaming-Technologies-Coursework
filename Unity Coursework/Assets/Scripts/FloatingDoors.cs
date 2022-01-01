using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDoors : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    public float speed = 2.5f, duration = 5f;
    public bool repeatable;
    //float startX, startY;
    Vector3 startPos, minScale;

    [SerializeField]
    Vector3 maxScale;

    // Start is called before the first frame update
    IEnumerator  Start() //change into IEnumerator to accomodate for doorRight Coroutine
                         //left door is like normal code except it now yield returns null because of the IEnumerator
    {
        //doorLeft = GameObject.FindGameObjectWithTag("DoorLeft1");
        //doorRight = GameObject.FindGameObjectWithTag("DoorRight1");

        if (door.tag == "DoorLeft1")
        {
            //get current (initial, starting) position of object -- the door in which this script is attached to
            startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            yield return null;
            //startX = transform.position.x;
            //startY = transform.position.y;
        }
        else if (door.tag == "DoorRight1")
        {
            //get current (initial, starting) position of object -- the door in which this script is attached to
            //startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            //Lerper();

            //reset the scale or size of the door using minScale & maxScale values

            startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            minScale = transform.localScale / 2; //make the minimum scale or size of the door to half than the original

            while (repeatable)
            {
                //lerp up scale and lerp down
                yield return RepeatLerp(minScale, maxScale, duration);
                yield return RepeatLerp(maxScale, minScale, duration);
            }
        }


    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        //start at 0 and  slowly increment until 1
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
        //move from {55, 0, 27} to {60, 0, 27} and back
        //transform.position = new Vector3(startX + (Mathf.PingPong(Time.time * speed, 5)), transform.position.y, transform.position.z);
        //transform.position = new Vector3(transform.position.x, startY + Mathf.PingPong(Time.time * speed, 3), transform.position.z);

        if(door.tag == "DoorLeft1")
        {
            transform.position = new Vector3(startPos.x + (Mathf.PingPong(Time.time * speed, 6)), transform.position.y, transform.position.z);
            transform.position = new Vector3(transform.position.x, startPos.y + Mathf.PingPong(Time.time * speed, 2), transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z + Mathf.PingPong(Time.time * speed, 4));
        }
        else if (door.tag == "DoorRight1")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z + Mathf.PingPong(Time.time * speed, 10));
        }

    }
}
