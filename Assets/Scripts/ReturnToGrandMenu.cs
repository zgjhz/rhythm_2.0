using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToGrandMenu : MonoBehaviour
{
    public void OnExitButtonClick() {
        SceneManager.LoadScene("TestMenu");
    }
}
