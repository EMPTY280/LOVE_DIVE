using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIControl : MonoBehaviour
{
    [SerializeField] private Image progressBar = null;
    [SerializeField] private RectTransform hpIcon = null;
    private float hpIconWidth = 0f;

    private static UIControl instance = null;
    public static UIControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("UI").GetComponent<UIControl>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (progressBar == null || hpIcon == null) return;
        hpIconWidth = hpIcon.sizeDelta.x;
    }

    /// <summary>
    /// 프로그래스 바를 업데이트합니다. 
    /// </summary>
    /// <param name="value">프로그래스 바의 채워진 정도입니다. 0.0f ~ 1.0f</param>
    public void UpdateProgressBar(float value)
    {
        progressBar.fillAmount = Mathf.Clamp(value, 0.0f, 1.0f);

        if (value >= 1.0f)
        {
            GameManager.Instance.ChangeScene("Result");
        }
    }

    /// <summary>
    /// hp 표시기를 업데이트합니다.
    /// </summary>
    /// <param name="hp">업데이트할 체력의 수입니다. 0 이상</param>
    public void UpdateHP(int hp)
    {
        if (hp < 0) return;
        Vector2 newSize = hpIcon.sizeDelta;
        newSize.x = hp * hpIconWidth;
        hpIcon.sizeDelta = newSize;
    }
}
