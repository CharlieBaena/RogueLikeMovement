using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D myRB;
    public Image panel;


    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float dashDuration = 0.15f;
    [SerializeField]
    float dashDistance = 1f;
    [SerializeField]
    float dashCooldown = 0.5f;
    [SerializeField]
    LayerMask limitLayer;
    [SerializeField]
    Text youLoseText;
    [SerializeField]
    Image[] contenedoresVidas;
    [SerializeField]
    Button tryAgainButton;

    Animator animator;
    Vector2 movement;
    Vector2 dashDirection;
    float timePassedFromLastDash;
    int vidas;

    [HideInInspector]
    public bool isDashing;
    [HideInInspector]
    public bool isShooting;
    [HideInInspector]
    public bool isStriking;
    [HideInInspector]
    public bool isChangingForm;
    [HideInInspector]
    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        timePassedFromLastDash = dashCooldown;
        vidas = 10;
    }

    private void Update()
    {

        //Movimiento 2D
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude );

        //Dash
        if (!isDashing && timePassedFromLastDash > dashCooldown && !isShooting && !isStriking && !isChangingForm && !isDead)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                timePassedFromLastDash = 0f;
                animator.speed = 0f;
                Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dashDirection = mPosition - transform.position;
                isDashing = true;
                StartCoroutine(dashCourutine());

            }
        }


        //Menu Pausa

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            panel.gameObject.SetActive(true);
        }
    }


    void FixedUpdate()
    {
        if (!isDashing && !isShooting && !isStriking && !isChangingForm && !isDead)
        {
            myRB.MovePosition(myRB.position + movement.normalized * speed * Time.fixedDeltaTime);
        }
        timePassedFromLastDash += Time.fixedDeltaTime;
    }

    IEnumerator dashCourutine()
    {


        if(dashDistance <= 0.001f)
        {
            yield break;
        }

        if (dashDuration <= 0.001f)
        {
            transform.position += (Vector3)(dashDirection.normalized * dashDistance);
            yield break;
        }


        float timePassed = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + dashDistance * (Vector3)dashDirection.normalized;


        RaycastHit2D hit = Physics2D.Raycast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position, Mathf.Infinity, limitLayer);
        Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.yellow,10f);


        if (hit.collider != null && !hit.collider.gameObject.CompareTag("Player"))
        {
            Vector2 distanciaImpacto = hit.point - (Vector2)transform.position;
            if (distanciaImpacto.magnitude >= dashDistance)
            {
                targetPosition = transform.position + dashDistance * (Vector3)dashDirection.normalized;
            }
            else
            {
                targetPosition = hit.point;
                targetPosition.z = -1;

            }
        }


        while (timePassed < dashDuration)
        {
            Vector3 nextPosition = Vector3.Lerp(startPosition, targetPosition, timePassed / dashDuration);
            transform.position = nextPosition;

            yield return null;
            timePassed += Time.deltaTime;
        }

        transform.position = targetPosition;
        isDashing = false;
        animator.speed = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead)
        {
            if (collision.gameObject.CompareTag("Enemy") && !isStriking)
            {

                contenedoresVidas[vidas - 1].enabled = false;
                vidas--;
                if (vidas == 0)
                {
                    isDead = true;
                    youLoseText.gameObject.SetActive(true);
                    tryAgainButton.gameObject.SetActive(true);
                    Time.timeScale = 0f;

                }

            }
        }

    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

}
