using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NPC
public class QuestItem : MonoBehaviour
{

    private TalkSystem m_TalkSystem = null;

    public Text m_txt_QuestType = null;
    public Image m_img_Icon = null;

    public Text m_txt_QuestContent = null;

    public GameObject m_ProgressObj = null;
    public Image m_img_Gage = null;
    public Text m_txt_Gage = null;


    public Button m_btn_Action= null;

    private bool m_isOn = false;

    private void Start()
    {
        if (m_btn_Action != null)
            m_btn_Action.onClick.AddListener(OnClickAction);
    }


    private Quest m_Quest = null;
    public void InitQuest(Quest _quest,TalkSystem _talk) {
        m_Quest = _quest;

        m_txt_QuestContent.text = m_Quest.Contents;

        m_txt_QuestType.text = m_Quest.QType == QuestType.Main ? "MAIN" : "SUB";

        m_img_Icon.sprite = m_Quest.ContentType == QuestContentType.KillMonster || m_Quest.ContentType == QuestContentType.KillMonster ?
            Storage.GetInstance().m_Resources.GetQuestIconSprite(0) : Storage.GetInstance().m_Resources.GetQuestIconSprite(1);
        m_TalkSystem = _talk;
        if (m_Quest.isReceiveReward) {
            this.gameObject.SetActive(false);
            m_Quest = null;
            return;
        }
      

        if (!m_Quest.isAccept)
        {
            m_btn_Action.gameObject.SetActive(true);
            m_ProgressObj.SetActive(false);
            return;
        }
        else {
            m_img_Gage.fillAmount = m_Quest.Rate;
            m_txt_Gage.text = string.Format("{0}/{1}", m_Quest.Count, m_Quest.TargetCount);
            m_btn_Action.gameObject.SetActive(true);
            m_ProgressObj.SetActive(true);

        }
        if (m_Quest.isClear && !m_Quest.isReceiveReward)
        {
            m_btn_Action.gameObject.SetActive(true);
            m_ProgressObj.SetActive(false);
            m_btn_Action.transform.GetChild(0).GetComponent<Text>().text = "보상 받기";
        }


        m_isOn = true;
    }
    public void OnClickAction() {
        if (!m_Quest.isAccept)
        {
            m_Quest.Accept();
            m_btn_Action.gameObject.SetActive(false);
            m_img_Gage.fillAmount = m_Quest.Rate;
            m_txt_Gage.text = string.Format("{0}/{1}", m_Quest.Count, m_Quest.TargetCount);
            m_ProgressObj.SetActive(true);
            GameManager.GetInstance().SetMessage("퀘스트를 승낙하였습니다");
            m_TalkSystem.StartTalk(1);
            return;
        }
        if (m_Quest.isClear && !m_Quest.isReceiveReward)
        {
            SoundManager.GetInstance().PlayEffect("QuestClear");
            GameManager.GetInstance().SetMessage("보상을 받았습니다");
            m_Quest.ReceiveReward();
            m_TalkSystem.StartTalk(2);
            this.gameObject.SetActive(false);
            m_Quest = null;

        }       
       
    }

    public void UpdateState() {
        if (m_Quest == null)
            return;
        if (!m_isOn)
            return;
        if (m_Quest.isAccept && !m_Quest.isClear) {//퀘스트 수락  //클리어x
            m_img_Gage.fillAmount = m_Quest.Rate;
            m_txt_Gage.text = string.Format("{0}/{1}", m_Quest.Count, m_Quest.TargetCount);
        }
        if (m_Quest.isClear&& !m_Quest.isReceiveReward) {//퀘스트 클리어 보상 미수령
            if (!m_btn_Action.gameObject.activeSelf) {
                m_btn_Action.gameObject.SetActive(true);
                m_btn_Action.transform.GetChild(0).GetComponent<Text>().text = "보상 받기";
            }            
        }
       
    }
}
