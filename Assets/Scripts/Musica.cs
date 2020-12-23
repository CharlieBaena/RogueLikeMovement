using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Musica : MonoBehaviour
{
    public AudioClip menu, creditos , gameplay;



    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MenuPrincipal":
                if (GetComponent<AudioSource>().clip != menu)
                {
                    GetComponent<AudioSource>().clip = menu;
                    GetComponent<AudioSource>().Play();
                }
                break;

            case "EscenaGameplay":
                if (GetComponent<AudioSource>().clip != gameplay)
                {
                    GetComponent<AudioSource>().clip = gameplay;
                    GetComponent<AudioSource>().Play();
                }
                break;

            case "Creditos":
                if (GetComponent<AudioSource>().clip != creditos)
                {
                    GetComponent<AudioSource>().clip = creditos;
                    GetComponent<AudioSource>().Play();
                }
                break;

        }

    }
}
