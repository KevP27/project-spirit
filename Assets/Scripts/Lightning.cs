using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Animator animator;

    public AudioSource stun1;
    public AudioSource stun2;
    public AudioSource lightning;

    public AudioSource electricity;
    public AudioSource lightningWail;

    private bool isReloading = false;
    private bool isStun = false;
    private bool isRunning = false;

    public float fireRate = 0.01f;
    public float damage = 30f;
    public float range = 150f;
    public float impactForce = 700f;
    public float radius = 5f;

    private float nextTimeToFire = 0f;

    public Camera fpsCam;
    //public GameObject explosion;

    public ParticleSystem stunExplosion;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);

        electricity.Play();
        lightningWail.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            animator.SetBool("Running", isRunning);

            Cast();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isStun = false;

            animator.SetBool("Stun", isStun);
        }
    }

    void Cast()
    {
        stun1.Play();
        stun2.Play();
        lightning.Play();

        stunExplosion.Play();

        isStun = !isStun;

        animator.SetBool("Stun", isStun);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Collider[] colliders = Physics.OverlapSphere(hit.transform.position, radius);

            foreach (Collider nearbyEnemy in colliders)
            {
                Rigidbody rb = nearbyEnemy.GetComponent<Rigidbody>();
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                Target enemy = nearbyEnemy.GetComponent<Target>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    FindObjectOfType<HitStop>().Stop(0.1f);
                    StartCoroutine(WaitForSpawn());
                }
            }

            IEnumerator WaitForSpawn()
            {
                while (Time.timeScale != 1.0f)
                {
                    yield return null;
                }
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}