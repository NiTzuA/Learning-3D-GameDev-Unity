using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MouseController : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;
    public Transform rayCaster;
    public static MouseController instance;
    public bool gameIsPaused;
    public Slider sensitivitySlider;
    public TMP_Text sensitivityValue;

    float xRotation = 0f;
    float yRotation = 0f;


    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 300;
        gameIsPaused = false;
    }

    void Update()
    {
        if (gameIsPaused)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; //GetAxis is already independent of FPS? So no need for Time.deltaTime... huh...
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        yRotation -= mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        rayCaster.transform.localRotation = Quaternion.Euler(xRotation, 0f, yRotation); //Use the raycaster when recoil has been implemented
    }

    public void ChangeSensitivity()
    {
        mouseSensitivity = sensitivitySlider.value;
        sensitivityValue.text = "Sensitivity: " + mouseSensitivity.ToString("F2");
    }
}
