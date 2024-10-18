using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] waypoints;  // ����� �������� ��� ������
    private int currentWaypointIndex = 0;

    public TrafficLightCone trafficLight;  // ������ �� ��������
    public float stopDistance = 10f;  // ���������, �� ������� ������ ������ ������������ ����� ����������

    private bool isStoppedAtLight = false;  // ����, ������������, ����������� �� ������ �� ���������

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // ������������� ������ ����� ��� ��������
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("����� �� ��������� �� NavMesh!");
            return;
        }

        // ���������, ��������� �� ������ ����� �� ���������� � ����� �� ������������
        if (IsNearTrafficLight() && !trafficLight.IsGreenLight() && !isStoppedAtLight)
        {
            agent.isStopped = true;  // ������������� ������ �� ������� ��� ������ ����
            isStoppedAtLight = true;  // ������������� ����, ��� ������ �����������
            Debug.Log("������ ������������ �� ���������.");
        }

        // ���� ���� ���� �������, ������ ����� ���������� ��������
        if (isStoppedAtLight && trafficLight.IsGreenLight())
        {
            agent.isStopped = false;  // ���������� ��������
            isStoppedAtLight = false;  // ���������� ���� ���������
            Debug.Log("������ ���������� �������� �� ������� ����.");
        }

        // ���������, �������� �� ������ ������� ����� ��������
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isStoppedAtLight)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // ����� ��� ��������, ��������� �� ������ ����� �� ����������
    bool IsNearTrafficLight()
    {
        if (trafficLight == null) return false;

        // ������������ ��������� �� ���������
        float distanceToLight = Vector3.Distance(transform.position, trafficLight.transform.position);

        // ���������, ��������� �� ������ � �������� ��������� ����� ����������
        return distanceToLight < stopDistance;
    }
}
