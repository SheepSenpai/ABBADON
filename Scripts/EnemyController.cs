using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    public LayerMask layerWall;

    public int health = 3;
    public GameObject explosion;
    private int currentHealth;

    public float playerRange = 10f;

    public Rigidbody2D theRB;
    public float moveSpeed;

    public bool shouldShoot;
    public float fireRate = .5f;
    private float shotCounter;
    public GameObject bullet;
    public Transform firePoint;

    public bool isMeele;
    public float attackRate = 2.5f;
    private float attackCounter;
    public int damageAmount;
    public GameObject hand;
    public Transform handPlace;

    public SpriteRenderer sp;
    public Sprite closedMouth;
    public Sprite openMouth;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 playerDirection = PlayerController.instance.transform.position - transform.position;
        Ray ray = new Ray(transform.position, PlayerController.instance.transform.position - transform.position);
        Debug.DrawRay(transform.position, PlayerController.instance.transform.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "MainCamera")
            {
                if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) <= 1 && PlayerController.instance.currentHealth > 0)
                {
                    theRB.velocity = Vector2.zero;
                    if (isMeele)
                    {
                        attackCounter -= Time.deltaTime;
                        if (attackCounter <= 0)
                        {
                            PlayerController.instance.takeDamage(damageAmount);
                            Instantiate(hand, handPlace.position, handPlace.rotation);
                            attackCounter = attackRate;
                        }
                    }
                }
                else if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) < playerRange && PlayerController.instance.currentHealth > 0)
                {
                    sp.sprite = openMouth;

                    theRB.velocity = playerDirection.normalized * moveSpeed;

                    if (shouldShoot)
                    {
                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            Instantiate(bullet, firePoint.position, firePoint.rotation);
                            shotCounter = fireRate;
                        }
                    }
                }
                else
                {
                    sp.sprite = closedMouth;
                    theRB.velocity = Vector2.zero;
                }
            }
            else if (hit.transform.tag != "MainCamera")
            {
                theRB.velocity = Vector2.zero;
                if (shouldShoot)
                {
                    sp.sprite = closedMouth;
                }
            }
            else
            {
                theRB.velocity = Vector2.zero;
                if (shouldShoot)
                {
                    sp.sprite = closedMouth;
                }
            }
        }
    }

    public void TakeDamage()
    {
        health--;
        
        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    public void TakeDamageAmount(int damage)
    {
        AudioController.instance.PlayRangedShot();
        health -= damage;

        if (health <= 0)
        {
            AudioController.instance.PlayEnemyDeath();
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
