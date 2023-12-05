using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraPoints : MonoBehaviour
{
    //Tanner Hedges
    //Extra Points Power Up
    GameObject gameManager;
    GameCount gameCount;
    public AudioClip collectOreClip;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("MainCamera");
        gameCount = gameManager.GetComponent<GameCount>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
            gameCount.extraScore = 200;
            Destroy(gameObject);
            
            controller.PlaySound(collectOreClip);
        }

    }

}
