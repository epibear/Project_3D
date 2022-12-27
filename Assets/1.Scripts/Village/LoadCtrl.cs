using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCtrl : MonoBehaviour
{

    [SerializeField]
    private TalkSystem m_TalkSystem = null;

    [SerializeField]
    private string m_NpcCode;

    public GameObject m_SubCanvasObj = null;
    public GameObject m_SubCamera = null;

    public Button m_btn_Close = null;

    [SerializeField]
    private Transform m_QuestTr = null;

    private QuestItem[] m_Quests = null;
    [SerializeField]
    private GameObject m_QuestObj = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(OnClickClose);

        m_Quests = m_QuestTr.GetComponentsInChildren<QuestItem>();

        var _list = Storage.GetInstance().GetNpcQuestList(m_NpcCode);

        if (m_Quests.Length < _list.Count)
        {
           
            var count = _list.Count - m_Quests.Length;          
            for (int i = 0; i < count; i++) {
                var obj = Instantiate(m_QuestObj);
                obj.transform.SetParent(m_QuestTr);
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.identity;

            }
            m_Quests = m_QuestTr.GetComponentsInChildren<QuestItem>();
        }

        for (int i = 0; i < _list.Count; i++) {
            m_Quests[i].InitQuest(_list[i], m_TalkSystem);
        }

        m_SubCamera.SetActive(false);
        m_SubCanvasObj.SetActive(false);
    }


    private void OnClickClose()
    {
        m_SubCamera.SetActive(false);
        m_SubCanvasObj.SetActive(false);
        MainUIManager.GetInstance().OnMainCanvas();
        GameManager.GetInstance().EndEvent();
    }

    private void UpdateQuestState() {
        foreach (QuestItem _quest in m_Quests) {
            _quest.UpdateState();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UpdateQuestState();
            //SubCamera On
            m_SubCamera.SetActive(true);
            m_SubCanvasObj.SetActive(true);
            MainUIManager.GetInstance().OffMainCanvas();
            m_TalkSystem.StartTalk(0);
            GameManager.GetInstance().StartEvent();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnClickClose();
            GameManager.GetInstance().EndEvent();
        }

    }
}
