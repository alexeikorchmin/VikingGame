using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private int attackDistance;

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) == false) return;

        Raycast();
        print("Player attacks");
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        print("Player receives damage");
    }

    private void Raycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, attackDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * 10, Color.blue, 2f);

            if (hit.transform.CompareTag("Enemy"))
                hit.transform.gameObject.GetComponent<IEnemy>().ReceiveDamage(damage);
        }
    }

    private void Awake()
    {
        maxHealth = currentHealth;
    }

    private void Update()
    {
        Attack();
    }
}