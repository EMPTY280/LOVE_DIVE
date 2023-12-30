using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScene : MonoBehaviour
{
    [SerializeField] private string startSceneName = "StartScene";

    [SerializeField] private float typeStartDelay = 1f;
    [SerializeField] private Text gameoverText;
    private string originText = "";
    [SerializeField][Min(0.01f)] private float typeSpeed = 5f;

    [SerializeField] private GameObject buttonObj;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private float buttonFadeSpeed = 1f;

    private Coroutine task = null;

    private void Awake()
    {
        originText = gameoverText.text;
        gameoverText.text = "";

        buttonText.color = Vector4.zero;
        buttonObj.SetActive(false);

        task = StartCoroutine(Typing());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            ForceEndTyping();
    }

    public void ReturnToTitle()
    {
        GameManager.Instance.ChangeScene(startSceneName);
    }

    private IEnumerator Typing()
    {
        yield return new WaitForSecondsRealtime(typeStartDelay);
        int currIdx = 0;
        int strLen = originText.Length;
        while(currIdx <= strLen)
        {
            string newText = originText;
            newText = newText.Insert(currIdx, "<color=#00000000>");
            newText += "</color>";
            gameoverText.text = newText;
            yield return new WaitForSecondsRealtime(1f / typeSpeed);
            currIdx++;
        }
        task = StartCoroutine(RetryButton());
    }

    private IEnumerator RetryButton()
    {
        float alpha = 0.0f;
        buttonObj.SetActive(true);
        while(alpha < 1.0f)
        {
            Vector4 newColor = Vector4.one;
            alpha += buttonFadeSpeed * Time.deltaTime;
            newColor.w = alpha;
            buttonText.color = newColor;
            yield return null;
        }
        task = null;
    }

    private void ForceEndTyping()
    {
        if (task == null) return;

        StopCoroutine(task);
        gameoverText.text = originText;
        buttonText.color = Vector4.one;
        buttonObj.SetActive(true);
    }
}
