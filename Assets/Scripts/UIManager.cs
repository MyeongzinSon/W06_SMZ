using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] Image playerHpUI;
    [SerializeField] Image[] bossHpUIs;
    [SerializeField] Sprite[] playerHpSprites;
    [SerializeField] Sprite playerHpLoss;
    [SerializeField] Sprite bossHpLoss;
    [SerializeField] float playerHpAlertDuration;
    [SerializeField] Color alertColor;
    [SerializeField] GameObject gameOverUI;

    bool isAlert;
    float playerHpAlertTimer;
    Color defaultPlayerColor;


    public void Awake()
    {
        Instance = this;
        defaultPlayerColor = playerHpUI.color;
    }
    private void Update()
    {
        if (isAlert)
        {
            playerHpAlertTimer -= Time.deltaTime;
            if (playerHpAlertTimer > playerHpAlertDuration / 2)
            {
                playerHpUI.sprite = playerHpSprites[1];
                playerHpUI.color = defaultPlayerColor;
            }
            else if (playerHpAlertTimer > 0)
            {
                playerHpUI.sprite = playerHpSprites[0];
                playerHpUI.color = alertColor;
            }
            else
            {
                playerHpAlertTimer = playerHpAlertDuration;
            }
        }
    }
    public void SetPlayerHp(int value)
    {
        if (value > 0)
        {
            playerHpUI.sprite = playerHpSprites[value];
            if (value == 1)
            {
                isAlert = true;
                playerHpAlertTimer = playerHpAlertDuration;
            }
        }
        else
        {
            isAlert = false;
            playerHpUI.color = defaultPlayerColor;
            playerHpUI.sprite = playerHpLoss;
        }
    }
    public void SetBossHP(int value)
    {
        if (value < bossHpUIs.Length)
        {
            bossHpUIs[value].sprite = bossHpLoss;
        }
        else
        {
            Debug.LogError($"UIManager : 보스 체력 UI보다 많은 값을 설정함!");
        }
    }

    public void EnableGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
