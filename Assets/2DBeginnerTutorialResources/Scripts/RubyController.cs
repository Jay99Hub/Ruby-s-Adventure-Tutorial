using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public TextMeshProUGUI cogs;
    private int cogsValue = 4;

    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;
    public AudioClip frogSound;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public GameObject healthIncrease;
    public GameObject healthDecrease;
    public TextMeshProUGUI scoreText;
    public GameObject collectableEffect;
    public GameObject clockObject;
    public int score = 0;
    public TextMeshProUGUI  gameOverText;
    public TextMeshProUGUI  winGameText;
    public TextMeshProUGUI timeStartText;

    bool gameOver = false;
    bool timeStop = false;
    bool soundPlayed = false;
    bool buttonPressed = false;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        cogs.text = "Cogs: " + cogsValue.ToString();

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
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

        if (Input.GetKeyDown(KeyCode.C))
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
                    PlaySound(frogSound);
                }
            }
        }

        if (timeStop == true)
        {
           gameOverText.gameObject.SetActive(true);
            gameOverText.text = "You lost! Press R to Restart!";
            if (!soundPlayed)
            {
                PlaySound(loseSound);
                soundPlayed = true;
            }
            speed = 0.0f;
            gameOver = true;
             if (Input.GetKey(KeyCode.R))
        {

            if (gameOver == true)

            {

              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene

            }

        }
        }


        if (currentHealth == 0)
        {
           gameOverText.gameObject.SetActive(true);
            gameOverText.text = "You lost! Press R to Restart!";
            if (!soundPlayed)
            {
                PlaySound(loseSound);
                soundPlayed = true;
            }
            speed = 0.0f;
            gameOver = true;
             if (Input.GetKey(KeyCode.R))
        {

            if (gameOver == true)

            {

              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene

            }

        }
        }

         if(score == 4){
            winGameText.gameObject.SetActive(true);
            if (!soundPlayed)
            {
                PlaySound(winSound);
                soundPlayed = true;
            }
            
            winGameText.text = "You Win! Game Created By Group 27";
            Time.timeScale = 0.0f;


        }

        if (buttonPressed == false)
        {
            
            timeStartText.gameObject.SetActive(true);
            timeStartText.text = "Play with a timer? Press T, if not press N!";
            speed = 0.0f;
            if (Input.GetKeyDown(KeyCode.T))
            {
                clockObject.gameObject.SetActive(true);
                timeStartText.gameObject.SetActive(false);
                buttonPressed = true;
                speed = 3.0f;
                EventManager.OnTimerStart();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                clockObject.gameObject.SetActive(false);
                buttonPressed = true;
                timeStartText.gameObject.SetActive(false);
                speed = 3.0f;
            }
        }

    }
    
    private void OnEnable()
    {
        EventManager.TimerStop += EventManagerOnTimerStop;
    }

    private void EventManagerOnTimerStop()
    {
        timeStop = true;
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject projectileObject = Instantiate(healthDecrease, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            animator.SetTrigger("Hit");
            PlaySound(hitSound);
        }
        if(amount > 0)
        {
            GameObject projectileObject = Instantiate(healthIncrease, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void ChangeScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = "Fixed Robots: " + score.ToString();
       
       
    }

    public void ChangeCogs(int cogsAmount)
    {
        cogsValue += cogsAmount;
        cogs.text = "Cogs: " + cogsValue.ToString();
        GameObject projectileObject = Instantiate(collectableEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
         
    }

    void Launch()
    {
        if(cogsValue > 0){
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
        cogsValue--;
        cogs.text = "Cogs: " + cogsValue.ToString();
    
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}