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
    [SerializeField]
    private GameObject camera;

    private CanvasGroup uiCanvasGroup;
    private Vector3 targetVec;
    private Vector3 targetRotVec;
    private Vector3 closeUpVec;

    void Start()
    {
        uiCanvasGroup = GetComponent<CanvasGroup>();
        targetVec = new Vector3(12f, 30f, 0f);
        targetRotVec = new Vector3(-1.2f, 60f, 0f);
        closeUpVec = new Vector3(15.3f, 30f, 9.4f);
    }

    void Update()
    {
        
    }

    public void StartBtnActive()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(uiCanvasGroup.DOFade(0f, 1f).SetEase(Ease.Linear));
        seq.Append(camera.transform.DOLocalMove(targetVec, 2.5f).SetEase(Ease.InOutQuad));
        seq.Append(camera.transform.DOLocalRotate(targetRotVec, 2.8f).SetEase(Ease.InOutQuad));
        seq.Join(camera.transform.DOLocalMove(closeUpVec, 2.8f).SetEase(Ease.InOutQuad));
        seq.AppendCallback(() => PlayAnim());
        seq.AppendInterval(3f);
        seq.AppendCallback(()=> GameManager.Instance.ChangeScene("DeadOutTest"));
        
        seq.Play();
    }

    private void PlayAnim()
    {
        santaAnimator.SetTrigger("IsStart");
    }
}
