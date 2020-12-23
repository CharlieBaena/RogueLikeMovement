using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    float speed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 res = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
        transform.position = new Vector3(res.x,res.y,-1f);
    }
}
