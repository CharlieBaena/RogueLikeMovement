using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnerEnemigos : MonoBehaviour
{
    [SerializeField]
    GameObject prefabEnemigo;
    [SerializeField]
    Transform[] spawnPositions;
    [SerializeField]
    float tiempoEntreSpawns = 5f;
    [SerializeField]
    int enemigosCadaSpawn = 4;
    [SerializeField]
    GameObject player;


    private int cantidadSpawneada = 0;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemigos", 0f, tiempoEntreSpawns);
    }

    // Update is called once per frame
    void Update()
    {

        switch (cantidadSpawneada)
        {
            case 10:
                enemigosCadaSpawn = 8;
                break;
            case 20:
                enemigosCadaSpawn = 16;
                break;
            case 50:
                enemigosCadaSpawn = 32;
                break;
            case 100:
                enemigosCadaSpawn = 64;
                break;
        }

    }



    void SpawnEnemigos()
    {
        GameObject enemigoSpawneado;
        for(int i= 0; i< enemigosCadaSpawn; i++)
        {
            int random = Random.Range(0, 4);

            enemigoSpawneado = Instantiate(prefabEnemigo, Vector3.zero, Quaternion.identity);
            switch (random)
            {
                case 0:
                    enemigoSpawneado.transform.position = spawnPositions[0].position;
                    break;
                case 1:
                    enemigoSpawneado.transform.position = spawnPositions[1].position;
                    break;
                case 2:
                    enemigoSpawneado.transform.position = spawnPositions[2].position;
                    break;
                case 3:
                    enemigoSpawneado.transform.position = spawnPositions[3].position;
                    break;
            }

            enemigoSpawneado.GetComponent<Enemigo>().player = player;
            cantidadSpawneada++;
            i++;
        }
    }
}
