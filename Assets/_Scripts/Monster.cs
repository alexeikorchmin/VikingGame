using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IEnemy
{
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

        navMeshAgent.SetDestination(playerGo.transform.position);

        if (navMeshAgent.remainingDistance <= attackDistance)
        {
            Attack();
        }
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        print("Monster receives damage");
    }

    public void Attack()
    {
        player.ReceiveDamage(damage);
        print("Monster attacks");
    }

    public void Die()
    {

    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        playerGo = GameObject.FindGameObjectWithTag("Player");

        navMeshAgent.SetDestination(playerGo.transform.position);

        if (playerGo.TryGetComponent(out Player player))
            this.player = player;

        maxHealth = currentHealth;
    }

    private void Update()
    {
        ChasePlayer();
    }
}