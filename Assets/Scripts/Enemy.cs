using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    public float startHealth = 100;
    private float health;

    private AudioSource EnemyAudio;
    public AudioClip HitSound;
    public AudioClip DieSound;

    private Transform target;
    private int waypointIndex = 0;

    public Image healthBar;
    

    // Start is called before the first frame update
    void Start()
    {   
        health = startHealth;
        target = Waypoints.waypoints[0];  
        EnemyAudio = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            EnemyAudio.PlayOneShot(DieSound, 0.5f);
            Die();
        }
        else
        {
            EnemyAudio.PlayOneShot(HitSound, 0.5f);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if(waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Waypoints.waypoints[waypointIndex];
    }
}
