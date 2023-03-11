using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IEnemy
{
    public static event Action OnMosterDied;
    
    //[SerializeField] private TMP_Text healthText;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private int attackDistance = 3;

    private NavMeshAgent navMeshAgent;
    private GameObject playerGo;
    private Player player;

    [SerializeField] private bool canMove;
    private Vector3 spawnPosition;

    public void Attack()
    {
        if (playerGo.activeSelf == false) return;

        if (Physics.Raycast(transform.position, transform.forward, out var hit, attackDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * 10, Color.green, 1f);
            Debug.Log("Monster Raycast OK");

            if (hit.transform.CompareTag("Player"))
            {
                player.ReceiveDamage(damage);
                Debug.Log("Monster Raycast Player OK. Monster attacks");
            }
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

    public void ChasePlayer()
    {
        if (!canMove) return;

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
            CancelInvoke(nameof(Attack));
    }

    public void DisplayHealth()
    {
        //healthText.text = currentHealth.ToString();
    }

    public void Die()
    {
        Debug.Log("Monster Died");
        canMove = false;
        OnMosterDied?.Invoke();
        gameObject.SetActive(false);

        Invoke(nameof(Spawn), 2f);
    }

    private void Spawn()
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        DisplayHealth();
        canMove = true;
    }

    private void Init(bool isGameStarted)
    {
        canMove = isGameStarted;
    }

    private void Awake()
    {
        GameManager.OnGameStarted += Init;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStarted -= Init;
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        playerGo = GameObject.FindGameObjectWithTag("Player");

        if (playerGo.TryGetComponent(out Player player))
            this.player = player;

        spawnPosition = transform.position;
        currentHealth = maxHealth;
        DisplayHealth();
    }

    private void Update()
    {
        ChasePlayer();
    }
}