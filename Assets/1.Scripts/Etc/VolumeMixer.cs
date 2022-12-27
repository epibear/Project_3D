using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeMixer : MonoBehaviour
{
    [SerializeField,Header("VolumeType")]
    private VolumeType m_Type;

    [SerializeField]
    private Text m_txt_Nams = null;

    [SerializeField]
    private Toggle m_tgl_SoundOn = null;

    [SerializeField]
    private Slider m_slider_Volume = null;

    // Start is called before the first frame update
    void Start()
    {
        m_txt_Nams.text = m_Type.ToString();
        if (m_tgl_SoundOn != null)
            m_tgl_SoundOn.onValueChanged.AddListener(OnChangedSoundOnOff);

        if (m_slider_Volume != null)
            m_slider_Volume.onValueChanged.AddListener(OnChangedVolume);
    }

    private void OnChangedSoundOnOff(bool _isOn) {
        SoundManager.GetInstance().SetVolume(m_Type, _isOn);
    }

    private void OnChangedVolume(float _value) {
        SoundManager.GetInstance().SetVolume(m_Type, _value);
    }

}
