using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public ParticleSystem smokeEffect;
    Rigidbody2D rb;
    float timer;
    bool broken = true;
    
    Animator animator;

    GameCount gameCount;

    //Zongyang Min
    //Get Player
    Transform player;

    GameObject manager;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = 2.5f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Zongyang Min
        //Get Player Transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
        manager = GameObject.FindGameObjectWithTag("MainCamera");
        gameCount = manager.GetComponent<GameCount>();
    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        followPlayer();
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    
    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        GetComponent<Rigidbody2D>().simulated = false;
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");
        gameCount.score += 500;
        gameCount.score += gameCount.extraScore;
        smokeEffect.Stop();
        Destroy(gameObject, 2.0f);
    }

    public void followPlayer()
    {
        //Zongyang Min
        //Get Target Position
        Vector2 target = new Vector2(player.position.x, player.position.y);
        //Zongyang Min
        //Get Target Direction
        Vector2 direction = player.transform.position - rb.transform.position;
        direction.Normalize();
        //Zongyang Min
        //Look at Target
        animator.SetFloat("Move X", direction.x);
        animator.SetFloat("Move Y", direction.y);
        //Zongyang Min
        //Move Toward Target Position
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
}
