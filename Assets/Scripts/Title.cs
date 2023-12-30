using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Title : MonoBehaviour
{
    [SerializeField] GameObject credit = null;
    [SerializeField]
    private CanvasGroup creditCanvas;

    private void Start()
    {
        creditCanvas.interactable = false;
        creditCanvas.blocksRaycasts = false;
    }

    public void StartGame()
    {
        GameManager.Instance.ChangeScene("DeadOutTest");
    }

    public void OpenCredit()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(creditCanvas.DOFade(1f, 0.5f).SetEase((Ease.InOutQuad)));
        creditCanvas.interactable = true;
        creditCanvas.blocksRaycasts = true;
    }

    public void CloseCredit()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(creditCanvas.DOFade(0f, 0.5f).SetEase((Ease.InOutQuad)));
        creditCanvas.interactable = false;
        creditCanvas.blocksRaycasts = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
