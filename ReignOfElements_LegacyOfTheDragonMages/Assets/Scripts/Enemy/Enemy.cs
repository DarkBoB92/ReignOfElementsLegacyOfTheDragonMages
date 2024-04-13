using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using SpellType;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float rangeOfSight, rangeOfAttack;
    [SerializeField] protected float currentTime, chasingTime, attackDelay,timeDelay;
    [SerializeField] protected int damage;
    [SerializeField] protected GameObject player;
    [SerializeField] protected float speed;
    [SerializeField] protected bool attacking;
    [SerializeField] protected State currentState;
    [SerializeField] protected Transform[] targetPatrolPositions;
    [SerializeField] protected int targetCounter = 0;    
    [SerializeField] protected Rigidbody2D rb;
    //[SerializeField] protected int horizontal, vertical; //Thinking to use simple movement for patrol and change direction after collision
    protected Vector2 playerPos, directionToMove; // directionToCast;
    protected Health playerHealth, ownHealth;    
    public enum State {Attack, Patrol}
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ownHealth = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null )
        {
            playerHealth = player.GetComponent<Health>();
        }
        currentState = State.Patrol;
        timeDelay = 3;
    }
    // Update is called once per frame
    void Update()
    {        
        currentTime += Time.deltaTime;
        if (currentState == State.Attack)
        {
            chasingTime += Time.deltaTime;
        }
        CheckInSightDistance();
        Move();
    }
    protected virtual void Attack()
    {

        Debug.Log("Something");
        //Do attack
    }
    protected virtual void Move()
    {        
        if (currentState == State.Attack)
        {
            MoveToPlayer();                      
        }
        else if (currentState == State.Patrol)
        {
            MoveToPatrol();
        }
    }

    protected virtual void MoveToPlayer()
    {
        playerPos = player.transform.position;
        //Check if you want to use Lerp or give velocity on the RigidBody               
        if (!ownHealth.wet && !ownHealth.stunned)
        {
            directionToMove = playerPos - new Vector2(transform.position.x, transform.position.y);
            rb.velocity = directionToMove * speed;
            //transform.position = new Vector2(Mathf.Lerp(transform.position.x, playerPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, playerPos.y, speed * Time.deltaTime));
            CheckAttackRangeDistance();
        }
        else if (ownHealth.wet)
        {
            directionToMove = playerPos - new Vector2(transform.position.x, transform.position.y);
            rb.velocity = directionToMove * (speed/3);
        }
        else if (ownHealth.stunned)
        {
            directionToMove = playerPos - new Vector2(transform.position.x, transform.position.y);
            rb.velocity *= 0;
        }        

        if (chasingTime >= timeDelay)
        {
            currentState = State.Patrol;
            chasingTime = 0;
        }
    }
    protected virtual void MoveToPatrol()
    {
        Vector2 patrolPos = targetPatrolPositions[targetCounter].position;
        //Check if you want to use Lerp or give velocity on the RigidBody
        directionToMove = patrolPos - new Vector2(transform.position.x, transform.position.y);
        rb.velocity = directionToMove * speed;
        //transform.position = new Vector2(Mathf.Lerp(transform.position.x, patrolPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, patrolPos.y, speed * Time.deltaTime));
        float distance = Vector2.Distance(transform.position, patrolPos);
        if (distance < 0.1 && currentTime >= timeDelay)
        {
            targetCounter++;
            currentTime = 0;
            if (targetCounter >= targetPatrolPositions.Length)
            {
                targetCounter = 0;
            }
        }
    }

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

    protected void CheckAttackRangeDistance()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= rangeOfAttack)
        {
            rb.velocity *= 0.5f;
            Attack();
        }
        else
        {
            rb.velocity = directionToMove * speed;
            attacking = false;
        }
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
