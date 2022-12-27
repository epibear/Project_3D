using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillUI : MonoBehaviour,  IPointerDownHandler, IDragHandler, IEndDragHandler
{

    private Image m_img_DragableIcon = null;

    public Image m_img_Icon = null;
    public Text m_txt_Info = null;
    public Text m_txt_Name = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    private Skill m_Skill = null;
    public void InitSkill(Skill _skill, Image _obj)
    {
        m_Skill = _skill;

        m_txt_Name.text = string.Format("½ºÅ³ : {0}", m_Skill.Name);

        m_txt_Info.text = string.Format(m_Skill.Info, (m_Skill.DamageRate * 100).ToString("N0"));

        m_img_DragableIcon = _obj;

        m_img_Icon.sprite = Storage.GetInstance().m_Resources.GetSkillSprite(m_Skill.SpiteIndex);
    }




    private bool m_isDragOn = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        m_isDragOn = true;
        m_img_DragableIcon.transform.position = eventData.position;
        m_img_DragableIcon.gameObject.SetActive(true);
        m_img_DragableIcon.sprite = this.m_img_Icon.sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_isDragOn)
        {
            m_img_DragableIcon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_isDragOn) {
            m_img_DragableIcon.transform.position = Vector3.zero;
            m_img_DragableIcon.gameObject.SetActive(false);
        }          
    }

    public string GetSkillCode()
    {
        return m_Skill.SkillCode;
    }
    public Sprite GetSprite()
    {
        return m_img_Icon.sprite;
    }
}