using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] bool showOnStart;


    private void Start()
    {
        if (showOnStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
