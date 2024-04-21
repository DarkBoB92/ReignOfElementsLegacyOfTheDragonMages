using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using SpellType;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy General Variables")]
    [SerializeField] protected float rangeOfSight;
    [SerializeField] protected float rangeOfAttack;
    [SerializeField] protected float patrolTime, chasingTime, attackDelay, timeDelay;       
    [SerializeField] protected float speed;
    protected Vector2 directionToMove; // directionToRayCast;
    protected Rigidbody2D rb;
    protected Vector2 currentPos;
    protected Vector2 lastPos;
    protected Animator animator;


    [Header("Enemy Type")]
    [SerializeField] protected bool melee;
    [SerializeField] protected bool ranged;

    [Header("Enemy State")]
    [SerializeField] protected State currentState;
    [SerializeField] protected bool attacking;

    [Header("Player Related Variables")]
    protected GameObject player;
    protected Vector2 playerPos;
    

    [Header("Patrol Position Variables")]
    [SerializeField] protected Transform[] targetPatrolPositions;
    protected int targetCounter = 0;  
    
    //[SerializeField] protected int horizontal, vertical; //Thinking to use simple movement for patrol and change direction after collision
    
    [Header("Health References")]
    protected Health playerHealth, ownHealth;

    [Header("Drop Item")]
    [SerializeField] protected GameObject[] droppingItems;

    protected enum State {Attack, Patrol}
    
    // On Awake() get all references and set enemy initial state
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ownHealth = GetComponent<Health>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null )
        {
            playerHealth = player.GetComponent<Health>();
        }
        currentState = State.Patrol;
    }
    
    // On Update() handle timers progression and if player reference is not null execute CheckInSightDistance() and Move() methods
    void Update()
    {
        if (currentState == State.Patrol) 
        {
            patrolTime += Time.deltaTime;
        }
        if (currentState == State.Attack)
        {
            chasingTime += Time.deltaTime;
        }
        if (player != null)
        {
            CheckInSightDistance();
            Move();
        }
    }

    // This Method is an abstract method that will be overrided by the child class
    protected abstract void Attack();

    // This method calls respectively the methods to move depending on the enemy state
    protected void Move()
    {        
        if (currentState == State.Attack)
        {
            speed = 3;
            MoveToPlayer();
        }
        else if (currentState == State.Patrol)
        {
            speed = 1;
            MoveToPatrol();
        }
        currentPos = transform.position;
        if(!attacking)
        {
            MovementAnimationCheck();
        }
        else
        {
            AttackAnimationCheck();
        }
    }

    // MoveToPlayer() references the player position and handles speed according to its health alteration, moving towards the player,
    // executes CheckAttackRangeDistance() and checks if chasing time is not finished, if it is, it sets enemy state to Patrol
    protected void MoveToPlayer()
    {
        if (player != null)
        {
            playerPos = player.transform.position;

            if (!ownHealth.wet && !ownHealth.stunned)
            {
                DirectionToMoveWhileAttacking();
                rb.velocity = directionToMove * speed;
            }
            else if (ownHealth.wet)
            {
                DirectionToMoveWhileAttacking();
                rb.velocity = directionToMove * (speed / 3);
            }
            else if (ownHealth.stunned)
            {
                DirectionToMoveWhileAttacking();
                rb.velocity *= 0;
            }
            CheckAttackRangeDistance();
            if (chasingTime >= timeDelay)
            {
                currentState = State.Patrol;
                attacking = false;
                chasingTime = 0;
            }
        }
        else
        {
            currentState = State.Patrol;
            attacking = false;
            chasingTime = 0;
        }
    }

    // This method adjust the distance from the player depending on enemy attack range and direction of player
    protected void DirectionToMoveWhileAttacking()
    {
        if(playerPos.x < transform.position.x)
        {
            directionToMove = playerPos - new Vector2(transform.position.x - (rangeOfAttack / 1.25f), transform.position.y);
        }
        else if (playerPos.x > transform.position.x)
        {
            directionToMove = playerPos - new Vector2(transform.position.x + (rangeOfAttack / 1.25f), transform.position.y);
        }
        else if (playerPos.y < transform.position.y)
        {
            directionToMove = playerPos - new Vector2(transform.position.x, transform.position.y - (rangeOfAttack / 1.25f));
        }
        else if (playerPos.y > transform.position.y)
        {
            directionToMove = playerPos - new Vector2(transform.position.x, transform.position.y + (rangeOfAttack / 1.25f));
        }
        else if (playerPos.x < transform.position.x && playerPos.y < transform.position.y)
        {
            directionToMove = playerPos - new Vector2(transform.position.x - (rangeOfAttack / 1.25f), transform.position.y - (rangeOfAttack / 1.25f));
        }
        else if (playerPos.x < transform.position.x && playerPos.y > transform.position.y)
        {
            directionToMove = playerPos - new Vector2(transform.position.x - (rangeOfAttack / 1.25f), transform.position.y + (rangeOfAttack / 1.25f));
        }
        else if (playerPos.x > transform.position.x && playerPos.y < transform.position.y)
        {
            directionToMove = playerPos - new Vector2(transform.position.x - (rangeOfAttack / 1.25f), transform.position.y - (rangeOfAttack / 1.25f));
        }
        else if (playerPos.x > transform.position.x && playerPos.y > transform.position.y)
        {
            directionToMove = playerPos - new Vector2(transform.position.x + (rangeOfAttack / 1.25f), transform.position.y + (rangeOfAttack / 1.25f));
        }
    }

    // This method moves the enemy towards points in its array of targets
    protected void MoveToPatrol()
    {
        Vector2 patrolPos = targetPatrolPositions[targetCounter].position;
        directionToMove = patrolPos - new Vector2(transform.position.x, transform.position.y);
        rb.velocity = directionToMove * speed;
        float distance = Vector2.Distance(transform.position, patrolPos);
        if (distance < 0.1 && patrolTime >= timeDelay)
        {
            targetCounter++;
            patrolTime = 0;
            if (targetCounter >= targetPatrolPositions.Length)
            {
                targetCounter = 0;
            }
        }
    }

    // This method triggers the Attack state of the enemy if Player is in sight range
    protected void CheckInSightDistance()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= rangeOfSight)
        {
            currentState = State.Attack;
            chasingTime = 0;
        }
        //TODO: Raycast Method() and check if there is no other collisions in between 
        //      Enemy and Player, if the only collision is the player, Attack.
        //if (player != null)
        //{
        //    if (currentState != State.Attack)
        //    {
        //        playerPos = player.transform.position;
        //        directionToCast = playerPos - new Vector2(transform.position.x, transform.position.y);
        //    }
        //    //float distance = Vector2.Distance(transform.position, player.transform.position);
        //    if (distance <= rangeOfSight)
        //    {
        //        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToCast);
        //        if (hit.rigidbody != null)
        //        {
        //            Debug.Log($"I am hitting: {hit.rigidbody}");
        //            if (hit.rigidbody.CompareTag("Player"))
        //            {
        //                currentState = State.Attack;
        //                chasingTime = 0;
        //            }
        //        }
        //    }
        //}
    }

    // When the enemy is in attack state, with this method it will check if the player is in its attack range
    protected void CheckAttackRangeDistance()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= rangeOfAttack)
        {
            if (melee)
            {
                rb.velocity *= 0.5f;
            }
            if (ranged)
            {
                rb.velocity *= 1.5f;
            }
            Attack();
        }
        else
        {
            if (attacking)
            {
                if (melee)
                {
                    rb.velocity /= 0.5f;
                }
                if (ranged)
                {
                    rb.velocity /= 1.5f;
                }
                attacking = false;
            }
        }
    }

    protected void OnDestroy()
    {
        Health health = GetComponent<Health>();
        if (health != null)
        {
            if(health.currentHealth <= 0)
            {
                int dropChoice = Random.Range(0, droppingItems.Length);
                GameObject itemToDrop = Instantiate(droppingItems[dropChoice], transform.position, Quaternion.identity);
            }
        }

    }

    protected void MovementAnimationCheck()
    {
        Vector2 deltaPosition = currentPos - lastPos;

        if (deltaPosition.magnitude > 0.01f)
        {
            if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
            {
                if (deltaPosition.x > 0)
                {
                    animator.Play("WalkRight");
                }
                else if (deltaPosition.x < 0)
                {
                    animator.Play("WalkLeft");
                }
            }
            else
            {
                if (deltaPosition.y > 0)
                {
                    animator.Play("WalkUp");
                }
                else if (deltaPosition.y < 0)
                {
                    animator.Play("WalkDown");
                }
            }
        }
        lastPos = currentPos;
    }

    protected void AttackAnimationCheck()
    {
        Vector2 deltaPosition = currentPos - lastPos;

        if (deltaPosition.magnitude < rangeOfAttack)
        {
            if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
            {
                if (deltaPosition.x > 0)
                {
                    animator.Play("AttackRight");
                }
                else if (deltaPosition.x < 0)
                {
                    animator.Play("AttackLeft");
                }
            }
            else
            {
                if (deltaPosition.y > 0)
                {
                    animator.Play("AttackUp");
                }
                else if (deltaPosition.y < 0)
                {
                    animator.Play("AttackDown");
                }
            }
        }
        lastPos = currentPos;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeOfAttack);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(transform.position, directionToCast);
    }
}
