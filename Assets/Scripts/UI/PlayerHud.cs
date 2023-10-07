using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI moneyText;

    Inventory Inventory => FindObjectOfType<Inventory>();

    private void Start()
    {
        Inventory.OnMoneyChange += SetMoney;
        SetMoney(Inventory.CurrentMoney, 0);
    }

    public void SetHealth(int health, int maxHealth)
    {
        image.fillAmount = (float)health / (float)maxHealth;
    }

    void SetMoney(int current, int change)
    {
        moneyText.text = current.ToString();
    }
}
