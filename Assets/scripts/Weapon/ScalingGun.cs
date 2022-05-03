using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingGun : MonoBehaviour
{
    PlayerController player;

    [SerializeField] AudioSource src;
    [SerializeField] AudioClip _enlarge, _shrink;
    public GameObject projectilePrefab;
    public Transform projectileSpawnpoint;
    public float projectileForce = 100;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        AudioSource src = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(player.getPlayerInput().handleButtonInput("Fire1", InputMode.PRESS))
        {
            src.clip = _enlarge;
            src.PlayOneShot(_enlarge);
            createProjectile(ScaleType.INCREASE);
            
        }
        else if(player.getPlayerInput().handleButtonInput("Fire2", InputMode.PRESS))
        {
            src.clip = _shrink;
            src.PlayOneShot(_shrink);
            createProjectile(ScaleType.DECREASE);
        }
    }

    GameObject createProjectile(ScaleType mode)
    {
        GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnpoint.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.direction = Camera.main.transform.forward;
        projectile.force = projectileForce;

        switch(mode)
        {
            case ScaleType.DECREASE:
                // Need to handle how scaling is done, e.g. Can the player change the scale, or is the scaling static?
                projectile.massChange = -projectile.massChange;
                projectile.scaleChange = new Vector3(-0.1f, -0.1f, -0.1f);
                break;
            case ScaleType.INCREASE:
                projectile.massChange = +projectile.massChange;
                projectile.scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
                break;
        }

        return projectileGO;
    }
}
