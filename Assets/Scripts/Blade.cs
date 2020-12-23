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
            direction = Camera.main.WorldToViewportPoint(transform.position) - Camera.main.ScreenToViewportPoint(Input.mousePosition);
            angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        timePassedFromLastStrike += Time.fixedDeltaTime;
    }


    IEnumerator bladeCourutine()
    {

        float timePassed = 0f;
        Vector3 startP = transform.position;
        Quaternion startPosition = Quaternion.AngleAxis(-strikeAngle + inicialAngle, Vector3.forward); 
        Quaternion targetPosition = Quaternion.AngleAxis(strikeAngle + inicialAngle, Vector3.forward);



        if (startPosition.eulerAngles.z > targetPosition.eulerAngles.z) //En caso de que la rotacion tenga que pasar por 0
        {
            float timeFixed1 = (strikeDuration * (360 - startPosition.eulerAngles.z)) / (strikeAngle * 2);  //Calculamos que timpo necesita cada arco
            float timeFixed2 = strikeDuration - timeFixed1;
            bool llegadoAlFinal = false;
            Vector3 nextPosition;

            while (timePassed < strikeDuration)
            {


                if (!llegadoAlFinal)
                {
                    nextPosition = Vector3.Lerp(startPosition.eulerAngles, new Vector3(0, 0, 360), timePassed / timeFixed1);
                }
                else
                {
                    nextPosition = Vector3.Lerp(new Vector3(0, 0, 0.1f), targetPosition.eulerAngles, (timePassed-timeFixed1) / timeFixed2);
                }

                if (nextPosition.z == 360)
                {
                    llegadoAlFinal = true;
                }

                transform.rotation = Quaternion.AngleAxis(nextPosition.z, Vector3.forward);
                transform.position = startP;


                yield return null;
                timePassed += Time.deltaTime;
            }

        }
        else //Rotacion normal
        {
        
            while (timePassed < strikeDuration)
            {
                Vector3 nextPosition = Vector3.Lerp(startPosition.eulerAngles, targetPosition.eulerAngles, timePassed / strikeDuration);
                transform.rotation = Quaternion.AngleAxis(nextPosition.z, Vector3.forward);
                transform.position = startP;

                yield return null;
                timePassed += Time.deltaTime;
            }
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
