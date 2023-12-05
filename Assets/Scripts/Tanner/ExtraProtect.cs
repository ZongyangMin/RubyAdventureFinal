using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tanner Hedges
//Extra Protect Pickup Script
public class ExtraProtect : MonoBehaviour
{
    public AudioClip collectStarClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.extraProtection = 2;
            Destroy(gameObject);
            
            controller.PlaySound(collectStarClip);
        }

    }
}
