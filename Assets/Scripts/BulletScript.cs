using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float spawnTime;
    public float liveTime;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTime + liveTime < Time.timeSinceLevelLoad)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("PLAYER DIED");
        }

        Destroy(gameObject);
    }
}
