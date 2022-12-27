using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillCtrl : MonoBehaviour
{
    [SerializeField]
    private Image m_img_DrawableUI = null;
    public Button m_btn_Close = null;

    private SkillUI[] m_Skills = null;

    public Transform m_SkillTr = null;
    public GameObject m_SkillObj = null;

    private bool m_isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Skills = this.transform.GetComponentsInChildren<SkillUI>();

        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(() => {
                this.gameObject.SetActive(false);
            });

        var _list = Storage.GetInstance().GetMySkillList();

        if (m_Skills.Length < _list.Count)
        {

            var count = _list.Count - m_Skills.Length;
            Debug.Log("스킬 확장 + " + count);
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(m_SkillObj);
                obj.transform.SetParent(m_SkillTr);
                obj.transform.localScale = Vector3.one;

            }
            m_Skills = this.transform.GetComponentsInChildren<SkillUI>();
        }

        for (int i = 0; i < _list.Count; i++)
        {
            m_Skills[i].InitSkill(_list[i], m_img_DrawableUI);
        }

        m_isOn = true;
        this.gameObject.SetActive(false);
    }


    private void OnEnable()
    {

        if (m_isOn)
        {
            //스킬이 늘었을 경우 or 모든 스킬 감시 해서 획득했을 경우
        }
    }
}
