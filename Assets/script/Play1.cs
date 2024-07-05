using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play1 : MonoBehaviour
{
    public void Back(){
        SceneManager.LoadScene("MainMenu");
    }

    void OnMouseDown()
    {
        // Value to be passed
        int valueToPass = 1;
        PlayerPrefs.SetInt("PassedValue", valueToPass);
        
        SceneManager.LoadScene("InformationAyah");
    }
}
