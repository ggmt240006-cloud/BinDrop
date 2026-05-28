using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Text scoreText;

    public Button retryButton;
    public Button menuButton;
    public Button exitButton;

    private void Start()
    {
        retryButton.onClick.AddListener(OnRetry);
        menuButton.onClick.AddListener(OnMenu);
        exitButton.onClick.AddListener(OnExit);
    }

    public void ShowGameOver(int score)
    {
        scoreText.text = "Score: " + score;
        gameObject.SetActive(true);

    }

    private void OnRetry()
    {
        MainManager.Instance.OnRetry();
    }
    
    private void OnMenu()
    {
        MainManager.Instance.OnMenu();
    }

    private void OnExit()
    {
        // 게임 종료 로직
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

}
