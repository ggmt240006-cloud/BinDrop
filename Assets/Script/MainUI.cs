using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button StartBtn;
    public Button helpBtn;
    public Button exitBtn;

    public UnityEvent OnGameStartBtnClick;
    public UnityEvent OnHelpBtnClick;
    public UnityEvent OnExitBtnClick;

    public GameObject helpUI;
    public Button helpUICloseBtn;

    private void Start()
    {
        StartBtn.onClick.AddListener(OnStartBtnClicked);
        helpBtn.onClick.AddListener(OnHelpBtnClicked);
        exitBtn.onClick.AddListener(OnExitBtnClicked);
        helpUICloseBtn.onClick.AddListener(() => helpUI.SetActive(false));
    }

    private void OnStartBtnClicked()
    {
        OnGameStartBtnClick.Invoke();
    }

    private void OnHelpBtnClicked()
    {
        OnHelpBtnClick.Invoke();
        helpUI.SetActive(true);
    }

    private void OnExitBtnClicked()
    {
        OnExitBtnClick.Invoke();
        Application.Quit();
    }
}