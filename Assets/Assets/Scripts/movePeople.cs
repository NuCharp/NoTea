using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class movePeople : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    public float LookRadius;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = playerManager.instance.transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= LookRadius)
        {
            agent.SetDestination(target.position);
            if(distance <= agent.stoppingDistance)
            {
                LookTarget();
            }
        }
    }

    void LookTarget()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}
