using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    void Update()
    {
        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Rotation
        float rotatelInput = Input.GetAxis("RotationHorizontal");
        float rotationAmount = -rotatelInput*rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationAmount*rotationSpeed * Time.deltaTime, Space.World);

        rotatelInput = Input.GetAxis("RotationVertical");
        rotationAmount = -rotatelInput*rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.left, rotationAmount*rotationSpeed * Time.deltaTime);



    }
}
