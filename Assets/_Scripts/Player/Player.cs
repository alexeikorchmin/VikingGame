using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerDied;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private int attackDistance;

    private bool canAttack;

    public void Attack()
    {
        if (!canAttack) return;
        
        if (Input.GetMouseButtonDown(0) == false) return;

        Raycast();
        Debug.Log("Player attacks");
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player HP = 0");
            Die();
        }

        healthBar.SetHealth(currentHealth);
        Debug.Log("Player receives damage");
    }

    private void Awake()
    {
        GameManager.OnGameStarted += Init;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStarted -= Init;
    }

    private void Init(bool isGameStarted)
    {
        canAttack = isGameStarted;
        gameObject.SetActive(isGameStarted);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        Attack();
    }

    private void Raycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, attackDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * 10, Color.blue, 1f);
            Debug.Log("Player Raycast OK");

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<IEnemy>().ReceiveDamage(damage);
                Debug.Log("Player Raycast IEnemy OK");
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        OnPlayerDied?.Invoke();
        gameObject.SetActive(false);
    }
}