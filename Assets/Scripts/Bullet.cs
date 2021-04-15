using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float destroyTimer = 2f;
    public int damage = 25;
    public float explosionRadius = 0f;
    public GameObject impactEffect;

    private Transform shootLocation;

    public void Seek(Transform _target, float _speed, Transform _shootLocation)
    {
        target = _target;
        speed = _speed;
        shootLocation = _shootLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceByFrame = speed * Time.deltaTime;

        if(direction.magnitude <= distanceByFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceByFrame, Space.World);
    }

    void HitTarget()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(shootLocation.forward * speed, ForceMode.Impulse);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        Destroy(gameObject, destroyTimer);
        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }
}
