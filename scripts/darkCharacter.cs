using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkCharacter : Enemy
{
   private Rigidbody2D rb;
    public float chaseRadius;
    public float attackRadius;
    public Animator anim;

    public Transform homePos;

    public Transform target;
    


    public int bossDamage = 20;

    public PlayerMovement playerMovement;

    

    

    
    void Start()
    {
        maxHealth = 300;
        currenthealth = 300;
        currentState = EnemyState.idle;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        
    }

    

    void FixedUpdate() 
    {
        //CheckDistance();
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius && playerMovement.warriorCurrentHealth >= 0)
        {
            
            fallowPlayer();
        }
        else if(Vector3.Distance(target.position, transform.position) > chaseRadius )
        {
            GoHome();
        }

        if (playerMovement.warriorCurrentHealth <= 0)
        {
            speed = 0.4f;
            attackRadius = 4f;
            anim.SetBool("attack", false);
            anim.SetBool("isMoving", true);
            GoHome();
        }
    }

    

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
    

    public void fallowPlayer()
    {
        
        
        speed = 0.5f;
        anim.SetBool("attack", false);
        anim.SetBool("isMoving", true);
        anim.SetFloat("moveX", (target.position.x - transform.position.x));
        anim.SetFloat("moveY", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        
        if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            anim.SetBool("isMoving", false);
            speed = 0f;
            anim.SetBool("attack", true);
            
            
        }
            
        
        

        
        
    }

    public void GoHome()
    {
        anim.SetFloat("moveX", (homePos.position.x - transform.position.x));
        anim.SetFloat("moveY", (homePos.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, homePos.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, homePos.position) == 0 )
        {
            anim.SetBool("isMoving", false);
        }
    }
    
    
    public void TakeDamage(int damage)
    {
        currenthealth -= damage;

        if (currenthealth <= 0)
        {
            playerMovement.warriorHealth += 100;
            StartCoroutine(DieCo());
        }

    }

    IEnumerator DieCo()
    {
        playerMovement.warriorCurrentHealth = playerMovement.warriorHealth;
        speed = 0f;
        attackRadius = 4f;
        
        anim.SetBool("dead", true);
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(5f);

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().TakeDamage(bossDamage);
        }

        
        
    }
}
