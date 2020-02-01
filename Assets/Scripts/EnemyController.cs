﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints = null;

    public float turnSpeed = 2;
    public float idleSpeed = 1;
    public float chaseSpeed = 2;

    enum Behaviour
    {
        ChasePlayer,
        FollowPoints,
        ReturnToPatrolArea
    }
    Behaviour currentBehaviour;

    int waypointIndex = 0;

    public float startChaseRadius;
    public float stopChaseRadius;

    //true if chasing, false if not

    //true = chase
    //false = follow path

    // Target of the chase
    // (initialise via the Inspector Panel)
    GameObject target;

    // Chaser's speed
    // (initialise via the Inspector Panel)

    // Chasing game object must have a AStarPathfinder component - 
    // this is a reference to that component, which will get initialised
    // in the Start() method
    private AStarPathfinder pathfinder = null;
    private AStarPathfinder pathWayPoint = null;


    // Use this for initialization
    void Start()
    {
        currentBehaviour = Behaviour.FollowPoints;
        target = GameObject.Find("Player");

        //Get the reference to object's AStarPathfinder component
        pathfinder = transform.GetComponent<AStarPathfinder>();
        pathWayPoint = transform.GetComponent<AStarPathfinder>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(target.transform.position, transform.position);

        if (distance < startChaseRadius && currentBehaviour != Behaviour.ChasePlayer)
        {
            currentBehaviour = Behaviour.ChasePlayer;
        }
        else if (distance > stopChaseRadius && currentBehaviour == Behaviour.ChasePlayer)
        {
            currentBehaviour = Behaviour.ReturnToPatrolArea;
        }

        switch (currentBehaviour)
        {
            case Behaviour.ChasePlayer:
                Chase();
                break;
            case Behaviour.FollowPoints:
                FollowWayPoints();
                break;
            case Behaviour.ReturnToPatrolArea:
                pathfinder.GoTowards(waypoints[waypointIndex].position, idleSpeed);
                break;
        }
    }

    private void Chase()
    {
        if (Vector2.Distance(target.transform.position, transform.position) < 2)
        {

            Vector2 difference = transform.position - target.transform.position;
            float targetAngle = Vector2.Angle(Vector2.up, difference);
            if (difference.x > 0)
            {
                targetAngle = targetAngle * -1;
            }

            // rotates towards target angle with rate given by turnSpeed
            transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.Euler(0, 0, targetAngle + 90),
                            turnSpeed * Time.deltaTime);

            // constantly moves in the direction it's facing
            transform.Translate(Vector3.left * chaseSpeed * Time.deltaTime);
        }
        else
        {
            pathfinder.GoTowards(target, chaseSpeed);
        }

    }

    private void FollowWayPoints()
    {
        if (waypoints != null)
        {
            // If Enemy didn't reach last waypoint it can move
            // If enemy reached last waypoint then it stops

            // finds required angle for fish to face next waypoint and turns it into a Euler angle
            Vector2 difference = transform.position - waypoints[waypointIndex].position;
            float targetAngle = Vector2.Angle(Vector2.up, difference);
            if (difference.x > 0)
            {
                targetAngle = targetAngle * -1;
            }

            // rotates towards target angle with rate given by turnSpeed
            transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.Euler(0, 0, targetAngle + 90),
                            turnSpeed * Time.deltaTime);

            // constantly moves in the direction it's facing
            transform.Translate(Vector3.left * idleSpeed * Time.deltaTime);


            // pathfinder.GoTowards(waypoints[waypointIndex].gameObject, idleSpeed);
        }
    }

    private void goToCurrentPoint()
    {
        pathfinder.GoTowards(target, chaseSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when entering the collider of a waypoint, start start going to the next one
        if (collision.gameObject.tag == "Waypoint")
        {
            if (currentBehaviour == Behaviour.FollowPoints)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
            }
            else if (currentBehaviour == Behaviour.ReturnToPatrolArea)
            {
                currentBehaviour = Behaviour.FollowPoints;
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
            }
        }
    }
}