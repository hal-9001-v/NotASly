using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    static Pause instance;

    CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();
    SceneLoader SceneLoader => FindObjectOfType<SceneLoader>();

    bool isPaused;

    [SerializeField] Button pauseButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        pauseButton.onClick.AddListener(Hide);
        quitButton.onClick.AddListener(() => SceneLoader.QuitGame());

        Hide();
    }

    public void Show()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;

        Time.timeScale = 0;

        instance.isPaused = true;
    }

    public void Hide()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;

        Time.timeScale = 1;

        instance.isPaused = false;
    }

    public void Switch()
    {
        if (isPaused)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}
