using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MonoBehaviour
{
    public Animator animator;
    public Transform groundCheck;
    public LayerMask groundMask; // Assign this in the Inspector for ground detection
    public float groundDistance = 0.4f; // Radius for checking if on ground

    private bool isMoving;
    private bool isRunning = false;
    private bool inAir;
    //private bool isScoped = false;
    //private bool isFlipped = false;
    //public GameObject ScopeOverlay;
    //public GameObject weaponCamera;
    //public Camera mainCamera;

    //public float scopedFOV = 15f;
    //private float normalFOV;

    void WeaponRunning()
    {
        if (isMoving && !inAir && Input.GetKey(KeyCode.LeftShift))
        {
            // Start or keep running if moving on the ground and holding shift
            isRunning = true;
            animator.SetBool("Running", isRunning);
        }
        else
        {
            // Stop running if in the air or not holding shift
            isRunning = false;
            animator.SetBool("Running", isRunning);
        }
    }

    void Update()
    {
        // Check for movement input
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        isMoving = inputX != 0 || inputY != 0;

        // Check if player is in the air
        bool wasInAir = inAir;
        inAir = !Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If player has just landed and is moving with shift held, resume running
        if (wasInAir && !inAir && isMoving && Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            animator.SetBool("Running", isRunning);
        }

        // Update the running animation state
        WeaponRunning();
        /*
        if (Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;
            isFlipped = !isFlipped;

            animator.SetBool("Scope Flip", isFlipped);
            animator.SetBool("Scoped", isScoped);

            if (isScoped)
            {
                StartCoroutine(onScoped());
            }
            else
            {
                onUnScoped();
            }
        }
        */


        /*
        void onUnScoped()
        {
            ScopeOverlay.SetActive(false);
            weaponCamera.SetActive(true);

            mainCamera.fieldOfView = normalFOV;
        }

        IEnumerator onScoped()
        {
            yield return new WaitForSeconds(.01f);

            ScopeOverlay.SetActive(true);
            weaponCamera.SetActive(false);

            normalFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = scopedFOV;
        }
        */
    }
}