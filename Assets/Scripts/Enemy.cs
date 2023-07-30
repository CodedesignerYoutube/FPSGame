using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyType
{
    Zombie1,
}

public class Enemy : MonoBehaviour
{

    [SerializeField] private EnemyType enemyType;

    [SerializeField] private int damage;

    [SerializeField] private float detectionDistance = 10;
    [SerializeField] private float runningDistance = 4;
    [SerializeField] private float attackingDistance = 2;
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 3;

    public bool IsDead { get; private set; }

    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _player;

    private enum AnimationName
    {
        Idle,
        Walk,
        Run,
        Attack,
        Dead
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void ActivateAnimationClip(AnimationName animationName)
    {
        _animator.SetBool(AnimationName.Idle.ToString(), false);
        _animator.SetBool(AnimationName.Walk.ToString(), false);
        _animator.SetBool(AnimationName.Run.ToString(), false);
        _animator.SetBool(AnimationName.Attack.ToString(), false);

        _animator.SetBool(animationName.ToString(), true);

    }

    public void DamagePlayer()
    {
        PlayerHealthController.Instance.DeductHealth(damage);
    }

    public void Dead()
    {
        if(!IsDead)
        {
            _animator.SetTrigger(AnimationName.Dead.ToString());
        }

        IsDead = true;
    }


    private void LateUpdate()
    {
        if (IsDead)
        {
            _agent.enabled = false;
            return;
        }

        var distance = Vector3.Distance(transform.position, _player.position);

        if (!PlayerHealthController.Instance.IsAlive)
        {
            _agent.updatePosition = false;
            ActivateAnimationClip(AnimationName.Idle);
            return;
        }

        if (distance <= detectionDistance)
        {
            _agent.SetDestination(_player.position);
        }

        if (distance >= detectionDistance)
        {
            ActivateAnimationClip(AnimationName.Idle);
        }
        else if (distance >= runningDistance && distance < detectionDistance)
        {
            ActivateAnimationClip(AnimationName.Walk);
            _agent.speed = walkingSpeed;
        }
        else if (distance < runningDistance && distance >= attackingDistance)
        {
            ActivateAnimationClip(AnimationName.Run);
            _agent.speed = runningSpeed;
        }
        else
        {
            ActivateAnimationClip(AnimationName.Attack);
        }

    }
}
