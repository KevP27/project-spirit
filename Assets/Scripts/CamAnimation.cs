using System.Collections;
using UnityEngine;

public class CamAnimation : MonoBehaviour
{
    public AudioSource footstep;
    public AudioSource fastStep;

    //public CharacterController playerController;
    public Transform groundCheck;
    public Collider playerCollider;
    public Animation anim; 
    private bool isMoving;
    private bool onGround;

    private bool left;
    private bool right;

    public LayerMask groundMask; 
    public float groundDistance = 0.4f; 

    void CameraAnimations()
    {
        if (onGround) // Check if player is on the ground
        {
            if (isMoving)
            {
                if (Input.GetKey("left shift"))
                {
                    if (left)
                    {
                        if (!anim.isPlaying)
                        {
                            anim.Play("Head Bob Left Run");
                            fastStep.Play();
                            left = false;
                            right = true;
                        }
                    }
                    if (right)
                    {
                        if (!anim.isPlaying)
                        {
                            anim.Play("Head Bob Right Run");
                            fastStep.Play();
                            right = false;
                            left = true;
                        }
                    }
                }
                else
                {
                    if (left)
                    {
                        if (!anim.isPlaying)
                        {
                            anim.Play("Head Bob Left");
                            footstep.Play();
                            left = false;
                            right = true;
                        }
                    }
                    if (right)
                    {
                        if (!anim.isPlaying)
                        {
                            anim.Play("Head Bob Right");
                            footstep.Play();
                            right = false;
                            left = true;
                        }
                    }
                }
            }
        }
    }

    void Start()
    {
        left = true;
        right = false;
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        isMoving = inputX != 0 || inputY != 0;

        // Check if the player is on the ground
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        CameraAnimations();
    }
}
