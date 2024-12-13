using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMainMenu : MonoBehaviour
{
    public void OnReturnBtnClick() {
        SceneManager.LoadScene("MainMainMenu");
    }
}
