// GameUI.cs
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    public Transform countTextTransform;
    public Text countText;

    public GameObject btnParent;
    public Button rightBtn;
    public Button leftBtn;
    public GameObject levelUpUI;
    public GameObject[] lifeUIs;

    private Vector3 _countOriginalScale;
    private Vector3 _levelUpOriginalScale;

    void Start()
    {
        _countOriginalScale = countTextTransform.localScale;
        _levelUpOriginalScale = levelUpUI.transform.localScale;

        levelUpUI.SetActive(false);
        SetButtonsActive(false);
    }

    // 카운트 텍스트 갱신
    public void UpdateCount(int count)
    {
        countText.text = count.ToString();
    }

    // 카운트 올라갈 때 펀치 스케일 연출
    public void PlayCountAnim()
    {
        countTextTransform.DOKill();
        countTextTransform.localScale = _countOriginalScale;
        countTextTransform
            .DOPunchScale(Vector3.one * 0.4f, 0.3f, 5, 0.5f)
            .SetEase(Ease.OutElastic);
    }

    // 레벨업 연출
    public void PlayLevelUpAnim()
    {
        levelUpUI.SetActive(true);
        levelUpUI.transform.localScale = Vector3.zero;

        levelUpUI.transform
            .DOScale(_levelUpOriginalScale * 1.2f, 0.3f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                levelUpUI.transform
                    .DOScale(Vector3.zero, 0.2f)
                    .SetDelay(0.8f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => levelUpUI.SetActive(false));
            });
    }

    // 라이프 감소
    public void UpdateLife(int currentLife)
    {
        for (int i = 0; i < lifeUIs.Length; i++)
        {
            lifeUIs[i].SetActive(i < currentLife);
        }

        // 라이프 깎일 때 화면 흔들기 연출
        Camera.main.DOShakePosition(0.3f, 10f, 15)
            .SetUpdate(true);
    }

    // 좌우 버튼 활성/비활성
    public void SetButtonsActive(bool active)
    {
        btnParent.SetActive(active);
    }
}