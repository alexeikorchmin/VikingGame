using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static event Action<bool> OnGameisPlayed;

    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text startRestartButtonText;
    [SerializeField] private GameObject topPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject finalScoreButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private int currentScore;

    public void DisplayCurrentScore(int newCurrentScore)
    {
        currentScore = newCurrentScore;
        currentScoreText.text = $"Scores: {currentScore.ToString()}";
    }

    private void Awake()
    {
        Player.OnPlayerDied += OnPlayerDiedHandler;

        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDestroy()
    {
        Player.OnPlayerDied -= OnPlayerDiedHandler;
    }

    private void OnPlayerDiedHandler()
    {
        OnGameisPlayed?.Invoke(false);
        DisplayFinalScore();
        topPanel.SetActive(false);
        finalScoreButton.SetActive(true);
        startRestartButtonText.text = "RESTART";
        startPanel.SetActive(true);
    }

    private void DisplayFinalScore()
    {
        finalScoreText.text = $"Scores: {currentScore.ToString()}";
    }

    private void StartGame()
    {
        startPanel.SetActive(false);
        topPanel.SetActive(true);
        finalScoreButton.SetActive(false);
        startRestartButtonText.text = "START";
        OnGameisPlayed?.Invoke(true);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}