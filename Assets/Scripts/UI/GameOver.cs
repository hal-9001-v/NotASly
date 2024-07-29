using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GameOver : MonoBehaviour
{
    CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();

    SceneLoader SceneLoader => FindAnyObjectByType<SceneLoader>();

    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;

    private void Awake()
    {
        Hide();

        retryButton.onClick.AddListener(() =>
        {
            SceneLoader.ReloadScene();
        });

        exitButton.onClick.AddListener(() =>
        {
            SceneLoader.QuitGame();
        });
    }

    public void Show()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }

}
