using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class WeapnManager : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private GameObject weaponInventory;

    private bool _weaponInventoryState;

    public Weapon SelectedWeapon
    {
        get;
        private set;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleWeaponBox(!_weaponInventoryState);
        }
    }


    public void SelectWeapon(Weapon selectedWeapon)
    {
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(weapon == selectedWeapon);
        }

        SelectedWeapon = selectedWeapon;
    }



    public void ToggleWeaponBox(bool state)
    {
        weaponInventory.SetActive(state);
        _weaponInventoryState = state;
        Time.timeScale = state ? 0 : 1;
        FirstPersonController.ForceStop = state;
        Cursor.visible = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
