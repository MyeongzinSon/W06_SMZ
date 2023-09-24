using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CokeGaugeUI : MonoBehaviour
{
    [SerializeField] Image gaugeBar;
    [SerializeField] Sprite fullSprite;

    Sprite defaultSprite;

    private void Awake()
    {
        defaultSprite = gaugeBar.sprite;
    }
    public void SetGaugeValue(float value, bool isFull = false, bool canUse = true)
    {
        gaugeBar.sprite = isFull ? fullSprite : defaultSprite;
        gaugeBar.color = canUse ? Color.white : Color.gray;
        gaugeBar.fillAmount = value;
    }
}
