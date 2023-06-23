using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public Gun equippedGun1;
    public Gun equippedGun2;
    public Gun equippedGun3;
    public TMP_Text weaponName;
    public TMP_Text weaponCurrentAmmo;
    public TMP_Text magSize;
    public TMP_Text sensitivityValue;
    Gun currentGun;
    public static GameManager instance;
    public static bool gameIsPaused = false;
    public GameObject pauseUI;
    MouseController mouseController;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        mouseController = MouseController.instance;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Alpha1))
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
            currentGun = equippedGun1;
            UpdateText();
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            weapon3.SetActive(false);
            currentGun = equippedGun2;
            UpdateText();
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {      
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(true);
            currentGun = equippedGun3;
            UpdateText();
        }

        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("SampleScene");
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseController.gameIsPaused = false;
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        mouseController.gameIsPaused = true;
        sensitivityValue.text = "Sensitivity: " + mouseController.mouseSensitivity.ToString("F2");
    }


    public void UpdateText()
    {
        weaponCurrentAmmo.text = currentGun.currentAmmo.ToString();
        weaponName.text = currentGun.weaponName;
        magSize.text = "/" + currentGun.magSize.ToString();
    }

}
