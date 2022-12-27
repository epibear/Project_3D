using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillSlot : MonoBehaviour,IDropHandler
{
    private Skill m_Skill = null;
    [SerializeField]
    private Button m_btn_Action = null;
    [SerializeField]
    private Image m_img_Icon = null;

    [SerializeField]
    private Image m_img_Cover = null;
    [SerializeField]
    private Text m_txt_Timer = null;


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<SkillUI>()) {
            var _code = eventData.pointerDrag.GetComponent<SkillUI>().GetSkillCode();
            m_Skill = Storage.GetInstance().GetSkillinAllList(_code);
            m_img_Icon.sprite = eventData.pointerDrag.GetComponent<SkillUI>().GetSprite();
            m_img_Icon.enabled = true;
        }       
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_btn_Action != null)
            m_btn_Action.onClick.AddListener(OnClickAction);

        m_img_Icon.enabled = false;

        m_img_Cover.gameObject.SetActive(false);
    }

    private void OnClickAction() {
        //m_img_Cover.On
        //m_txt_Timer.text = Timer

        if (m_Skill == null)
            return;
        if (m_isActive)
            return;
                      
        var isOn = PlayerCtrl.GetInstance().OnActiveSkill(m_Skill.SkillCode);

        if (isOn)
            StartCoroutine(SkillCo());

    }

    private bool m_isActive = false;
    private float m_CoolTime = 0.0f;
    private IEnumerator SkillCo() {
        m_isActive = true;
        m_CoolTime = m_Skill.CoolTime;
        m_img_Cover.fillAmount = 1;
        m_img_Cover.gameObject.SetActive(true);
        while (m_CoolTime >= 0.0f) {
            m_img_Cover.fillAmount = m_CoolTime / m_Skill.CoolTime;
            m_txt_Timer.text = string.Format("{0}s", m_CoolTime.ToString("N1"));
            m_CoolTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        m_isActive = false;
        m_img_Cover.gameObject.SetActive(false);
        yield return null;
    }

    private void OnDisable()
    {
        if (m_isActive) {
            StopCoroutine(SkillCo());
            m_isActive = false;
            m_CoolTime = 0.0f;
            m_img_Cover.gameObject.SetActive(false);
        }

    }

}
