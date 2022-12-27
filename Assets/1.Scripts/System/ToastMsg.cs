using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastMsg : MonoBehaviour
{
    [SerializeField]
    private Text m_txt_Msg = null;
    [SerializeField]
    private Animator m_animator = null;

    private Color m_AccentColor = Color.red;
    private Color m_BaseColor = Color.white;
 
    public void SetMessage(string _msg, bool _point=false) {
        if (_point)
        {
            m_txt_Msg.color = m_AccentColor;
            m_txt_Msg.fontSize = 45;
        }
        else {
            m_txt_Msg.color = m_BaseColor;
            m_txt_Msg.fontSize = 37;
        }
        m_txt_Msg.text = _msg;
        this.gameObject.SetActive(true);
        m_animator.Play("ToastMessage");
    }
    public void SetOff() {
        this.gameObject.SetActive(false);
    }
 
}
