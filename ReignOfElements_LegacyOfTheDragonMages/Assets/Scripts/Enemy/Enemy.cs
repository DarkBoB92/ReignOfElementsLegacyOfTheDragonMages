using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (player != null)
            {
                Vector2 playerPos = player.transform.position;
                transform.position = new Vector2(Mathf.Lerp(transform.position.x, playerPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, playerPos.y, speed * Time.deltaTime));
                if (chasingTime >= timeDelay)
                {
                    currentState = State.Patrol;
                    chasingTime = 0;
                }                
            }
        }
        else if (currentState == State.Patrol)
        {
            Vector2 patrolPos = targetPatrolPositions[targetCounter].position;
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, patrolPos.x, speed * Time.deltaTime), Mathf.Lerp(transform.position.y, patrolPos.y, speed * Time.deltaTime));        
            float distance = Vector2.Distance(transform.position, patrolPos);
            if (distance < 0.1f && currentTime >= timeDelay)
            {
                targetCounter++;
                currentTime = 0;
                if (targetCounter >= targetPatrolPositions.Length - 1)
                {
                    targetCounter = 0;
                }
            }
        }
    }
    void CheckDistance()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= rangeOfSight)
            {
                currentState = State.Attack;
                chasingTime = 0;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfSight);
    }
}
