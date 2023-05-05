using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int maximumHealth = 100;

    public static PlayerHealthController Instance { get; private set; }

    private int _currentHealth;
    private Rigidbody _rb;

    public bool IsAlive => _currentHealth > 0;

    private void Start()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }else if (this != Instance)
        {
            Destroy(gameObject);
        }

        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        _currentHealth = maximumHealth;
    }

    private void Update()
    {
        if (!IsAlive)
        {
            GetComponent<CharacterController>().enabled = false;
            _rb.isKinematic = false;
            _rb.AddForce(transform.up * -1);
            healthText.text = $"Health: {_currentHealth}";
        }
    }

    public void DeductHealth(int damageValue)
    {
 
        if (_currentHealth > 0)
        {
            _currentHealth -= damageValue;
        }

        healthText.text = $"Health: {_currentHealth}";
    }

}
