using UnityEngine;
using UnityEngine.AI;

public class PedestrianCrossingTrigger : MonoBehaviour
{
    public TrafficLightCone pedestrianLight;  // ������ �� �������� ��� ���������

    void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������, �������� � ����, ��� ���
        if (other.CompareTag("Cube"))
        {
            // ��������� ��������� ���������
            if (pedestrianLight != null && !pedestrianLight.IsGreenLight())
            {
                // ������������� ���, ���� �������� �� �������
                NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.isStopped = true;
                    Debug.Log("��� ����������� ����� ���������, ���� �������� �����.");
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // ���������, ��� ������, ���������� � ����, ��� ���
        if (other.CompareTag("Cube"))
        {
            // ���� �������� ���������� �������, ���������� ��������
            if (pedestrianLight != null && pedestrianLight.IsGreenLight())
            {
                NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.isStopped = false;
                    Debug.Log("��� ��������� �������� �� ������� ����.");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // ����� ��� �������� ����
        if (other.CompareTag("Cube"))
        {
            Debug.Log("��� ������� ���� ����������� ��������.");
        }
    }
}
