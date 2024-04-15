using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] Text hpText;
    public void OnInit(float dame)
    {
        hpText.text = dame.ToString();
        Invoke(nameof(OnDespan), 1f);
    }

    private void OnDespan()
    {
        Destroy(gameObject);
    }
}
