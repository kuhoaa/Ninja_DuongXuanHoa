using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager ins;
    private void Awake()
    {
        ins = this;
    }
    [SerializeField] Text cointText;

    public void SetCoin(int coin)
    {
        cointText.text = coin.ToString();
    }
}
