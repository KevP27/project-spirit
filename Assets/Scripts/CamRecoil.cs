using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRecoil : MonoBehaviour
{
    public Animator animator;

    private bool isCamRecoil = false;

    public void camRecoil()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCamRecoil = !isCamRecoil;
            animator.SetBool("camRecoil", isCamRecoil);
        }
    }

    public void notCamRecoil()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isCamRecoil = false;
            animator.SetBool("camRecoil", isCamRecoil);
        }
    }
}
