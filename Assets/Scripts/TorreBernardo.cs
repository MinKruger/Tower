using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreBernardo : MonoBehaviour
{
    private Transform target;
    public Transform partToRotate;

    public string enemyTag = "Enemy";

    [Header("Attributes")]
    public float range = 15f;
    public float turnSpeed = 10f;
    public float fireRate = 1f;
    private float fireCountdown = 0.5f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        //faz a torre seguir os inimigos imediatamente com um intervalo de 1.0 segundo
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    //Função para mirar nos inimigos
    void UpdateTarget()
    {
        //recuperar todos os inimigos com tag Enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if(nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        //Target lock on
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    //This function creates the bullet behavior
    public void Shoot()
    {
        GameObject tempBullet;
                tempBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Bullet bullet = tempBullet.GetComponent<Bullet>();
                if (bullet != null)
                    bullet.Seek(target, 50f, firePoint);
    }
}
