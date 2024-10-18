using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 0.5f;  // �������� �������� ���������
    public float mouseSensitivity = 100f;  // ���������������� ����
    public Transform playerCamera;  // ������ �� ������ ��������� (��� ���������� �������)
    private Vector3 moveVector;

    private float rotationX = 0f;  // ���������� ��� �������� ������������� �������� (�����/����)
    private float rotationY = 0f;  // ���������� ��� ��������������� �������� (�����/������)

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // ������������ ��������, ����� �������� �� �����������
    }

    void Update()
    {
        // ���������� ��������� ��������� �� ����������� (�����/������)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);  // ������������ ���� ��������� �� ��� Y

        // ���������� ��������� ������ (�����/����)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationX -= mouseY;  // ����������� ����������� �� ��� X (���� ����� -> ������ ����)
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);  // ������������ ������������ ������� ������ �� 90 ��������

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);  // ������� ������ �� ��� X

        // ���������� ��������� ��������� � ����������
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        // �������� � �������� ��������� � ����������� �� ��������
        Vector3 movement = transform.forward * moveVector.z + transform.right * moveVector.x;
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }
}
