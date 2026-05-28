// TrashItem.cs
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrashItem : MonoBehaviour
{
    [Header("Settings")]
    public float fallSpeed = 300f;        // 낙하 속도 (픽셀/초)
    public float rotateSpeed = 90f;       // 회전 속도

    private TrashType _trashType;
    private RectTransform _rt;
    private Canvas _canvas;
    private bool _isCaught = false;
    private float _bottomLimit;           // 화면 하단 Y 기준

    public TrashType TrashType => _trashType;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void Initialize(TrashType type)
    {
        _trashType = type;
        // 화면 하단 기준 (Canvas 높이 절반 + 여유)
        _bottomLimit = -(_canvas.GetComponent<RectTransform>().rect.height / 2f) - 100f;
    }

    void Update()
    {
        if (_isCaught) return;

        // 낙하
        _rt.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;

        // 회전 연출
        _rt.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);

        // 바닥 도달 → 놓침 처리
        if (_rt.anchoredPosition.y < _bottomLimit)
        {
            OnMissed();
        }
    }

    // 통과 충돌 감지 (TrashBin에서 호출)
    public void OnCaught(TrashType binType)
    {
        if (_isCaught) return;
        _isCaught = true;

        if (binType == _trashType)
        {
            // 맞는 통
            GameManager.Instance.OnTrashCaught();
            PlayCatchAnim(true);
        }
        else
        {
            // 틀린 통
            GameManager.Instance.OnTrashMissed();
            PlayCatchAnim(false);
        }
    }

    void OnMissed()
    {
        _isCaught = true;
        GameManager.Instance.OnTrashMissed();
        Destroy(gameObject);
    }

    void PlayCatchAnim(bool isCorrect)
    {
        // 맞으면 위로 튀었다 사라짐, 틀리면 흔들리다 사라짐
        if (isCorrect)
        {
            _rt.DOScale(Vector3.zero, 0.3f)
               .SetEase(Ease.InBack)
               .OnComplete(() => Destroy(gameObject));
        }
        else
        {
            _rt.DOShakePosition(0.3f, 20f, 10)
               .OnComplete(() => Destroy(gameObject));
        }
    }
}