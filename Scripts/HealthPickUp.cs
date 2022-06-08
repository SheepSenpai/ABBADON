using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthAmount = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerController.instance.currentHealth != PlayerController.instance.maxHealth)
            {
                AudioController.instance.PlayHealthPickUp();
                PlayerController.instance.currentHealth += healthAmount;
                PlayerController.instance.updateHealthUI();

                Destroy(gameObject);
            }
        }
    }
}
