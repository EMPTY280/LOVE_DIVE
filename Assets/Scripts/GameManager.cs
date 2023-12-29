using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private Image fadeImage = null;
    private float fadeTime = 1.0f;
    private float transitionDelay = 2.0f;

    private delegate void Callback();
    [SerializeField] private bool isFading = false;

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
        DontDestroyOnLoad(gameObject);

        GameObject prefab = Resources.Load<GameObject>("Prefabs/ScreenEffect");
        GameObject inst = Instantiate(prefab);
        fadeImage = inst.transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// 페이드 아웃과 함께 씬을 전환합니다.
    /// </summary>
    /// <param name="name">전환한 씬의 이름입니다.</param>
    public void ChangeScene(string name)
    {
        if (isFading) return;
        isFading = true;
        StartCoroutine(CFadeOut( () => {
            SceneManager.LoadScene(name);
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
        c();
    }

    private IEnumerator CFadeIn(Callback c)
    {
        while (fadeImage.color.a > 0)
        {
            Color newColor = fadeImage.color;
            newColor.a -= Time.deltaTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        c();
        SetPause(false);
        isFading = false;
    }
}
