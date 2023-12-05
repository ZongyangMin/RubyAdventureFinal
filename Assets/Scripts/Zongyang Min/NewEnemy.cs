using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEngine.UI;
public class NewEnemy : MonoBehaviour
{
    public float speed = 0;
    public float health = 1;
    Rigidbody2D rb;
    SkeletonAnimation sa;
    Transform player;
    public bool isFlipped = false;
    private int stat;
    public AudioClip takeHit;
    private int statChange;
    private bool isHit = false;
    AudioSource audioSource;

    public int MonsterScore = 0;
    GameObject gameManager;
    GameCount gameCount;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stat = 0;
        statChange = stat;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sa = GetComponent<SkeletonAnimation>();
        gameManager = GameObject.FindGameObjectWithTag("MainCamera");
        gameCount = gameManager.GetComponent<GameCount>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(isHit)
        {
            StartCoroutine(delayTime());
        }
        else if(stat == 4)
        {
            return;
        }
        else if(stat != statChange)
        {
            changeAnimation();
            statChange = stat;
        }
        checkStatus();

        if(health<=0 && stat!=4)
        {
            Defeat();
        }
        followPlayer();
    }

    //Zongyang Min
    //New Enemy Follow Player Function
    public void followPlayer()
    {
        //Zongyang Min
        //Get Target Position
        Vector2 target = new Vector2(player.position.x, player.position.y);
        //Zongyang Min
        //Move Toward Target Position
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    //Zongyang Min
    //Check Direction Status
    public void checkStatus()
    {
        Vector2 direction = player.transform.position - rb.transform.position;
        if(isHit)
        {
            return;
        }
        else if(Mathf.Abs(direction.x)>Mathf.Abs(direction.y))
        {
            stat = 1;
        }
        else
        {
            if(direction.y<0)
            {
                stat = 2;
            }
            else
            {
                stat = 3;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(1.0f);
        isHit = false;
    }

    //Zongyang Min
    //Change New Enemy Animation Based on Direction Status Change
    public void changeAnimation()
    {
        Vector3 flipped = transform.localScale;
        Vector2 direction = player.transform.position - rb.transform.position;
        if(stat == 1)
        {
            sa.skeleton.SetSkin("Side");
            sa.skeleton.SetSlotsToSetupPose();
            sa.AnimationState.SetAnimation(0, "Side_Walk", true);
            if(direction.x<0 && !isFlipped)
            {
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            }
            else if(direction.x > 0 && isFlipped)
            {
            transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
            }
        }
        else if(stat == 2)
        {
            sa.skeleton.SetSkin("Front");
            sa.skeleton.SetSlotsToSetupPose();
            sa.AnimationState.SetAnimation(0, "Front_Walk", true);
        }
        else if(stat == 3)
        {
            sa.skeleton.SetSkin("Back");
            sa.skeleton.SetSlotsToSetupPose();
            sa.AnimationState.SetAnimation(0, "Back_Walk", true);
        }
    }

    public void Damage()
    {
        health--;
        Vector3 flipped = transform.localScale;
        Vector2 direction = player.transform.position - rb.transform.position;
        stat = 5;
        if(Mathf.Abs(direction.x)>Mathf.Abs(direction.y))
        {
            sa.skeleton.SetSkin("Side");
            sa.skeleton.SetSlotsToSetupPose();
            sa.AnimationState.SetAnimation(0, "Side_Hurt", false);
            if(direction.x<0 && !isFlipped)
            {
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
            }
            else if(direction.x > 0 && isFlipped)
            {
            transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
            }
        }
        else
        {
            if(direction.y<0)
            {
                sa.skeleton.SetSkin("Front");
                sa.skeleton.SetSlotsToSetupPose();
                sa.AnimationState.SetAnimation(0, "Front_Hurt", false);
            }
            else
            {
                sa.skeleton.SetSkin("Back");
                sa.skeleton.SetSlotsToSetupPose();
                sa.AnimationState.SetAnimation(0, "Back_Hurt", false);
            }
        }
        PlaySound(takeHit);
        isHit = true;
    }

    public void Defeat()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        stat = 4;
        sa.skeleton.SetSkin("Front");
        sa.skeleton.SetSlotsToSetupPose();
        sa.AnimationState.SetAnimation(0, "Front_Death", false);
        gameCount.score += MonsterScore;
        gameCount.score += gameCount.extraScore;
        Destroy(gameObject, 2.0f);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
