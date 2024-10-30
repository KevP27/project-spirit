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
    private Coroutine resumeRunningCoroutine;

    public Text ammoText;

    public Transform groundCheck;
    public LayerMask groundMask; // Assign this in the Inspector for ground detection
    public float groundDistance = 0.4f; // Radius for ground detection
    private bool isGrounded;

    void Start()
    {
        currentAmmo = maxAmmo;
        ammoText.text = currentAmmo.ToString();
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            if (isGrounded) // Only reload if on the ground
            {
                StartCoroutine(Reload());
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && isGrounded)
        {
            StartCoroutine(Reload());
        }

        // Handle firing
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            // Stop running animation immediately upon firing
            if (isRunning)
            {
                StopRunning();
            }

            // Reset and restart coroutine to resume running after delay
            if (resumeRunningCoroutine != null)
            {
                StopCoroutine(resumeRunningCoroutine);
            }
            resumeRunningCoroutine = StartCoroutine(ResumeRunningAfterDelay(0.5f));

            Shoot();
        }

        // Release recoil when firing stops
        if (Input.GetMouseButtonUp(0))
        {
            isRecoil = false;
            animator.SetBool("Recoil", isRecoil);

            FindObjectOfType<CamRecoil>().notCamRecoil();
        }

        // Running animation should only play if no firing and shift is held
        if (Input.GetKey("left shift") && !Input.GetMouseButton(0) && !isRunning && resumeRunningCoroutine == null)
        {
            isRunning = true;
            animator.SetBool("Running", true);
        }
    }

    IEnumerator Reload()
    {
        // Stop running if reloading starts
        isRunning = false;
        animator.SetBool("Running", isRunning);

        // Reset recoil state and stop recoil animation
        isRecoil = false;
        animator.SetBool("Recoil", isRecoil);

        // Trigger reloading animation
        isReloading = true;
        reload.Play();
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);

        // Wait for reload time to complete
        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        // Reset ammo count and isReloading flag
        currentAmmo = maxAmmo;
        isReloading = false;
        ammoText.text = currentAmmo.ToString();
    }

    void Shoot()
    {
        // Ensure running is off during shooting
        isRunning = false;
        animator.SetBool("Running", isRunning);

        isRecoil = !isRecoil;
        animator.SetBool("Recoil", isRecoil);

        dustExplosion.Play();
        muzzleFlash.Play();

        gunSound.Play();

        currentAmmo--;
        ammoText.text = currentAmmo.ToString();

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


    void StopRunning()
    {
        isRunning = false;
        animator.SetBool("Running", isRunning);
    }

    IEnumerator ResumeRunningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Only resume running if left shift is still pressed and no shooting
        if (Input.GetKey("left shift") && !Input.GetMouseButton(0))
        {
            isRunning = true;
            animator.SetBool("Running", true);
        }

        // Set coroutine to null so Update can re-enable running manually if needed
        resumeRunningCoroutine = null;
    }
}
