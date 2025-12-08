using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer Electricity;
    [SerializeField] SpriteRenderer Light;
    [SerializeField] AudioSource audioSourceTrap;
    Collider2D TrapCollider;
    bool switchedOn;
    [SerializeField] float animationPeriodMin;
    [SerializeField] float animationPeriodMax;
    [SerializeField] float maxTimeOn;
    [SerializeField] float maxTimeOff;
    float switch_time_on;
    float switch_time_off;
    float animationTimerLight;
    float animationTimerElectricity;



    // Start is called before the first frame update
    void Start()
    {
        TrapCollider = GetComponent<Collider2D>();
        audioSourceTrap = GetComponent<AudioSource>();
        audioSourceTrap.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        //ElectricitySwitch(switchedOn);
        //if (switchedOn)
        //{
        //    switch_time_on -= Time.deltaTime;
        //}
        //if (switchedOn == false)
        //{
        //    switch_time_off -= Time.deltaTime;
        //}
        animationTimerElectricity -= Time.deltaTime;
        animationTimerLight -= Time.deltaTime;
        if (animationTimerElectricity < 0)
        {
            ElectricityAnimate();
        }
        //if (animationTimerLight < 0)
        //{
        //    LightAnimate();
        //}
    }

    void ElectricityAnimate()
    {
        if (Random.Range(0, 2) == 0)
        {
            Electricity.flipX = !Electricity.flipX;
        }
        else
        {
            Electricity.flipY = !Electricity.flipY;
        }
        animationTimerElectricity = Random.Range(animationPeriodMin, animationPeriodMax);
    }

    void LightAnimate()
    {
        if (Random.Range(0, 2) == 0)
        {
            Light.flipX = !Light.flipX;
        }
        animationTimerLight = Random.Range(animationPeriodMin, animationPeriodMax);
    }

    void ElectricitySwitch(bool on)
    {
        Light.enabled = on;
        Electricity.enabled = on;
        TrapCollider.enabled = on;
    }
}
