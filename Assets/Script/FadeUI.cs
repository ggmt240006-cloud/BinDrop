using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Coroutine _fadeCoroutine;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Fade In -> 중간 콜백 실행 -> 대기 -> Fade Out -> 최종 콜백 실행
    /// </summary>
    /// <param name="fadeInTime">나타나는 시간</param>
    /// <param name="stayTime">중간에 머무는 시간</param>
    /// <param name="fadeOutTime">사라지는 시간</param>
    /// <param name="onMidCallback">Fade In 완료 직후 실행할 코드</param>
    /// <param name="onEndCallback">모든 과정이 끝난 후 실행할 코드</param>
    public void FadeInOut(float fadeInTime, float stayTime, float fadeOutTime, Action onMidCallback = null, Action onEndCallback = null)
    {
        gameObject.SetActive(true);
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeInOutRoutine(fadeInTime, stayTime, fadeOutTime, onMidCallback, onEndCallback));
    }

    private IEnumerator FadeInOutRoutine(float inTime, float stayTime, float outTime, Action midCallback, Action endCallback)
    {
        // 1. Fade In (나타나기)
        yield return StartCoroutine(AnimateFade(1f, inTime));

        // 2. 중간 콜백 호출 (이 시점에 캐릭터 이동이나 데이터 변경 등을 수행)
        midCallback?.Invoke();

        // 3. 지정된 시간만큼 대기
        yield return new WaitForSeconds(stayTime);

        // 4. Fade Out (사라지기)
        yield return StartCoroutine(AnimateFade(0f, outTime));

        // 5. 최종 완료 콜백 호출
        endCallback?.Invoke();

        _fadeCoroutine = null;

        gameObject.SetActive(false);
    }

    // Alpha 조절 공통 로직
    private IEnumerator AnimateFade(float targetAlpha, float duration)
    {
        float startAlpha = _canvasGroup.alpha;
        float time = 0;

        // 레이캐스트 설정 (표시될 때만 클릭 방지 활성화)
        _canvasGroup.blocksRaycasts = targetAlpha > 0;
        _canvasGroup.interactable = targetAlpha > 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
    }
}