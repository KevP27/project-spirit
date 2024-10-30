using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MonoBehaviour
{
    public Animator animator;

    //private bool isScoped = false;
    //private bool isFlipped = false;
    private bool isRunning = false;


    public Transform groundCheck;

    private bool isMoving;
    private bool inAir;
    //public GameObject ScopeOverlay;
    //public GameObject weaponCamera;
    //public Camera mainCamera;

    //public float scopedFOV = 15f;
    //private float normalFOV;

    void WeaponRunning()
    {
        if (isMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = !isRunning;

                animator.SetBool("Running", isRunning);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;

                animator.SetBool("Running", isRunning);
            }
        }

        if (inAir)
        {
            if (Input.GetKey("left shift"))
            {
                isRunning = false;

                animator.SetBool("Running", isRunning);
            }
        }
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal"); //Keyboard input to determine if player is moving
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
        {
            isMoving = true;
        }
        else if (inputX == 0 && inputY == 0)
        {
            isMoving = false;
        }

        if(groundCheck.position.y > 1f)
        {
            inAir = true;
        }

        if (groundCheck.position.y < 1f)
        {
            inAir = false;
        }


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