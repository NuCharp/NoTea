using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 0.5f;  // —корость движени€ персонажа
    public float mouseSensitivity = 100f;  // „увствительность мыши
    private Vector3 moveVector;

    private float rotationY = 0f;  // ѕеременна€ дл€ хранени€ вращени€ по оси Y

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // «амораживаем вращение, чтобы персонаж не заваливалс€
    }

    void Update()
    {
        // ”правление поворотом персонажа по движению мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // ѕоворачиваем персонажа по оси Y (влево/вправо)
        transform.Rotate(Vector3.up * mouseX);

        // ”правление движением персонажа с клавиатуры
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        // ѕриводим в движение персонажа в зависимости от поворота
        Vector3 movement = transform.forward * moveVector.z + transform.right * moveVector.x;
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }
}
