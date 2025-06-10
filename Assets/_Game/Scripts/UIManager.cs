using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    void Awake()
    {
        instance = this;
    }

    [SerializeField] TextMeshProUGUI coinText;

    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }

}
