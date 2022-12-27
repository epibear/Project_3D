using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PosionSlot : MonoBehaviour, IDropHandler
{
    private Posion m_Posion = null;
    [SerializeField]
    private Button m_btn_Action = null;
    [SerializeField]
    private Image m_img_Icon = null;

    [SerializeField]
    private Text m_txt_Amount = null;    

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<InvenItem>())
        {
            var _code = eventData.pointerDrag.GetComponent<InvenItem>().GetItemCode();
            m_Posion =(Posion)Storage.GetInstance().GetItem(_code);
            m_img_Icon.sprite = eventData.pointerDrag.GetComponent<InvenItem>().GetItemSprite();
            m_img_Icon.gameObject.SetActive(true);
            UpdatePosion();
            m_txt_Amount.enabled = true;
            GameManager.GetInstance().ac_UpdatePosion += UpdatePosion;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_btn_Action != null)
            m_btn_Action.onClick.AddListener(OnClickAction);

        m_txt_Amount.enabled = false;
        m_img_Icon.gameObject.SetActive(false);
    }

    private void OnClickAction()
    {
        if (m_Posion == null)
            return;

        m_Posion.m_Amount -= 1;

        PlayerCtrl.GetInstance().UsePosion(m_Posion);
        GameManager.GetInstance().UpdatePosion();

        if (m_Posion.m_Amount <= 0)
        {
            m_txt_Amount.enabled = false;
            m_img_Icon.gameObject.SetActive(false);
            m_Posion = null;          
            GameManager.GetInstance().ac_UpdatePosion -= UpdatePosion;            
            return;
        }
    }

    private void UpdatePosion()
    {
        m_txt_Amount.text = m_Posion.m_Amount.ToString();
    }
}
