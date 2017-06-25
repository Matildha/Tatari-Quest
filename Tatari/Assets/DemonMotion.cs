using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonMotion : MonoBehaviour {

    private Vector3 start;
    private Vector3 end;
    public GameObject player;
    public float speed = 1f;

    float startTime;
    float distance;

	void Start () {
        start = this.transform.position;
        startTime = Time.time;
        end = player.transform.position;
        distance = Vector3.Distance(start, end);
	}
	
	void Update () {
        float travelDist = (Time.time - startTime) * speed;
        end = player.transform.position;
        distance = Vector3.Distance(start, end);
        float t = travelDist / distance;
        transform.position = Vector3.Lerp(start, end, t);
        if (t == 1)
        {
            t = 0;
            startTime = Time.time;
        }
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            print("Demon found player, selfdestruct");
            Destroy(gameObject);
        }
    }
}
