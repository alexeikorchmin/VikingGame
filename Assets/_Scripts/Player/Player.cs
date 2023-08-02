using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerDied;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private int attackDistance;

    [SerializeField] private Animator animator;

    private bool canAttack;

    public void Attack()
    {
        if (!canAttack) return;

        if (Input.GetMouseButtonDown(0) == false) return;

        animator.Play("Attack");

        Debug.Log("Player attacks");
        Invoke(nameof(Raycast), 3f);
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;

        animator.Play("TakeDamage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player TakeDamage HP = 0");
            //Die();
            StartCoroutine(DieCoroutine());
        }

        healthBar.SetHealth(currentHealth);
        Debug.Log("Player receives damage");
    }

    private void Awake()
    {
        GameManager.OnGameisPlayed += Init;
    }

    private void OnDestroy()
    {
        GameManager.OnGameisPlayed -= Init;
    }

    private void Init(bool isGameStarted)
    {
        canAttack = true;
        gameObject.SetActive(true);
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
            Debug.Log("Player DrawRaycast OK");

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<IEnemy>().ReceiveDamage(damage);
                Debug.Log("Player Raycast IEnemy OK");
            }
        }
    }

    private void Die()
    {
        animator.Play("PlayerDie");
        Debug.Log("Player Died");
        OnPlayerDied?.Invoke();
        gameObject.SetActive(false);
    }

    private IEnumerator DieCoroutine()
    {
        Debug.Log("Coroutine Start");
        animator.Play("Die");
        OnPlayerDied?.Invoke();
        canAttack = false;
        yield return new WaitForSeconds(3f);

        Debug.Log("Coroutine Player Died");
        gameObject.SetActive(false);
    }
}