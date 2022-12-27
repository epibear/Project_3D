using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestCtrl : MonoBehaviour
{
    public Button m_btn_Close = null;

    private QuestUI[] m_Quests = null;

    public Transform m_QuestTr = null;
    public GameObject m_QuestObj = null;

    private bool m_isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(() => {
                this.gameObject.SetActive(false);
            });

        UpdateQuestState();

        m_isOn = true;
        this.gameObject.SetActive(false);
    }

    private void UpdateQuestState() {
        m_Quests = this.transform.GetComponentsInChildren<QuestUI>();

        var _list = Storage.GetInstance().GetAcceptQuestList();

        if (m_Quests.Length < _list.Count)
        {
            var count = _list.Count - m_Quests.Length;
            Debug.Log("퀘스트 확장 + " + count);
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(m_QuestObj);
                obj.transform.SetParent(m_QuestTr);
                obj.transform.localScale = Vector3.one;

            }
            m_Quests = this.transform.GetComponentsInChildren<QuestUI>();
        }

        for (int i = 0; i < _list.Count; i++)
        {
            m_Quests[i].InitQuest(_list[i]);
        }


    }

    private void OnEnable() {

        if (m_isOn) {

            UpdateQuestState(); //수정해야겟다 이러면 너무 자주 불리는데

            foreach (QuestUI _quest in m_Quests)
                _quest.UpdateState();        
        }
    }
}
