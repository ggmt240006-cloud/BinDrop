// TrashBin.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TrashBin : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [Header("Bin Settings")]
    public Image binImage;
    public Sprite[] binSprites;           // 0:일반 1:플라스틱 2:종이 3:유리 순서

    [Header("Catch Zone")]
    public RectTransform catchZone;       // 쓰레기 감지 영역 (통 상단)

    private RectTransform _rt;
    private Canvas _canvas;
    private TrashType _currentType = TrashType.General;
    private float _canvasHalfWidth;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasHalfWidth = _canvas.GetComponent<RectTransform>().rect.width / 2f;
    }

    void Start()
    {
        SetBinType(TrashType.General);
    }

    public void GameStart()
    {
        SetBinType(TrashType.General);
        _rt.anchoredPosition = new Vector2(0f, _rt.anchoredPosition.y);
    }

    // 좌우 버튼으로 타입 변경
    public void OnClickRight()
    {
        int next = ((int)_currentType + 1) % GetMaxType();
        SetBinType((TrashType)next);
    }

    public void OnClickLeft()
    {
        int maxType = GetMaxType();
        int prev = ((int)_currentType - 1 + maxType) % maxType;
        SetBinType((TrashType)prev);
    }

    int GetMaxType()
    {
        switch (GameManager.Instance.GetCurrentLevel())
        {
            case 1: return 1; // 일반만
            case 2: return 2; // 일반, 플라스틱
            case 3: return 3; // 일반, 플라스틱, 종이
            case 4: return 4; // 일반, 플라스틱, 종이, 유리
            default: return 1;
        }
    }

    public void SetBinType(TrashType type)
    {
        _currentType = type;
        Debug.Log($"BinType 변경: {_currentType}");
        if (binSprites != null && binSprites.Length > (int)type)
            binImage.sprite = binSprites[(int)type];

        _rt.DOPunchScale(Vector3.one * 0.15f, 0.2f, 5, 0.5f);
    }

    // 드래그 이동
    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.GetComponent<RectTransform>(),
            eventData.position,
            _canvas.worldCamera,
            out localPoint
        );

        // X축만 이동, 화면 밖 제한
        float clampedX = Mathf.Clamp(localPoint.x, -_canvasHalfWidth + 80f, _canvasHalfWidth - 80f);
        _rt.anchoredPosition = new Vector2(clampedX, _rt.anchoredPosition.y);
    }

    // 충돌 감지 (매 프레임 TrashItem 위치와 비교)
    void Update()
    {
        CheckCatch();
    }

    void CheckCatch()
    {
        // catchZone 월드 기준 Rect
        Vector3[] catchCorners = new Vector3[4];
        catchZone.GetWorldCorners(catchCorners);
        Rect catchRect = new Rect(
            catchCorners[0].x,
            catchCorners[0].y,
            catchCorners[2].x - catchCorners[0].x,
            catchCorners[2].y - catchCorners[0].y
        );

        TrashItem[] items = FindObjectsOfType<TrashItem>();
        foreach (var item in items)
        {
            Vector3 itemWorldPos = item.GetComponent<RectTransform>().position;
            if (catchRect.Contains(new Vector2(itemWorldPos.x, itemWorldPos.y)))
            {
                item.OnCaught(_currentType);
            }
        }
    }
}