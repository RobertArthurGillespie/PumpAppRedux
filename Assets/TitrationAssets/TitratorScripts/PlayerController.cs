using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 2.0f;

    private CharacterController characterController;
    private Camera playerCamera;

    private float verticalRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Player Movement
        float forwardSpeed = Input.GetAxis("Vertical") * speed;
        float sideSpeed = Input.GetAxis("Horizontal") * speed;

        Vector3 speedVector = new Vector3(sideSpeed, 0, forwardSpeed);
        speedVector = transform.rotation * speedVector;

        characterController.SimpleMove(speedVector);

        // Player Rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * sensitivity;

        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
    }
}
