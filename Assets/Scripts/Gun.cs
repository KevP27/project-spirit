using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public Animator animator;

    public AudioSource gunSound;
    public AudioSource reload;

    private bool isRecoil = false;
    private bool isRunning = false;

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 70f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem dustExplosion;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public Text ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        ammoText.text = currentAmmo.ToString();
    }

    void OnEnable ()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            if (Input.GetKey("left shift"))
            {
                isRunning = false;
                animator.SetBool("Running", isRunning);
            }

            FindObjectOfType<CamRecoil>().camRecoil();

            Shoot();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isRecoil = false;

            animator.SetBool("Recoil", isRecoil);

            FindObjectOfType<CamRecoil>().notCamRecoil();
        }
    }

    IEnumerator Reload ()
    {
        isRecoil = false;

        animator.SetBool("Recoil", isRecoil);

        animator.SetBool("Running", isRunning);

        FindObjectOfType<CamRecoil>().notCamRecoil();

        reload.Play();

        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;

        ammoText.text = currentAmmo.ToString();
    }

    void Shoot()
    {
        isRecoil = !isRecoil;

        animator.SetBool("Recoil", isRecoil);

        dustExplosion.Play();
        muzzleFlash.Play();

        gunSound.Play();

        currentAmmo--;

        ammoText.text = currentAmmo.ToString();

        /*
        if (currentAmmo == 0)
        {
            ammoText.GetComponent<Text>().color = Color.red;
        }
        */

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
