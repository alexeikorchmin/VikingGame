using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private int attackDistance;

    private void DisplayHealth()
    {
        healthText.text = currentHealth.ToString();
    }

    private void Die()
    {
        Debug.Log("Player Died");
        gameObject.SetActive(false);
    }

    public void Attack()
    {
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

        DisplayHealth();
        Debug.Log("Player receives damage");
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        DisplayHealth();
    }

    private void Update()
    {
        Attack();
    }

    private void Raycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, attackDistance * 10))
        {
            Debug.Log("Player Raycast OK");
            Debug.DrawRay(transform.position, transform.forward * 10, Color.blue, 1f);

            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.gameObject.GetComponent<IEnemy>().ReceiveDamage(damage);
                Debug.Log("Player Raycast IEnemy OK");
            }
        }
    }
}