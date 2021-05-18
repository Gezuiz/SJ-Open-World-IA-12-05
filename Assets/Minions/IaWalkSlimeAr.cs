﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IaWalkSlimeAr : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    //public Animator anim;
    public Vector3 patrolposition;
    public float stoppedTime;
    public float patrolDistance = 10;
    public float timetowait = 3;
    public float distancetotrigger = 200;
    public float distancetoattack = 100;
    //public bool AttackControl;
    public int SuperAttack;
    public Transform FantasmaAr;
    public GameObject Explosion;
    public enum IaState
    {
        Stopped,
        Berserk,
        Attack,
        Damage,
        Dying,
        Patrol,
        Super,
    }

    public IaState currentState;
    // Start is called before the first frame update
    void Start()
    {
        patrolposition = new Vector3(transform.position.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y, transform.position.z + Random.Range(-patrolDistance, patrolDistance));
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case IaState.Stopped:
                Stopped();
                break;
            case IaState.Berserk:
                Berserk();
                break;
            case IaState.Attack:
                Attack();
                break;
            case IaState.Damage:
                Damage();
                break;
            case IaState.Dying:
                Dying();
                break;
            case IaState.Patrol:
                Patrol();
                break;
            case IaState.Super:
                Super();
                break;
        }

        //anim.SetFloat("Velocity", agent.velocity.magnitude);
    }

    void Patrol()
    {
        agent.isStopped = false;
        agent.SetDestination(patrolposition);
        //anim.SetBool("Attack", false);
        //tempo parado
        if (agent.velocity.magnitude < 0.1f)
        {
            stoppedTime += Time.deltaTime;
        }
        //se for mais q timetowait segundos
        if (stoppedTime > timetowait)
        {
            stoppedTime = 0;
            patrolposition = new Vector3(transform.position.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y, transform.position.z + Random.Range(-patrolDistance, patrolDistance));
        }
        //ditancia do jogador for menor q distancetotrigger
        if (Vector3.Distance(transform.position, target.transform.position) < distancetotrigger && Vector3.Distance(transform.position, target.transform.position) >= distancetoattack)
        {
            currentState = IaState.Berserk;
        }

    }

    void Stopped()
    {
        agent.isStopped = true;
        //anim.SetBool("Attack", false);

        if (target && Vector3.Distance(transform.position, target.transform.position) > distancetotrigger)
        {
            currentState = IaState.Patrol;
        }
    }
    void Berserk()
    {
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
        //anim.SetBool("Attack", false);
        SuperAttack = 0;
        //se a distancia dele for  menor q 3 ele ataca
        if (Vector3.Distance(transform.position, target.transform.position) < distancetoattack)
        {
            currentState = IaState.Attack;
        }

        //se a distancia dele for  maior q o trigger ele patrulha de novo 
        if (Vector3.Distance(transform.position, target.transform.position) > distancetotrigger)
        {
            currentState = IaState.Patrol;
        }
    }

    void Attack()
    {
        agent.isStopped = true;
        //anim.SetBool("Attack", true);
        //se o jogador se afastar ele volta a perseguir
        if (Vector3.Distance(transform.position, target.transform.position) > distancetoattack)
        {
            currentState = IaState.Berserk;
        }
        else
        {
            StartCoroutine("Reatacar");
            SuperAttack += 1;
        }
        if (SuperAttack >= 150)
        {
            currentState = IaState.Super;
        }
    }
    void Damage()
    {
        agent.isStopped = true;
        //anim.SetBool("Attack", false);
        //anim.SetTrigger("Hit");
        currentState = IaState.Stopped;
    }
    public void Dying()
    {
        //Instantiate(HourGlassCoin,  new Vector3 (dragaofogo.position));
        Debug.Log("Matou Slime");
    }

    IEnumerator Reatacar()
    {
        yield return new WaitForSeconds(2);
        //anim.SetBool("Attack", false);
        currentState = IaState.Attack;

    }

    void Super()
    {
        Debug.Log("Olha o super ataque rapaziada kkk fudeu");
        SuperAttack = 0;
        Instantiate(Explosion, FantasmaAr.position, Quaternion.identity);
        currentState = IaState.Attack;

    }
}
