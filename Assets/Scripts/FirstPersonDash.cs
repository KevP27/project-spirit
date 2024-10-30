using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonDash : MonoBehaviour
{
    public Animator animator;

    private bool isRunning = false;

    public Vector3 moveDirection;

    public AudioSource dash;
    public AudioSource dash1;

    public const float maxDashTime = 1.0f;
    public float dashDistance = 10;
    public float dashStoppingSpeed = 0.1f;
    float currentDashTime = maxDashTime;
    float dashSpeed = 6;
    CharacterController controller;

    public float dashRate = 0.5f;
    private float nextTimeToFire = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / dashRate;
            currentDashTime = 0;

            dash.Play();
            dash1.Play();

            isRunning = false;
            animator.SetBool("Running", isRunning);
        }
        if (currentDashTime < maxDashTime)
        {
            moveDirection = transform.forward * dashDistance;
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        controller.Move(moveDirection * Time.deltaTime * dashSpeed);
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonDash : MonoBehaviour
{
    public bool isDashing;

    private int dashAttempts;
    private float dashStartTime;

    FirstPersonMovement PlayerMovement;
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        playerController.GetComponent<PlayerMovement>();
        characterController.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleDash();
    }

    void HandleDash()
    {
        bool isTryingToDash = Input.GetKeyDown(KeyCode.E);

        if (isTryingToDash && !isDashing)
        {
            OnStartDash();
        }

        if (isDashing)
        {
            if (Time.time - dashStartTime <= 0.4f)
            {
                if (playerController.movementVector.Equals(Vector3.zero))
                {
                    characterController.Move(transform.forward * 30f * Time.deltaTime);
                }
                else
                {
                    characterController.Move(playerController.movmenetVector.normalized * 30f * Time.deltaTime);
                }
            }
        }
    }

    void OnStartDash()
    {

    }

    void OnEndDash()
    {

    }
}
*/
