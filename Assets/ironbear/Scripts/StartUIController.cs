using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartUIController : MonoBehaviour
{
    [SerializeField]
    private Animator uiAnimator;
    [SerializeField]
    private Animator santaAnimator;

    private CanvasGroup uiCanvasGroup;

    void Start()
    {
        uiCanvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        
    }

    public void StartBtnActive()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(uiCanvasGroup.DOFade(0f, 1f).SetEase(Ease.Linear)).OnComplete(() =>
        {
            santaAnimator.SetTrigger("IsStart");
            GameManager.Instance.ChangeScene("DeadOutTest2");
        });

        seq.Play();
    }
}
