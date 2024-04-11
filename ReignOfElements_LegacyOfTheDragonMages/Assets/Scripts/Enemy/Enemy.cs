using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float rangeOfSight; //Maybe public
    [SerializeField] protected float currentTime, chasingTime, timeDelay;
    [SerializeField] protected GameObject player;
    [SerializeField] protected float speed;
    [SerializeField] protected State currentState;
    [SerializeField] protected Transform[] targetPatrolPositions;
    [SerializeField] protected int targetCounter = 0;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected int horizontal, vertical;
    Vector2 playerPos, directionToCast;
    public enum State {Attack, Patrol}
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");        
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
        CheckDistance();
        MoveToPosition();
    }
    protected virtual void Attack()
    {
        Debug.Log("Something");
        //Do attack
    }
    protected virtual void MoveToPosition()
    {        
        if (currentState == State.Attack)
        {
            playerPos = player.transform.position;
            //Check if you want to use Lerp or give velocity on the RigidBody
            //Vector2 directionToMove = playerPos - new Vector2(transform.position.x, transform.position.y);
            //rb.velocity = directionToMove * speed;
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, playerPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, playerPos.y, speed * Time.deltaTime));
            if (chasingTime >= timeDelay)
            {
                currentState = State.Patrol;
                chasingTime = 0;
            }            
        }
        else if (currentState == State.Patrol)
        {
            Vector2 patrolPos = targetPatrolPositions[targetCounter].position;
            //Check if you want to use Lerp or give velocity on the RigidBody
            //Vector2 directionToMove = patrolPos - new Vector2(transform.position.x, transform.position.y);
            //rb.velocity = directionToMove * speed;
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, patrolPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, patrolPos.y, speed * Time.deltaTime));
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
    }
    void CheckDistance()
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
        //        directionToCast = new Vector2(transform.position.x, transform.position.y) - playerPos;
        //    }
        //    float distance = Vector2.Distance(transform.position, player.transform.position);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfSight);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(transform.position, directionToCast);
    }
}
