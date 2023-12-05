using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Christopher Mccort
//Spread Shot Cog PowerUp Pick Up Script
public class SpreadShotOrb : MonoBehaviour
{
    public AudioClip collectOrbClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.orbPowerUp = true;
            Destroy(gameObject);
            
            controller.PlaySound(collectOrbClip);
        }

    }
}
