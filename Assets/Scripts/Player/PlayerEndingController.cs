using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerEndingController : MonoBehaviour
{
    [SerializeField] private string endingSceneName;
    private Animator playerAni;
    private SpriteRenderer bossSprite;

    private IEnumerator Ending()
    {
        yield return new WaitForSeconds(2f);
        playerAni.Play("Dance");
        bossSprite.DOFade(0f, 3f);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(endingSceneName);

    }
}
