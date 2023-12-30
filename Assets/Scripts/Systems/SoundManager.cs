using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private float volumeMaster = 0.5f;
    private float volumeBGM = 1.0f;
    private float volumeSFX = 1.0f;

    [SerializeField] AudioClip[] BGMList;
    Dictionary<string, int> BGMKeys = new Dictionary<string, int>();
    [SerializeField] AudioClip[] SFXList;
    Dictionary<string, int> SFXKeys = new Dictionary<string, int>();

    AudioSource BGMchannel = null;
    Queue<AudioSource> SFXchannels = new Queue<AudioSource>();
    const int SFXcount = 15;

    private static SoundManager instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<SoundManager>();
                instance.name = "[ SoundManager ]";
            }
            return instance;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);

        // CREATE AUDIO CHANNELS
        BGMchannel = gameObject.AddComponent<AudioSource>();
        BGMchannel.loop = true;
        for (int i = 0; i < SFXcount; i++)
        {
            AudioSource channel = gameObject.AddComponent<AudioSource>();
            channel.loop = false;
            SFXchannels.Enqueue(channel);
        }
        UpdateVolume();

        // LOAD SOUNDS
        BGMList = Resources.LoadAll<AudioClip>("BGM");
        SFXList = Resources.LoadAll<AudioClip>("SFX");

        // CREATE KEYS + PRELOAD
        for (int i = 0; i < BGMList.Length; i++)
        {
            BGMKeys.Add(BGMList[i].name, i);
            BGMList[i].LoadAudioData();
        }
        for (int i = 0; i < SFXList.Length; i++)
        {
            SFXKeys.Add(SFXList[i].name, i);
            SFXList[i].LoadAudioData();
        }
    }

    public void SetVolumeMaster(float v)
    {
        volumeMaster = v;
        UpdateVolume();
    }
    public void SetVolumeBGM(float v)
    {
        volumeBGM = v;
        UpdateVolume();
    }
    public void SetVolumeSFX(float v)
    {
        volumeSFX = v;
        UpdateVolume();
    }

    public float GetVolumeMaster()
    {
        return volumeMaster;
    }
    public float GetVolumeBGM()
    {
        return volumeBGM;
    }
    public float GetVolumeSFX()
    {
        return volumeSFX;
    }

    private void UpdateVolume()
    {
        BGMchannel.volume = volumeMaster * volumeBGM;
        for (int i = 0; i < SFXcount; i++)
        {
            AudioSource channel = SFXchannels.Dequeue();
            channel.volume = volumeMaster * volumeSFX;
            SFXchannels.Enqueue(channel);
        }
    }

    private AudioClip GetBGM(string name)
    {
        if (BGMKeys.ContainsKey(name))
            return BGMList[BGMKeys[name]];
        else
            return BGMList[0];
    }

    private AudioClip GetSFX(string name)
    {
        if (SFXKeys.ContainsKey(name))
            return SFXList[SFXKeys[name]];
        else
            return SFXList[0];
    }

    public void PlayBGM(string name)
    {
        BGMchannel.clip = GetBGM(name);
        BGMchannel.Play();
    }

    public void PlaySFX(string name)
    {
        if (SFXchannels.Peek().isPlaying)
            return;

        AudioSource channel = SFXchannels.Dequeue();
        channel.clip = GetSFX(name);
        channel.Play();
        SFXchannels.Enqueue(channel);
    }
}
