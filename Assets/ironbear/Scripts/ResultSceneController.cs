using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultSceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;

    private Vector3 zoomVec;

    private void Start()
    {
        zoomVec = new Vector3(-2.75f, 1.17f, -8.24f);

        Invoke("ZoomCamera", 4.5f);
    }

    private void ZoomCamera()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(camera.transform.DOMove(zoomVec, 0.5f).SetEase(Ease.OutQuad));
        seq.AppendInterval(1f).OnComplete(() =>
        {
            GameManager.Instance.ChangeScene("StartScene");
        });
        seq.Play();
    }
}
