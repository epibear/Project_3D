using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
   // public Image m_img_Icon = null;
    public GameObject m_StateObj = null;
    public Text m_txt_State = null;

    public Text m_txt_Content = null;
   // public Text m_txt_Client = null;

    public Image m_img_Gage = null;
    public Text m_txt_Gage = null;

 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Quest m_Quest = null;
    public void InitQuest(Quest _Quest) {
        m_Quest = _Quest;

        m_txt_Content.text = m_Quest.Contents;


        if (m_Quest.isClear)
        {
            m_StateObj.SetActive(true);
            m_txt_State.text = "완료";
            m_img_Gage.fillAmount = 1;
            return;
        }
        
        if (m_Quest.isAccept)
        {
            m_StateObj.SetActive(true);
            m_txt_State.text = "진행중";
            m_img_Gage.fillAmount = m_Quest.Rate;
            m_txt_Gage.text = string.Format("{0}/{1}", m_Quest.Count, m_Quest.TargetCount);
        }
    }

    public void UpdateState() {
        if (m_Quest == null)
            return;

        if (m_Quest.isClear)
        {
            m_txt_State.text = "완료";
            m_img_Gage.fillAmount = 1;
            m_txt_Gage.text = string.Format("{0}/{1}", m_Quest.Count, m_Quest.TargetCount);
            return;
        }
        if (!m_StateObj.activeSelf)
            m_StateObj.SetActive(m_Quest.Count > 0 ? true :false);

        m_img_Gage.fillAmount = m_Quest.Rate;
        m_txt_Gage.text = string.Format("{0}/{1}", m_Quest.Count, m_Quest.TargetCount);
    }

}
