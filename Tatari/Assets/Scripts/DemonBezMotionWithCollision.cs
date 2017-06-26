using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBezMotionWithCollision : MonoBehaviour
{

    public GameObject player;
    public float speed = 1f;
    public float percSpeed = 0.01f;

    Vector3 start;
    public Vector3 controlP1;
    public Vector3 controlP2;
    Vector3 end;

    Vector3 dodgePoint;
    Vector3 prevPos;

    float startTime;
    float distance;
    float t;

    bool stop;
    bool onPostCollisionPath;
    bool reachedDodgePoint;

    void Start()
    {
        start = transform.position;
        print(start + " Demon start position");
        startTime = Time.time;
        stop = false;
    }

    void Update()
    {
        if(stop) return;

        /*if ((Time.time - startTime) < 3)
        {
            transform.position = start;
            return;
        }*/


        prevPos = transform.position;

        if (!onPostCollisionPath)
        {
            end = player.transform.position;
            t += percSpeed * Time.deltaTime;
            // Bezier curve movement towards end position
            transform.position = Mathf.Pow(1 - t, 3) * start + 3 * Mathf.Pow(1 - t, 2) * t * controlP1 +
                                                    3 * (1 - t) * Mathf.Pow(t, 2) * controlP2 + Mathf.Pow(t, 3) * end;
        }
        else
        {
            if(t > 1)
            {
                reachedDodgePoint = true;
                start = transform.position;
                startTime = Time.time;
            }
            if (!reachedDodgePoint)
            {
                float traveledDist = (Time.time - startTime) * speed;
                t = traveledDist / (Vector3.Distance(start, dodgePoint));
                transform.position = Vector3.Lerp(start, dodgePoint, t);
            }
            else
            {
                float traveledDist = (Time.time - startTime) * speed;
                t = traveledDist / (Vector3.Distance(start, end));
                transform.position = Vector3.Lerp(start, end, t);
            }
        }


        if (t == 1)
        {
            t = 0;
            startTime = Time.time;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Demon found player, selfdestruct");
            //Destroy(gameObject);
            stop = true;
        }
        else
        {
            print("Demon found non-player obstacle in trigger");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Demon found obstacle in collision");
        stop = true;
        //DodgeObstacle();
    }

    private void DodgeObstacle()
    {
        Vector3 back = transform.position - start;
        back.Normalize();
        transform.position -= back * 0.1f;
        start = transform.position;
        startTime = Time.time;
        t = 0;
        if (!Physics.Raycast(transform.position, end, 0.5f))
        {
            print("Demon free to continue forward");
            Vector3 toEnd = end - start;
            toEnd.Normalize();
            dodgePoint = transform.position + toEnd * 0.5f;
        }
        else if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), 2))
        {
            print("Demon free to dodge left");
            dodgePoint = transform.position + new Vector3(-2, 0, 0);
            //stop = true;
        }
        else if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), 2))
        {
            print("Demon free to dodge right");
            dodgePoint = transform.position + new Vector3(2, 0, 0);
        }
        onPostCollisionPath = true;
        reachedDodgePoint = false;
    }
}