using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject muzzleEffects;
    [SerializeField] private Transform barrel;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private float range = 50f;
    private Transform _cam;
    private AudioSource _audioSource;

    private void Start()
    {
        _cam = Camera.main.transform;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (Physics.Raycast(_cam.position, _cam.TransformDirection(Vector3.forward), out var hit, range))
        {

            if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
            {
                EnemyShot(hit.collider);
            }
            else
            {
                Instantiate(bulletHole, hit.point + (hit.normal * 0.001f),
                    Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }

            Instantiate(bulletImpact, hit.point + (hit.normal * 0.001f),
                Quaternion.FromToRotation(Vector3.up, hit.normal));

            Instantiate(muzzleEffects, barrel.position,
                barrel.rotation);

            _audioSource.PlayOneShot(shotSound);
        }
    }

    private void EnemyShot(Collider hitCollider)
    {
        var enemy = hitCollider.GetComponent<Enemy>();
        if (enemy.IsDead)
        {
            return;
        }

        enemy.Dead();
    }
}
