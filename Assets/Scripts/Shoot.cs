using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    GameObject proyectil;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    float cooldown;
    [SerializeField]
    GameObject bladeGO;
    [SerializeField]
    float speedWithBlade = 8f;
    [SerializeField]
    Image[] contenedoresBalas;
    [SerializeField]
    int bullets;

    Vector2 direction;
    float angle;
    float lastShootTime = 0;
    PlayerMovement pM;
    Animator animator;
    Animator fatherAnimator;
    int currentBullets;

    void Start()
    {
        pM = GetComponentInParent<PlayerMovement>();
        fatherAnimator = GetComponentInParent<Animator>();
        animator = GetComponent<Animator>();
        animator.speed = 1f;
        currentBullets = bullets;

    }

    // Update is called once per frame
    void Update()
    {
        GameObject bullet;

        if (!pM.isShooting && !pM.isStriking && !pM.isChangingForm && !pM.isDashing)
        {
            if (Input.GetMouseButtonDown(0) && currentBullets!=0)
            {
                if (Time.time > lastShootTime + cooldown || lastShootTime == 0)
                {
                    fatherAnimator.speed = 0f;
                    pM.isShooting = true;
                    currentBullets--;
                    contenedoresBalas[currentBullets].gameObject.SetActive(false);
                    bullet = Instantiate(proyectil, spawnPoint.position, Quaternion.identity);
                    bullet.GetComponent<Proyectile>().direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    lastShootTime = Time.time;
                    StartCoroutine(waitCourutine());
                }
            }


            if (Input.GetMouseButtonDown(1))
            {
                pM.isChangingForm = true;
                fatherAnimator.speed = 0f;
                animator.SetBool("changeToBlade", true);

                Reload();
                StartCoroutine(waitChangeFormCourutine());
            }

            if(currentBullets == 0)
            {
                StartCoroutine(waitRecargar());
            }
        }
    }

    private void FixedUpdate()
    {

        direction = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        direction = Camera.main.WorldToViewportPoint(transform.position) - (Vector3)direction;
        angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward );

    }

    IEnumerator waitCourutine()
    {
        yield return new WaitForSeconds(cooldown);
        pM.isShooting = false;
        fatherAnimator.speed = 1f;
    }

    IEnumerator waitChangeFormCourutine()
    {
        animator.speed = 1f;
        pM.SetSpeed(speedWithBlade);
        yield return new WaitForSeconds(1f);

        fatherAnimator.speed = 1f;
        bladeGO.SetActive(true);
        pM.isChangingForm = false;
        gameObject.SetActive(false);
    }

    IEnumerator waitRecargar()
    {
        yield return new WaitForSeconds(2f);
        Reload();
    }

    void Reload()
    {
        currentBullets = bullets;

        foreach (Image i in contenedoresBalas)
        {
            i.gameObject.SetActive(true);
        }
    }
}
