using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject explosion;
    public int health;
    public int damageAmount;
    public int damageToEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            AudioController.instance.PlayBarrelExplosion();
            if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) <= 3 && PlayerController.instance.currentHealth > 0)
            {
                PlayerController.instance.takeDamage(damageAmount);
            }
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
