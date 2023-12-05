using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tanner Hedges 
//Basic Penguin Left and Right Movement and Animation
public class Penguin_Intro : MonoBehaviour
{
    float horizontal;
    float inputRaw;
    private SpriteRenderer      m_SR;
    public float speed = 3.0f;
    public int                 m_facingDirection = 1;
    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        inputRaw = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(horizontal, 0);
        animator.SetFloat("Speed", move.magnitude);
    }

    void FixedUpdate()
    {
        //Tanner Hedges
        //get the correct direction for the penguin animation
        if (inputRaw > 0)
        {
            m_SR.flipX = false;
            m_facingDirection = 1;
        } 
        else if (inputRaw < 0)
        {
            m_SR.flipX = true;
            m_facingDirection = -1;
        }
        //Tanner Hedges 
        //penguin movement
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
}
