using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TorreProject
{
    public class Torre : MonoBehaviour
    {
        private Transform target;
        public Transform partToRotate;

        public string enemyTag = "Enemy";

        [Header("Unity-chan Attributes")]
        public float range = 15f;
        public float turnSpeed = 10f;
        public float fireRate = 1f;
        private float fireCountdown = 0.5f;

        #region Arma

        [Header("Gun Prefab Refrences")]
        public GameObject bulletPrefab;
        public Transform firePoint;
        public GameObject casingPrefab;
        public GameObject muzzleFlashPrefab;

        [Header("Gun Location Refrences")]
        //[SerializeField] private Animator gunAnimator;
        [SerializeField] private Transform barrelLocation;
        [SerializeField] private Transform casingExitLocation;

        [Header("Gun Settings")]
        [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
        [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
        [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            //faz a torre seguir os inimigos imediatamente com um intervalo de 1.0 segundo
            InvokeRepeating("UpdateTarget", 0f, 1f);

            if (barrelLocation == null)
                barrelLocation = transform;

            //if (gunAnimator == null)
            //    gunAnimator = GetComponentInChildren<Animator>();
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

            ////If you want a different input, change it here
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    //Calls animation on the gun that has the relevant animation events that will fire
            //    gunAnimator.SetTrigger("Fire");
            //}
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }

        //This function creates the bullet behavior
        public void Shoot()
        {
            //cancels if there's no bullet prefeb
            if (!bulletPrefab)
                return;

            if (muzzleFlashPrefab)
            {
                //Create the muzzle flash
                GameObject tempFlash;
                tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

                //Destroy the muzzle flash effect
                Destroy(tempFlash, destroyTimer);

                CasingRelease();

                // Create a bullet and add force on it in direction of the barrel
                GameObject tempBullet;
                tempBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
                Bullet bullet = tempBullet.GetComponent<Bullet>();
                if (bullet != null)
                    bullet.Seek(target, shotPower, barrelLocation);
            }
            else
            {
                // Create a bullet and add force on it in direction of the barrel
                GameObject tempBullet;
                tempBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
                Bullet bullet = tempBullet.GetComponent<Bullet>();
                if (bullet != null)
                    bullet.Seek(target, shotPower, barrelLocation);
            }
        }


        //This function creates a casing at the ejection slot
        void CasingRelease()
        {
            //Cancels function if ejection slot hasn't been set or there's no casing
            if (!casingExitLocation || !casingPrefab)
            { return; }

            //Create the casing
            GameObject tempCasing;
            tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
            //Add force on casing to push it out
            tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
            //Add torque to make casing spin in random direction
            tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

            //Destroy casing after X seconds
            Destroy(tempCasing, destroyTimer);
        }
    }
}
