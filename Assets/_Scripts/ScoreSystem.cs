using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private GameManager managerUI;

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
        managerUI.DisplayCurrentScore(currentScore);
    }
}