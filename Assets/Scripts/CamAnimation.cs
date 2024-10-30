using System.Collections;
using UnityEngine;

public class CamAnimation : MonoBehaviour
{
    public AudioSource footstep;
    public AudioSource fastStep;

    //public CharacterController playerController;
    public Transform groundCheck;
    public Animation anim; //Empty GameObject's animation component
    private bool isMoving;

    private bool left;
    private bool right;

    void CameraAnimations()
    {
        if (groundCheck.position.y < 1.5f)
        {
            if (isMoving == true)
            {
                if (Input.GetKey("left shift"))
                {
                    if (left == true)
                    {
                        if (!anim.isPlaying)
                        {//Waits until no animation is playing to play the next
                            anim.Play("Head Bob Left Run");
                            fastStep.Play();
                            left = false;
                            right = true;
                        }
                    }
                    if (right == true)
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
                    if (left == true)
                    {
                        if (!anim.isPlaying)
                        {//Waits until no animation is playing to play the next
                            anim.Play("Head Bob Left");
                            footstep.Play();
                            left = false;
                            right = true;
                        }
                    }
                    if (right == true)
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
    { //First step in a new scene/life/etc. will be "walkLeft"
        left = true;
        right = false;
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

        CameraAnimations();

    }
}