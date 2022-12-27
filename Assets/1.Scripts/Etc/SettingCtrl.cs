using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingCtrl : MonoBehaviour
{

    [SerializeField]
    private Button m_btn_Close = null;
    // Start is called before the first frame update
    void Start()
    {
        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });

        this.gameObject.SetActive(false);
    }    
}
