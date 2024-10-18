using System.Collections;
using UnityEngine;

public class TrafficLightCone : MonoBehaviour
{
    public Renderer coneRenderer;  // �������� ��� ��������� ����� ������
    public Color redColor = Color.red;    // ���� ��� �������� �����
    public Color yellowColor = Color.yellow;  // ���� ��� ������� �����
    public Color greenColor = Color.green;    // ���� ��� �������� �����

    private float greenDuration = 5f;   // �����, ����� ����� �������
    private float yellowDuration = 2f;  // �����, ����� ����� ������
    private float redDuration = 5f;     // �����, ����� ����� �������

    private enum TrafficState { Red, Yellow, Green }
    private TrafficState currentState;

    void Start()
    {
        if (coneRenderer == null)
        {
            coneRenderer = GetComponent<Renderer>();  // ������������� �������� �������� ������
        }

        // ��������� ����������� ����
        StartCoroutine(TrafficLightCycle());
    }

    // ������� ��� ����� ���������
    IEnumerator TrafficLightCycle()
    {
        while (true)
        {
            // ������� ����
            currentState = TrafficState.Green;
            SetLightColor(greenColor);
            yield return new WaitForSeconds(greenDuration);

            // ������ ����
            currentState = TrafficState.Yellow;
            SetLightColor(yellowColor);
            yield return new WaitForSeconds(yellowDuration);

            // ������� ����
            currentState = TrafficState.Red;
            SetLightColor(redColor);
            yield return new WaitForSeconds(redDuration);
        }
    }

    // ����� ��� ��������� ����� ������
    void SetLightColor(Color color)
    {
        coneRenderer.material.color = color;
    }

    // ����� ��� �������� �������� ��������� ��������� (������� ����)
    public bool IsGreenLight()
    {
        return currentState == TrafficState.Green;
    }
}
