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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Creditos");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
