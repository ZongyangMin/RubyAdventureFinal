using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPowerUp : MonoBehaviour
{
    public AudioClip collectLightningClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.maxStamina = 8.0f;
            controller.currentStamina = controller.maxStamina;
            Destroy(gameObject);
            
            controller.PlaySound(collectLightningClip);
        }

    }
}
