using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingGun : MonoBehaviour
{
    PlayerController player;

    public GameObject projectilePrefab;
    public Transform projectileSpawnpoint;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if(player.getPlayerInput().handleButtonInput("Fire1", InputMode.PRESS))
        {
            createProjectile(ScaleType.INCREASE);
        }
        else if(player.getPlayerInput().handleButtonInput("Fire2", InputMode.PRESS))
        {
            createProjectile(ScaleType.DECREASE);
        }
    }

    GameObject createProjectile(ScaleType mode)
    {
        GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnpoint.position, Quaternion.identity);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.direction = projectileSpawnpoint.forward;

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
