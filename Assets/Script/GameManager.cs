// GameManager.cs
using UnityEngine;
using DG.Tweening;
public enum TrashType
{
    General,    // 일반
    Plastic,    // 플라스틱
    Paper,      // 종이
    Glass       // 유리
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameUI gameUI;
    public GameOverUI gameOverUI;

    [Header("Settings")]
    public int lifeCount = 3;
    public int countPerLevel = 15;

    private int _currentCount = 0;
    private int _currentLife = 3;
    private int _currentLevel = 1; // 1~4
    private bool _isGameOver = false;

    public TrashSpawner trashSpawner;
    public TrashBin trashBin;

    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _currentLife = lifeCount;
        _currentLevel = 1;
        _currentCount = 0;
        gameUI.SetButtonsActive(false);
        gameUI.UpdateCount(0);
        // Start에서 자동실행 제거
    }

    public void GameStart()
    {
        _isGameOver = false;
        gameOverUI.gameObject.SetActive(false);
        _currentLife = lifeCount;
        _currentLevel = 1;
        _currentCount = 0;

        // 스폰된 오브젝트 전부 제거
        ClearAllSpawnedObjects();

        gameUI.SetButtonsActive(false);
        gameUI.UpdateCount(0);
        gameUI.UpdateLife(_currentLife);

        trashBin.GameStart();

        trashBin.SetBinType(TrashType.General);
        trashSpawner.UpdateSpawnTypes(1);
    }

    // 쓰레기 받았을 때 (맞는 통)
    public void OnTrashCaught()
    {
        if (_isGameOver) return;

        _currentCount++;
        gameUI.UpdateCount(_currentCount);
        gameUI.PlayCountAnim();

        // 레벨업 체크
        int newLevel = (_currentCount / countPerLevel) + 1;
        newLevel = Mathf.Clamp(newLevel, 1, 4);

        if (newLevel > _currentLevel)
        {
            _currentLevel = newLevel;
            OnLevelUp();
        }

        SoundManager.Instance.PlaySFX(0); // 성공 효과음
    }

    // 쓰레기 놓쳤거나 틀린 통
    public void OnTrashMissed()
    {
        if (_isGameOver) return;

        _currentLife--;
        gameUI.UpdateLife(_currentLife);

        if (_currentLife <= 0)
        {
            GameOver();
        }

        SoundManager.Instance.PlaySFX(1);
    }

    public int GetCurrentLevel() => _currentLevel;

    void OnLevelUp()
    {
        if (_currentLevel >= 2)
            gameUI.SetButtonsActive(true);

        gameUI.PlayLevelUpAnim();
        trashSpawner.UpdateSpawnTypes(_currentLevel);

        // 통 타입 리셋
        trashBin.SetBinType(TrashType.General);
    }

    void GameOver()
    {
        _isGameOver = true;

        // 스폰된 오브젝트 전부 제거
        ClearAllSpawnedObjects();
        trashSpawner.StopSpawn();

        gameOverUI.ShowGameOver(_currentCount);

        SoundManager.Instance.PlayBGM(2); // 게임오버 BGM 재생
    }

    void ClearAllSpawnedObjects()
    {
        trashSpawner.ClearAllSpawnedObjects();
    }
}