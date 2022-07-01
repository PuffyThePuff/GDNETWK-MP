using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private Transform tempSpawnPoint;
    [SerializeField] private Transform fieldCenter;

    private float respawnTimer = 3.0f;
    private bool isRespawning = false;

    // Update is called once per frame
    void Update()
    {
        if (isRespawning)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0.0f)
            {
                this.gameObject.transform.position = fieldCenter.position;
                isRespawning = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            this.gameObject.transform.position = tempSpawnPoint.position;
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            isRespawning = true;
            respawnTimer = 3.0f;
        }
    }
}