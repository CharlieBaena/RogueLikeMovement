using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField]
    float strikeDuration = 0.15f;
    [SerializeField]
    float strikeAngle = 30f;
    [SerializeField]
    float strikeCooldown = 0.5f;
    [SerializeField]
    GameObject gunGO;
    [SerializeField]
    float speedWithGun = 5f;


    float angle;
    float timePassedFromLastStrike;
    float inicialAngle;
    Vector2 direction;
    Vector3 startVectorAngle;
    PlayerMovement pM; 
    Animator animator;
    Animator fatherAnimator;
    bool isStriking;
    BoxCollider2D bladeCollider;




    // Start is called before the first frame update
    void Start()
    {
        pM = GetComponentInParent<PlayerMovement>();
        fatherAnimator = GetComponentInParent<Animator>();
        animator = GetComponent<Animator>();
        bladeCollider = gameObject.GetComponent<BoxCollider2D>();
        animator.speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStriking && timePassedFromLastStrike > strikeCooldown && !pM.isChangingForm && !pM.isDashing && !pM.isShooting)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                bladeCollider.enabled = true;
                timePassedFromLastStrike = 0f;
                pM.isStriking = true;
                fatherAnimator.speed = 0f;
                Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startVectorAngle = Camera.main.WorldToViewportPoint(transform.position) - Camera.main.ScreenToViewportPoint(Input.mousePosition);
                inicialAngle = Mathf.Atan2(-startVectorAngle.y, -startVectorAngle.x) * Mathf.Rad2Deg;
                isStriking = true;
                StartCoroutine(bladeCourutine());

            }

            if (Input.GetMouseButtonDown(1))
            {
                pM.isChangingForm = true;
                fatherAnimator.speed = 0f;
                animator.SetBool("changeToGun", true);
                StartCoroutine(waitChangeFormCourutine());
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isStriking)
        {
            direction = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            direction = Camera.main.WorldToViewportPoint(transform.position) - (Vector3)direction;
            angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        timePassedFromLastStrike += Time.fixedDeltaTime;
    }


    IEnumerator bladeCourutine()
    {


        isStriking = true;
        float timePassed = 0f;
        Vector3 startP = transform.position;
        Quaternion startPosition = Quaternion.AngleAxis(-strikeAngle + inicialAngle, Vector3.forward); 
        Quaternion targetPosition = Quaternion.AngleAxis(strikeAngle + inicialAngle, Vector3.forward);

        if(targetPosition.z < startPosition.z)
        {
            var temp = startPosition;
            startPosition = targetPosition;
            targetPosition = temp;
        }

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        /*print("startP " + startP);
        print("startR " + transform.rotation.eulerAngles);
        print("targetPosition " + targetPosition.eulerAngles);
        print("startPosition " + startPosition.eulerAngles);*/

        while (timePassed < strikeDuration)
        {
            Vector3 nextPosition = Vector3.Lerp(startPosition.eulerAngles, targetPosition.eulerAngles, timePassed / strikeDuration);
            /*if (targetPosition.z < startPosition.z)
            {
                transform.rotation = Quaternion.AngleAxis(nextPosition.z, -Vector3.forward);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(nextPosition.z, Vector3.forward);
            }*/

            transform.rotation = Quaternion.AngleAxis(nextPosition.z, Vector3.forward);
            transform.position = startP;
            //print("nextPosition " + nextPosition);

            yield return null;
            timePassed += Time.deltaTime;
        }

        transform.rotation = targetPosition;
        isStriking = false;
        fatherAnimator.speed = 1f;
        pM.isStriking = false;
        bladeCollider.enabled = false;
    }


    IEnumerator waitChangeFormCourutine()
    {
        animator.speed = 1f;
        pM.SetSpeed(speedWithGun);

        yield return new WaitForSeconds(1f);
        pM.isChangingForm = false;
        fatherAnimator.speed = 1f;
        gunGO.SetActive(true);
        gameObject.SetActive(false);
        //animator.SetBool("changeToGun", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
