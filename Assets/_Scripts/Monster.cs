using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IEnemy
{
    //[SerializeField] private TMP_Text healthText;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private int attackDistance = 3;

    private GameObject playerGo;
    private Player player;
    private NavMeshAgent navMeshAgent;

    public void ChasePlayer()
    {
        if (playerGo == null) return;

        if (navMeshAgent == null) return;

        if (playerGo.activeSelf == false)
        {
            navMeshAgent.isStopped = true;
            Debug.Log("NavMesh Stop");
            return;
        }

        navMeshAgent.SetDestination(playerGo.transform.position);

        if (navMeshAgent.remainingDistance <= attackDistance)
        {
            if (!IsInvoking(nameof(Attack)))
                InvokeRepeating(nameof(Attack), 0.5f, 2f);
        }
        else
        {
            CancelInvoke(nameof(Attack));
        }
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Monster HP = 0");
            Die();
        }

        DisplayHealth();
        Debug.Log("Monster receives damage");
    }

    public void Attack()
    {
        if (playerGo.activeSelf == false) return;

        player.ReceiveDamage(damage);
        Debug.Log("Monster attacks");
    }

    public void Die()
    {
        Debug.Log("Monster Died");
        gameObject.SetActive(false);
    }

    public void DisplayHealth()
    {
        //healthText.text = currentHealth.ToString();
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        playerGo = GameObject.FindGameObjectWithTag("Player");

        navMeshAgent.SetDestination(playerGo.transform.position);

        if (playerGo.TryGetComponent(out Player player))
            this.player = player;

        currentHealth = maxHealth;
        DisplayHealth();
    }

    private void Update()
    {
        ChasePlayer();
    }
}