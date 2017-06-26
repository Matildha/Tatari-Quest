using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonMotion : MonoBehaviour {

    public GameObject player;
    public float percSpeed;

    Vector3 start;
    public GameObject controlP1;
    public GameObject controlP2;
    Vector3 end;

    float t;
    bool stop;
    float startTime;

    void Start () {
        start = transform.position;
        stop = false;
        t = 0f;
        startTime = Time.time;
	}
	
	void Update () {

        if ((Time.time - startTime) < 2)
        {
            transform.position = start;
            return;
        }


        if (!stop)
        {
            end = player.transform.position;
            t += percSpeed * Time.deltaTime;
            // Bezier curve movement towards end position
            transform.position = Mathf.Pow(1 - t, 3) * start + 3 * Mathf.Pow(1 - t, 2) * t * controlP1.transform.position +
                                                    3 * (1 - t) * Mathf.Pow(t, 2) * controlP2.transform.position + Mathf.Pow(t, 3) * end;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            print("Demon found player, selfdestruct");
            stop = true;
            //Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Demon found obstacle in collision, selfdestruct");
        stop = true;
        //Destroy(gameObject);
    }
}
