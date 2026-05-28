using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MainUI mainUI;
    public GameUI gameUI;
    public FadeUI fadeUI;

    private void Awake()
    {
        mainUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Action call = () =>
         {
             mainUI.gameObject.SetActive(false);
             gameUI.gameObject.SetActive(true);
             GameManager.Instance.GameStart();
         };

        fadeUI.FadeInOut(0.5f, 0.5f , 0.5f,call);
    }

    public void EndGame()
    {
        Action call = () =>
        {
            mainUI.gameObject.SetActive(true);
            gameUI.gameObject.SetActive(false);
        };

        fadeUI.FadeInOut(0.5f, 0.5f, 0.5f, call);
    }

}
