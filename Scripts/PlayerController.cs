using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float nextProjectile = 0f;      
    public float projectileCoolDown = 0.01f;

    public static PlayerController instance;

    public Rigidbody2D theRB;

    public float moveSpeed = 5f;


    private Vector2 moveInput;
    private Vector2 mouseInput;

    private float maxAngle = 160;
    float minAngle = 10;

    public float mouseSensitivity = 1f;

    public Camera viewCam;

    public GameObject bulletImpact;
    public int currentAmmo;

    public Animator gunAnim;
    public Animator walkAnim;

    public int currentHealth;
    public int maxHealth = 100;
    public GameObject deathScreen;
    private bool hasDied;

    public Text healthText, ammoText;
    public GameObject damageScreen;

    private float nextShot = 0.0f;
    private float shotBuffer = 0.3f;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString() + "%";

        ammoText.text = currentAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDied)
        {
            //player movement
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            Vector3 moveHorizontal = transform.up * -moveInput.x;

            Vector3 moveVertical = transform.right * moveInput.y;

            theRB.velocity = (moveHorizontal + moveVertical) * moveSpeed;

            //player sight
            mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);

            viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));

            Vector3 RotAmount = viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f);

            viewCam.transform.localRotation = Quaternion.Euler(RotAmount.x, Mathf.Clamp(RotAmount.y, minAngle, maxAngle), RotAmount.z);

            if(moveInput != Vector2.zero)
            {
                walkAnim.SetBool("IsMoving", true);
            }
            else
            {
                walkAnim.SetBool("IsMoving", false);
            }

            if (Input.GetMouseButtonDown(0) && Time.time > nextShot)
            {
                if (currentAmmo > 0)
                {
                    AudioController.instance.PlayPlayerShoot();
                    Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("I'm looking at " + hit.transform.name);
                        nextShot = Time.time + shotBuffer;


                        if (hit.transform.tag == "Enemy")
                        { if (Vector2.Distance(viewCam.transform.position, EnemyController.FindObjectOfType<GameObject>().transform.position) <= 20)
                            {
                                Instantiate(bulletImpact, hit.point, transform.rotation);
                                hit.transform.parent.GetComponent<EnemyController>().TakeDamageAmount(1);
                            }
                            nextShot = Time.time + shotBuffer;

                        }
                        else if (hit.transform.tag == "Barrel")
                        {
                            hit.transform.parent.GetComponent<ExplosiveBarrel>().TakeDamage();
                            Instantiate(bulletImpact, hit.point, transform.rotation);
                            nextShot = Time.time + shotBuffer;
                        }
                        Instantiate(bulletImpact, hit.point, transform.rotation);
                    }
                    else
                    {
                        nextShot = Time.time + shotBuffer;
                    }
                    currentAmmo--;
                    gunAnim.SetTrigger("Shoot");
                    updateAmmoUI();
                }
            }
        }
    }

    public void takeDamage(int damageAmount)
    {
        AudioController.instance.PlayPlayerHurt();
        GameObject damageClone = Instantiate(damageScreen, damageScreen.transform.position, damageScreen.transform.rotation) as GameObject;
        GameObject parentObject = GameObject.Find("UI Canvas");
        damageClone.transform.parent = parentObject.transform;
        damageClone.SetActive(true);
        Destroy(damageClone, 0.1f);


        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            AudioController.instance.PlayPlayerDeath();
            walkAnim.SetBool("IsMoving", false);
            theRB.velocity = Vector2.zero;
            Cursor.lockState = CursorLockMode.Confined;
            deathScreen.SetActive(true);
            hasDied = true;
            currentHealth = 0;
        }

        healthText.text = currentHealth.ToString() + "%";
    }

    public void healHealth(int healAmount)
    {
        currentHealth -= healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthText.text = currentHealth.ToString() + "%";
    }

    public void updateAmmoUI()
    {
        ammoText.text = currentAmmo.ToString();
    }

    public void updateHealthUI()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthText.text = currentHealth.ToString() + "%";
    }
}
