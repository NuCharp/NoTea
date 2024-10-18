using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveCar : MonoBehaviour
{
    public Transform target;  // ���� ��� �������� (����� ������ ������ ��� ����� ��������)
    private NavMeshAgent agent;
    public float lookRadius = 10f;  // ������ ����������� ����

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // ���� ���� �� ��������� ����� ���������, ������� ����� �
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
                agent.SetDestination(target.position);  // ������������� ���� ��������

                // ���� �������� ���������
                if (distance <= agent.stoppingDistance)
                {
                    Debug.Log("������ �������� ����");
                    // ����� ����� �������� ������: ���������� ������, ������� ����, � �.�.
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // ���������� ������ ����������� � ���������
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
