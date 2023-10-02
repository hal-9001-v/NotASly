using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] Image image;

    public void SetHealth(int health, int maxHealth)
    {
        image.fillAmount = (float)health / (float)maxHealth;
    }
}
