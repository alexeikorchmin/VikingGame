using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private int currentScore;

    private void Awake()
    {
        Monster.OnMosterDied += OnMosterDiedHandler;
    }

    private void OnDestroy()
    {
        Monster.OnMosterDied -= OnMosterDiedHandler;
    }

    private void OnMosterDiedHandler()
    {
        currentScore++;
        gameManager.DisplayCurrentScore(currentScore);
    }
}