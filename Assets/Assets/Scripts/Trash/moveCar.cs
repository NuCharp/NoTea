using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveCar : MonoBehaviour
{
    public Transform target;  // Цель для движения (можно задать игрока или точку маршрута)
    private NavMeshAgent agent;
    public float lookRadius = 10f;  // Радиус обнаружения цели

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Если цель не назначена через инспектор, попытка найти её
        if (target == null && playerManager.instance != null)
        {
            target = playerManager.instance.transform;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);  // Устанавливаем цель движения

                // Если достигли остановки
                if (distance <= agent.stoppingDistance)
                {
                    Debug.Log("Машина достигла цели");
                    // Здесь можно добавить логику: остановить машину, сменить цель, и т.д.
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Отображаем радиус обнаружения в редакторе
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
