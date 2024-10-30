using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    private Charge charge;
    private Image barImage;

    public GameObject lightning;

    private void Awake()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();

        charge = new Charge();

    }

    private void Update()
    {
        charge.Update();

        barImage.fillAmount = charge.GetChargeNormalized();

        if (lightning.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                charge.TrySpendCharge(100);
            }
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {

        }

        if (Input.GetMouseButtonUp(0))
        {

        }
        
    }
    */
}

public class Charge
{
    public const int Charge_Max = 100;

    private float chargeAmount;
    private float chargeRegenAmount;

    public Charge()
    {
        chargeAmount = 100;
        chargeRegenAmount = 5f;
    }

    public void Update()
    {
        chargeAmount += chargeRegenAmount * Time.deltaTime;
        chargeAmount = Mathf.Clamp(chargeAmount, 0f, Charge_Max);
    }

    public void TrySpendCharge(int amount)
    {
        if (chargeAmount >= amount)
        {
            chargeAmount -= amount;
        }
    }

    public float GetChargeNormalized()
    {
        return chargeAmount / Charge_Max;
    }
}
