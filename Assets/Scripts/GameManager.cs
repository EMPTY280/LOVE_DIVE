using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private Image fadeImage = null;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float transitionDelay = 1.0f;

    private delegate void Callback();
    [SerializeField] private bool isFading = false;
    private string targetScene = "";

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameObject().AddComponent<GameManager>();
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
        Debug.Log("in");
        while (fadeImage.color.a < 1.0f)
        {
            Color newColor = fadeImage.color;
            newColor.a += Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(transitionDelay);
        if (c != null) c();
    }

    private IEnumerator CFadeIn(Callback c)
    {
        Debug.Log("out");
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
