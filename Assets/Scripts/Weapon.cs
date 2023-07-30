using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum WeaponType
{
    Revolver
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject muzzleEffects;
    [SerializeField] private Transform barrel;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private float range = 50f;
    [SerializeField] private CameraShaker shaker;
    [SerializeField] private float fireDelay = 1f;
    [SerializeField] private float shotPower = 0.1f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private TextMeshProUGUI ammoInfo;

    public static List<Weapon> WeaponList = new ();

    public WeaponType WeaponType => weaponType;

    private const int MagazineSize = 7;

    public int RestAmmo
    {
        get;
        set;
    }

    private int _currentAmmo = 7;

    private Animator _revolverController;

    private Transform _cam;
    private AudioSource _audioSource;

    private Vector3 _defaultHoleSize;

    private void Start()
    {
        _cam = Camera.main.transform;
        _audioSource = GetComponent<AudioSource>();
        _revolverController = GetComponent<Animator>();
        _revolverController.speed *= 3;

        _defaultHoleSize = bulletHole.transform.localScale;

        UpdateAmmoInfo();

        RestAmmo = 20;
        WeaponList.Add(this);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (Physics.Raycast(_cam.position, _cam.TransformDirection(Vector3.forward), out var hit, range))
        {

            StartCoroutine(Fire(hit));
        }
    }


    private IEnumerator Fire(RaycastHit hit)
    {
        if (_currentAmmo > 0)
        {
            _currentAmmo--;
            UpdateAmmoInfo();
        }else if (RestAmmo > 0)
        {
            Reload();
            yield break;
        }
        else
        {
            yield break;
        }

        _revolverController.SetTrigger("Shot1");

        yield return new WaitForSeconds(fireDelay);

        shaker.Shake(shotPower, shakeDuration);

        var scaleFactor = Random.Range(0.8f, 1.5f);

        if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            EnemyShot(hit.collider);
        }
        else
        {
            var bulletHoleRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(0, 0, Random.Range(0, 360));
            var bulletHoleInstance = Instantiate(bulletHole, hit.point + (hit.normal * 0.001f), bulletHoleRotation);

            bulletHoleInstance.transform.localScale = _defaultHoleSize * scaleFactor;

            Destroy(bulletHoleInstance, 5f);
        }

        Instantiate(bulletImpact, hit.point + (hit.normal * 0.001f),
            Quaternion.FromToRotation(Vector3.up, hit.normal));

        Instantiate(muzzleEffects, barrel.position,
            barrel.rotation);

        _audioSource.PlayOneShot(shotSound);
    }


    public void UpdateAmmoInfo()
    {
        ammoInfo.text = $"Ammo : {_currentAmmo}/{RestAmmo}";
    }


    private void Reload()
    {
        var ammoToReload = Math.Min(MagazineSize - _currentAmmo, RestAmmo);
        _currentAmmo += ammoToReload;
        RestAmmo -= ammoToReload;

        UpdateAmmoInfo();
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
