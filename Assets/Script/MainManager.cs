using UnityEngine;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance { get; private set; }

    public GameManager gameManager;
    public UIManager uiManager;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SoundManager.Instance.PlayBGM(0); // 메인 BGM 재생
    }

    public void OnGameStart()
    {
        SoundManager.Instance.PlayBGM(1); // 게임 BGM 재생
        uiManager.StartGame();
    }
    
    public void OnRetry()
    {
        SoundManager.Instance.PlayBGM(1); // 게임 BGM 재생
        uiManager.StartGame();
    }

    public void OnMenu()
    {
        SoundManager.Instance.PlayBGM(0); // 메인 BGM 재생
        uiManager.EndGame();
    }

}
