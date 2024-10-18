using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 0.5f;  // �������� �������� ���������
    public float mouseSensitivity = 100f;  // ���������������� ����
    private Vector3 moveVector;

    private float rotationY = 0f;  // ���������� ��� �������� �������� �� ��� Y

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // ������������ ��������, ����� �������� �� �����������
    }

    void Update()
    {
        // ���������� ��������� ��������� �� �������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // ������������ ��������� �� ��� Y (�����/������)
        transform.Rotate(Vector3.up * mouseX);

        // ���������� ��������� ��������� � ����������
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        // �������� � �������� ��������� � ����������� �� ��������
        Vector3 movement = transform.forward * moveVector.z + transform.right * moveVector.x;
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }
}
