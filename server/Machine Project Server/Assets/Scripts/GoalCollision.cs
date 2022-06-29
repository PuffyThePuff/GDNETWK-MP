using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollision : MonoBehaviour
{
    [SerializeField] private bool isPlayerOneGoal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
        }
    }
}