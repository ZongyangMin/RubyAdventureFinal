using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    private float speed = 4.0f;
    
    public int maxHealth = 5;

    public GameObject gameOverScreen;

    public GameObject healthBar;

    //Tanner Hedges
    //Get Game Over Music
    public GameObject gameOverMusic;

    //Tanner Hedges
    //Pause Game Screen
    public GameObject pauseScreen;

    //Tanner Hedges
    //Get Enter Music
    public GameObject enterMusic;

    //Christopher Mccort
    //Get NPC
    public GameObject npcFrog;

    //Christopher Mccort
    //Get Start Music
    public GameObject startMusic;

    //Zongyang Min
    //Get Task
    public GameObject task1;
    public GameObject task2;
    
    public GameObject projectilePrefab;
    
    public AudioClip throwSound;
    public AudioClip hitSound;
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    

    public ParticleSystem hitParticlePlayer;

    public ParticleSystem collectHealth;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;

    private bool isPaused = false;

    public bool orbPowerUp = false;

    //Tanner Hedges
    //Extra Protection Power Up
    public int extraProtection = 0;

    private float speedBoost = 8.0f;

    public float maxStamina = 5.0f;
    public float currentStamina;

    public GameObject spawnEvent;

    SpawnManager spawnManager;
    public GameObject scores;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        spawnManager = spawnEvent.GetComponent<SpawnManager>();
        
        currentHealth = maxHealth;

        currentStamina = maxStamina;

        audioSource = GetComponent<AudioSource>();

        gameOverMusic.SetActive(false);
        startMusic.SetActive(false);
        enterMusic.SetActive(true);
        npcFrog.SetActive(true);
        task1.SetActive(true);
        scores.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if(currentHealth <= 0)
        {
            GameOver();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        //Tanner Hedges
        //Activate Pause Screen
        if(!isPaused && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            PauseGame();
        }
        else if(isPaused && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            ResumeGame();
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                    spawnManager.gameStart = true;
                }
            }
        }

        //Christopher Mccort
        //Start the Game
        if(spawnManager.gameStart && Input.GetKeyDown(KeyCode.Return))
        {
            GameStart();
        }
    }
    
    void FixedUpdate()
    {
        //Zongyang Min
        //Speed Boost Power Up
        if(Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speedBoost * horizontal * Time.deltaTime;
            position.y = position.y + speedBoost * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
            currentStamina -= Time.fixedDeltaTime;
            UIStaminaBar.instance.SetValue(currentStamina / (float)maxStamina);
            Debug.Log(currentStamina);
        }
        else if(Input.GetKey(KeyCode.LeftShift) && currentStamina <= 0)
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }
        else
        {
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
            if(currentStamina<maxStamina)
            {
                currentStamina += Time.deltaTime/2;
                UIStaminaBar.instance.SetValue(currentStamina / (float)maxStamina); 
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            animator.SetTrigger("Hit");
            PlaySound(hitSound);
            Instantiate(hitParticlePlayer, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        else
        {
            Instantiate(collectHealth, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        //Tanner Hedges
        //Extra Protection Extra Hitpoints
        if(extraProtection>0 && amount < 0)
        {
            extraProtection--;
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        }
    }
    
    void Launch()
    {   
        //Christopher Mccort
        //Spread Shot Cog Option
        //Get New Angles
        Vector2 ySpreadPositive = new Vector2(0,0.58f);
        Vector2 ySpreadNegative = new Vector2(0,-0.58f);
        Vector2 xSpreadPositive = new Vector2(0.58f,0);
        Vector2 xSpreadNegative = new Vector2(-0.58f,0);
        //If Power Up Active
        if(orbPowerUp)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            GameObject top = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            GameObject bottom = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            Projectile projectileTop = top.GetComponent<Projectile>();
            Projectile projectileBottom = bottom.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            if(lookDirection.y == 0)
            {
                projectileTop.Launch(lookDirection + ySpreadPositive, 300);
                projectileBottom.Launch(lookDirection + ySpreadNegative, 300);
            }
            else
            {
                projectileTop.Launch(lookDirection + xSpreadPositive, 300);
                projectileBottom.Launch(lookDirection + xSpreadNegative, 300);
            }
        }
        else
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
        }
        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void GameOver()
    {
        healthBar.SetActive(false);
        enterMusic.SetActive(false);
        gameOverMusic.SetActive(true);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //Tanner Hedges
    //Pause Game Function
    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    //Tanner Hedges
    //Resume Game Function
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        isPaused = false;
    }

    //Christopher Mccort
    //Game Start Function
    public void GameStart()
    {
        spawnManager.gameInitiate = true;
        startMusic.SetActive(true);
        enterMusic.SetActive(false);
        npcFrog.SetActive(false);
        task1.SetActive(false);
        task2.SetActive(true);
        scores.SetActive(true);
        //Christopher Mccort
        //Spawn Power Ups
        Instantiate(spawnManager.spreadShotPowerUp, spawnManager.powerUpPosition1.transform.position, Quaternion.identity);
        Instantiate(spawnManager.speedBoostPowerUp, spawnManager.powerUpPosition2.transform.position, Quaternion.identity);
        Instantiate(spawnManager.extraProtectPowerUp, spawnManager.powerUpPosition3.transform.position, Quaternion.identity);
        Instantiate(spawnManager.healthRestore, spawnManager.powerUpPosition4.transform.position, Quaternion.identity);
        Instantiate(spawnManager.healthRestore, spawnManager.powerUpPosition5.transform.position, Quaternion.identity);
        Instantiate(spawnManager.healthRestore, spawnManager.powerUpPosition6.transform.position, Quaternion.identity);
        Instantiate(spawnManager.extraPointPowerUp, spawnManager.powerUpPosition7.transform.position, Quaternion.identity);
    }
}
