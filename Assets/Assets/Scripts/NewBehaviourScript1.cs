using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CubeManager : MonoBehaviour
{
    public GameObject cubePrefab;  // ������ ����
    public Transform[] homes;      // ����� ������ (����)
    public Transform[] waypoints;  // ����� ����� (������, �������� � �.�.)
    public float spawnInterval = 5f; // �������� ��������� �����

    void Start()
    {
        // ��������� ������� ��� �������������� ������ �����
        StartCoroutine(SpawnCubes());
    }

    IEnumerator SpawnCubes()
    {
        while (true)
        {
            // ����� ���� � ���������� �����������
            SpawnRandomCube();

            // ���� ����� ��������� �������
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // ����� ��� ������ ���� � ���������� �����������
    void SpawnRandomCube()
    {
        // �������� ��������� ��� ��� ������
        Transform home = homes[Random.Range(0, homes.Length)];

        // ������� ����� ��� � ������� ����
        GameObject newCube = Instantiate(cubePrefab, home.position, Quaternion.identity);

        // �������� ������ ���������� �����
        CubeAI cubeAI = newCube.GetComponent<CubeAI>();

        // ����������� ��������� ���� ��� ������ � ���������
        cubeAI.InitializeTasks(waypoints, home);
    }
}
