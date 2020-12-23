using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuPrincipal : MonoBehaviour
{


    public void Play()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaGameplay");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().panel.gameObject.SetActive(false);
    }
}
