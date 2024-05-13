using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{

    NavMeshAgent Agent => GetComponent<NavMeshAgent>();

    public float Speed { get => Agent.speed; set { Agent.speed = value; } }

    public void GoToPosition(Vector3 position)
    {
        Agent.enabled = true;
        Agent.isStopped = false;
        Agent.SetDestination(position);
    }

    public void RawMovement(Vector3 direction)
    {
        Agent.enabled = false;
        transform.position += direction.normalized * Time.deltaTime* Agent.speed;

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void Stop()
    {
        Agent.enabled = true;
        Agent.isStopped = true;
    }

    public void LookAt(Vector3 position)
    {
        var direction = position - transform.position;
        direction.y = 0;
        direction.Normalize();

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

}
