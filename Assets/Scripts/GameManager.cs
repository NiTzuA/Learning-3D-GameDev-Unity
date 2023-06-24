using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject weapon4;
    public GameObject weapon5;
    public GameObject weapon6;
    public Gun equippedGun1;
    public Gun equippedGun2;
    public Gun equippedGun3;
    public Gun equippedGun4;
    public Gun equippedGun5;
    public Gun equippedGun6;
    public TMP_Text weaponName;
    public TMP_Text weaponCurrentAmmo;
    public TMP_Text magSize;
    public TMP_Text sensitivityValue;
    Gun currentGun;
    public static GameManager instance;
    public bool gameIsPaused = false;
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


        if (Input.GetKeyDown(KeyCode.P))
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

        if (gameIsPaused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
            weapon4.SetActive(false);
            weapon5.SetActive(false);
            weapon6.SetActive(false);
            currentGun = equippedGun1;
            Invoke("UpdateText", 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            weapon3.SetActive(false);
            weapon4.SetActive(false);
            weapon5.SetActive(false);
            weapon6.SetActive(false);
            currentGun = equippedGun2;
            Invoke("UpdateText", 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {      
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(true);
            weapon4.SetActive(false);
            weapon5.SetActive(false);
            weapon6.SetActive(false);
            currentGun = equippedGun3;
            Invoke("UpdateText", 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
            weapon4.SetActive(true);
            weapon5.SetActive(false);
            weapon6.SetActive(false);
            currentGun = equippedGun4;
            Invoke("UpdateText", 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
            weapon4.SetActive(false);
            weapon5.SetActive(true);
            weapon6.SetActive(false);
            currentGun = equippedGun5;
            Invoke("UpdateText", 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
            weapon4.SetActive(false);
            weapon5.SetActive(false);
            weapon6.SetActive(true);
            currentGun = equippedGun6;
            Invoke("UpdateText", 0.1f);
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
