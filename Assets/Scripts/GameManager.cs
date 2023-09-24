using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const float MapSize = 36;

    public static GameManager instance;
    [SerializeField] GameObject player;
    [SerializeField] GameObject boss;

    [SerializeField] private string endingSceneName;
    private Animator playerAni;
    private SpriteRenderer bossSprite;

    private void Start()
    {
        playerAni = player.GetComponent<Animator>();
        bossSprite = boss.GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject GetPlayer() => player;

    public GameObject GetBoss() => boss;

    public void SetBoss(GameObject bossObject)
    {
        boss = bossObject;
    }

    public void GameOver()
    {
        UIManager.Instance.EnableGameOverUI();
    }

    public void StartEnding()
    {
        StartCoroutine(nameof(Ending));
    }
    private IEnumerator Ending()
    {
        bossSprite = boss.GetComponent<SpriteRenderer>();
        player.GetComponent<PlayerController>().enabled = false;
        for(int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        playerAni.Play("Dance");
        bossSprite.DOFade(0f, 3f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(endingSceneName);

    }
}
