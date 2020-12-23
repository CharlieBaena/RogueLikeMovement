using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Proyectile : MonoBehaviour
{
    public Vector3 direction;

    [SerializeField]
    float speedProjectile;

    Vector3 dir;
    Animator animator;
    Rigidbody2D myRB;
    bool unaVez = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRB = GetComponent<Rigidbody2D>();
        //dir = (direction.position - transform.position);
        dir = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (direction != null)
        {
            if (unaVez)
            {
                dir = (direction - transform.position);
                //print(dir);
                unaVez = false;
            }
            myRB.MovePosition(myRB.position + (Vector2)dir.normalized * speedProjectile * Time.fixedDeltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Explode", true);
            Destroy(gameObject, 0.5f);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
            }
        }

    }

}
