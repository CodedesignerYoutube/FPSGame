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

    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip idleSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;


    public bool IsDead { get; private set; }

    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _player;

    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
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
        if (!IsDead)
        {
            _animator.SetTrigger(AnimationName.Dead.ToString());

            // Play the death sound directly
            _audioSource.clip = deathSound;
            _audioSource.loop = false; // Assuming you don't want the death sound to loop
            _audioSource.Play();

            IsDead = true;
        }
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
            PlayAudio(idleSound);
        }
        else if (distance >= runningDistance && distance < detectionDistance)
        {
            ActivateAnimationClip(AnimationName.Walk);
            _agent.speed = walkingSpeed;
            PlayAudio(walkSound);
        }
        else if (distance < runningDistance && distance >= attackingDistance)
        {
            ActivateAnimationClip(AnimationName.Run);
            _agent.speed = runningSpeed;
            PlayAudio(walkSound);
        }
        else
        {
            ActivateAnimationClip(AnimationName.Attack);
            PlayAudio(attackSound);
        }

    }


    private void PlayAudio(AudioClip clip)
    {
        if (_audioSource.clip != clip || !_audioSource.isPlaying)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

}
