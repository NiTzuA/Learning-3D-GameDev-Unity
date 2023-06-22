using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;

    void Update()
    {

        if (Input.GetKey(KeyCode.Alpha1))
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            weapon3.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(true);
        }

    }
}
