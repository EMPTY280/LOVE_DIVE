using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private Image fadeImage = null;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float transitionDelay = 1.0f;

    /// <summary>
    /// 페이드 인/아웃이 완료되기까지의 시간입니다. (N초, 인/아웃에 각각 적용됨, 최소 0.1초)
    /// </summary>
    public float FadeTime
    {
        get { return fadeTime; }
        set { fadeTime = Mathf.Min(0.1f, value); }
    }

    /// <summary>
    /// 페이드 아웃 후 페이드 인이 시작되기까지의 대기 시간입니다. (N초, 최소 0초)
    /// </summary>
    public float TransitionDelay
    {
        get { return transitionDelay; }
        set { transitionDelay = Mathf.Min(0f, value); }
    }

    private delegate void Callback();
    [SerializeField] private bool isFading = false;
    private string targetScene = "";

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<GameManager>();
                instance.name = "[ GameManager ]";
            }
            return instance;
        }
    }

    private void Awake()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/ScreenEffect");
        GameObject inst = Instantiate(prefab);
        fadeImage = inst.transform.GetChild(0).GetComponent<Image>();

        Color newColor = fadeImage.color;
        newColor.a = 0f;
        fadeImage.color = newColor;
        fadeImage.gameObject.SetActive(true);
        fadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        DontDestroyOnLoad(inst);
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 페이드 아웃과 함께 씬을 전환합니다.
    /// </summary>
    /// <param name="name">전환할 씬의 이름입니다.</param>
    public void ChangeScene(string name)
    {
        if (isFading) return;
        isFading = true;

        targetScene = name;
        StartCoroutine(CFadeOut( () =>
        {
            SceneManager.LoadScene(targetScene);
            StartCoroutine(CFadeIn(null));
        }));
    }

    /// <summary>
    /// 게임의 일시정지 여부를 설정합니다.
    /// </summary>
    /// <param name="active">true일 때 일시정지</param>
    public void SetPause(bool active)
    {
        Time.timeScale = active ? 0.0f : 1.0f;
    }

    private IEnumerator CFadeOut(Callback c)
    {
        while (fadeImage.color.a < 1.0f)
        {
            Color newColor = fadeImage.color;
            newColor.a += Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        SetPause(true);
        yield return new WaitForSecondsRealtime(transitionDelay);
        if (c != null) c();
    }

    private IEnumerator CFadeIn(Callback c)
    {
        SetPause(false);
        while (fadeImage.color.a > 0)
        {
            Color newColor = fadeImage.color;
            newColor.a -= Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        if (c != null) c();
        isFading = false;
    }
}
