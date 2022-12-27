using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VolumeType
{
    None,
    BGM,
    EFFECT,
    UI
}
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    public static SoundManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
        DontDestroyOnLoad(this);
    }

    public Dictionary<string, AudioClip> Sounds = new Dictionary<string, AudioClip>();

    private const int MAX_EFFECT = 10;
    private const int MAX_UI = 5;


    private List<AudioSource> EffectSounds = new List<AudioSource>();
    private List<AudioSource> UISounds = new List<AudioSource>();

    private AudioSource BgmSound = null;


    private const string NameOfObject_1 = "EffectSound";
    private const string NameOfObject_2 = "UISound";
    private const string NameOfObject_3 = "BgmSound";

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < MAX_EFFECT; i++)
        {
            GameObject _new = new GameObject();
            _new.AddComponent<AudioSource>();
            _new.name = NameOfObject_1;
            _new.transform.SetParent(this.transform);
            EffectSounds.Add(_new.GetComponent<AudioSource>());

        }
        for (int i = 0; i < MAX_UI; i++)
        {
            GameObject _new = new GameObject();
            _new.AddComponent<AudioSource>();
            _new.name = NameOfObject_2;
            _new.transform.SetParent(this.transform);
            UISounds.Add(_new.GetComponent<AudioSource>());
        }

        GameObject _bgm = new GameObject();
        _bgm.AddComponent<AudioSource>();
        _bgm.name = NameOfObject_3;
        _bgm.transform.SetParent(this.transform);
        BgmSound = _bgm.GetComponent<AudioSource>();
    }


    public void PlayEffect(string _clip)
    {
        for (int i = 0; i < EffectSounds.Count; i++)
        {
            if (!EffectSounds[i].isPlaying)
            {
                EffectSounds[i].PlayOneShot(GetAudioClip(_clip));
                return;
            }
        }
        GameObject _new = new GameObject();
        _new.AddComponent<AudioSource>();
        EffectSounds.Add(_new.GetComponent<AudioSource>());

        EffectSounds[EffectSounds.Count - 1].PlayOneShot(GetAudioClip(_clip));

    }
 
    public void PlayBgm(string _clip)
    {
        BgmSound.clip = GetAudioClip(_clip);
        BgmSound.Play();
        BgmSound.loop=true;
      //  BgmSound.PlayOneShot(GetAudioClip(_clip));
    }
    public void PlayUISound(string _clip)
    {
        for (int i = 0; i < UISounds.Count; i++)
        {
            if (!UISounds[i].isPlaying)
            {
                UISounds[i].PlayOneShot(GetAudioClip(_clip));
                return;
            }
        }
        GameObject _new = new GameObject();
        _new.AddComponent<AudioSource>();
        UISounds.Add(_new.GetComponent<AudioSource>());

        UISounds[UISounds.Count - 1].PlayOneShot(GetAudioClip(_clip));
    }

    private AudioClip GetAudioClip(string _clip)
    {
        if (!Sounds.ContainsKey(_clip))
        {
            Sounds.Add(_clip, Resources.Load<AudioClip>("Sounds/" + _clip));
        }

        return Sounds[_clip];
    }

    public void SetVolume(VolumeType _type, float _value) {

        switch (_type) {
            case VolumeType.BGM:
                BgmSound.volume = _value;
                break;
            case VolumeType.EFFECT:
                foreach (AudioSource _source in EffectSounds) {
                    _source.volume = _value;
                } 
                break;
            case VolumeType.UI:
                foreach (AudioSource _source in UISounds)
                {
                    _source.volume = _value;
                }
                break;


        }
    }
    public void SetVolume(VolumeType _type, bool _isOn)
    {
        switch (_type)
        {
            case VolumeType.BGM:
                BgmSound.mute = !_isOn;
                break;
            case VolumeType.EFFECT:
                foreach (AudioSource _source in EffectSounds)
                {
                    _source.mute = !_isOn;
                }
                break;
            case VolumeType.UI:
                foreach (AudioSource _source in UISounds)
                {
                    _source.mute = !_isOn;
                }
                break;


        }

    }
}
