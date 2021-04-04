using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float destroyTimer = 2f;
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

        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(effectIns, 2f);
        
        Destroy(gameObject, destroyTimer);
    }
}
