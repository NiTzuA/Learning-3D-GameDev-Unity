using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;
    public Transform rayCaster;

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 300;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; //GetAxis is already independent of FPS? So no need for Time.deltaTime... huh...
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        yRotation -= mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        rayCaster.transform.localRotation = Quaternion.Euler(xRotation, 0f, yRotation); //Use the raycaster when recoil has been implemented
    }
}
