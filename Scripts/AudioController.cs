using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource ammo, enemyDeath, enemyrangedShot, enemymeleeShot, gunShot, health, playerhurt, playerdeath, barrelexplosion;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAmmoPickUp()
    {
        ammo.Stop();
        ammo.Play();
    }

    public void PlayHealthPickUp()
    {
        health.Stop();
        health.Play();
    }

    public void PlayEnemyDeath()
    {
        enemyDeath.Stop();
        enemyDeath.Play();
    }

    public void PlayRangedShot()
    {
        enemyrangedShot.Stop();
        enemyrangedShot.Play();
    }

    public void PlayMeleeShot()
    {
        enemymeleeShot.Stop();
        enemymeleeShot.Play();
    }

    public void PlayPlayerShoot()
    {
        gunShot.Stop();
        gunShot.Play();
    }

    public void PlayPlayerHurt()
    {
        playerhurt.Stop();
        playerhurt.Play();
    }

    public void PlayPlayerDeath()
    {
        playerdeath.Stop();
        playerdeath.Play();
    }

    public void PlayBarrelExplosion()
    {
        barrelexplosion.Stop();
        barrelexplosion.Play();
    }
}
