using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CubeAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform home;
    private Transform[] tasks;  // ������ � ���������� ��������
    private Transform[] assignedTasks; // ���������� ������ ��� ������� ����
    private int currentTaskIndex = 0;
    private int numberOfTasksToComplete; // ���������� ����� ��� ����������
    private bool allTasksComplete = false; // �������� ���������� ���� �����
    private float workPauseTime = 5f;   // ����� ����� �������� (����� ���������� ������)
    private float restTime = 10f;       // ����� ������
    public string[] disappearTaskNames; // ������ � ������� �����, ��� ���� ������ ��������

    private Vector3 initialPosition;  // ��������� �������, ������ ����� ����� ��������
    private bool isOnLongPath = false;  // ����, �����������, ��� ����� ��� �� �������� ����
    private float maxDistanceBeforeCommit = 5f;  // ������������ ����������, ��� ������� ����� ����� ��������� �� �������� ����

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;  // ��������� ��������� ������� ����
        StartCoroutine(PerformTaskRoutine());
    }

    // ������������� ����� � ����
    public void InitializeTasks(Transform[] taskPoints, Transform homePoint)
    {
        home = homePoint;
        tasks = taskPoints;

        // �������� �������� ���������� ����� �� 2 �� 6
        numberOfTasksToComplete = Random.Range(2, 7);

        // ��������� ���������� ������ ��� ����
        assignedTasks = new Transform[numberOfTasksToComplete];
        for (int i = 0; i < numberOfTasksToComplete; i++)
        {
            assignedTasks[i] = tasks[Random.Range(0, tasks.Length)];
        }

        Debug.Log("���� ��������� " + numberOfTasksToComplete + " �����.");
    }

    // �������� ������� ��� ���������� �����, ������ � ����������� �����
    IEnumerator PerformTaskRoutine()
    {
        if (assignedTasks == null || assignedTasks.Length == 0)
        {
            Debug.LogError("������ �� ��������� ����! ��� �� ����� ��������.");
            yield break;  // ��������� ���������� ��������, ���� ��� �����
        }

        // ��� ��� � ������ �� ����� �� �������
        for (int i = 0; i < assignedTasks.Length; i++)
        {
            currentTaskIndex = i;
            MoveToTask(assignedTasks[currentTaskIndex]);

            // ���, ���� ��� ������ �� ����� ������ ��� ��������� � ������������
            yield return StartCoroutine(HandlePath());

            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("��� �������� ������ " + (i + 1) + " �� " + assignedTasks.Length + ".");
                yield return new WaitForSeconds(workPauseTime); // ��� �������� �� ����� 5 ������

                // ������, ����� �� ��� ��������� (�������� ����� ������ ������)
                if (ShouldRest())
                {
                    Debug.Log("��� ����� ���������.");
                    yield return StartCoroutine(RestRoutine());
                }
            }
            else
            {
                Debug.Log("���� ������������, ��� ���� ������ ������.");
                yield return new WaitForSeconds(3f);  // ��� ������� ����� ������� ������ ������
            }
        }

        // ��� ������ ���������
        allTasksComplete = true;
        Debug.Log("��� �������� ��� ������ � ������������ �����.");

        // ��� ������������ �����
        MoveToHome();

        // ��� ����������� �����
        yield return new WaitUntil(() => agent.remainingDistance < 0.5f && !agent.pathPending);
        Debug.Log("��� �������� �����.");

        // �������� ����� �������� �����
        Disappear();
    }

    // ����������� � ������
    void MoveToTask(Transform task)
    {
        if (task != null)
        {
            Debug.Log("��� ������������ � ������: " + task.name);
            agent.SetDestination(task.position);
        }
        else
        {
            Debug.LogError("������ �� ���������!");
        }
    }

    // ����� ��� �����������, ����� ��� ������ ��������� (��������, 50% ����)
    bool ShouldRest()
    {
        return Random.Range(0f, 1f) < 0.2f;  // 50% ���� �� ����� ����� ������ ������
    }

    // ������� ��� ���������� ������
    IEnumerator RestRoutine()
    {
        Debug.Log("��� �������� � ������� " + restTime + " ������.");
        yield return new WaitForSeconds(restTime);  // ��� �������� �������� �����
    }

    // ����������� �����
    void MoveToHome()
    {
        if (home != null)
        {
            Debug.Log("��� ������������ �����.");
            agent.SetDestination(home.position);
        }
        else
        {
            Debug.LogError("��� ��� ���� �� ��������!");
        }
    }

    // ������������ ���� ����� ���������� �����
    void Disappear()
    {
        if (allTasksComplete)
        {
            Debug.Log("��� �������� ��� ������ � ��������.");
            Destroy(gameObject);  // ������� ������
        }
    }

    // ����� ��� ��������� �������� � �������� �� �������� ����
    IEnumerator HandlePath()
    {
        while (agent.pathPending)
        {
            yield return null; // ���, ���� ���� ����� ���������
        }

        // ���� ���� ������������
        if (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("���� ������������. ��� ���.");
            agent.ResetPath();  // ������������� ������
            yield return new WaitForSeconds(3f);  // ��� ��� �� �����

            // ���������, �������� �� ����� �������� ����
            if (ShouldRecalculatePath())
            {
                Debug.Log("�������� ����� �������� ����! �������� ����.");
                agent.SetDestination(assignedTasks[currentTaskIndex].position);
            }
        }
    }

    // ��������, ����� �� ������������� ����
    bool ShouldRecalculatePath()
    {
        // ���� ����� ��� ���� ������� ������ �� �������� �������, �� �� ������������
        if (Vector3.Distance(transform.position, initialPosition) > maxDistanceBeforeCommit)
        {
            Debug.Log("����� ������� ������, ����� ��������� �� �������� ����.");
            return false;
        }

        // ������������ ������� ����
        NavMeshPath newPath = new NavMeshPath();
        agent.CalculatePath(assignedTasks[currentTaskIndex].position, newPath);

        // ���������, �������� �� ����� �������� ���� (���� ���� �������� � ��� ����� ������ ��������)
        if (newPath.status == NavMeshPathStatus.PathComplete && CalculatePathLength(newPath) < CalculatePathLength(agent.path))
        {
            return true;
        }

        return false;
    }

    // ����� ��� ������� ����� ����
    float CalculatePathLength(NavMeshPath path)
    {
        float length = 0f;
        if (path.corners.Length < 2)
            return length;

        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return length;
    }
}
