using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AmmoType
{
    Revolver
}

public class Items : MonoBehaviour
{
    [SerializeField] private AmmoType itemType;
    [SerializeField] private int bullet = 7;

    private bool _itemAchieved;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var correctWeapon = Weapon.WeaponList.Find(w => w.WeaponType.ToString().Equals(itemType.ToString()));

            if (correctWeapon && !_itemAchieved)
            {
                correctWeapon.RestAmmo += bullet;
                GetComponent<AudioSource>().Play();
                _itemAchieved = true;
                correctWeapon.UpdateAmmoInfo();
                Destroy(gameObject, 1f);
            }
        }
    }
}
