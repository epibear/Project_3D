using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvenItem : MonoBehaviour,IPointerDownHandler,IDragHandler,IEndDragHandler,IPointerUpHandler ,IPointerEnterHandler ,IPointerExitHandler
{
    public GameObject m_DragObj = null;
    public GameObject m_EquipObj = null;
    public Text m_txt_Amount = null;
    public Image m_img_Icon = null;

    //---

    [SerializeField,Header("ItemInfo")]
    private GameObject m_HoverItemObj = null;
    [SerializeField]
    private Text m_txt_HoverItemName = null;
    [SerializeField]
    private Text m_txt_HoverItemInfo = null;
    [SerializeField]
    private Text m_txt_HoverItemPrice = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    private ItemType m_Type = ItemType.None;
    private int m_ItemCode;
    //Image
    //DrageAble


    private bool m_isEquip = false;
    private Item m_item = null;

    public void InitItem(Item _item) {
        m_item = _item;
        m_Type = _item.m_Type;
        m_ItemCode = _item.m_ItemCode;
        switch (m_Type) {
            case ItemType.Weapon:
                {
                    m_isEquip = ((Weapon)_item).isEquip;
                    m_EquipObj.SetActive(m_isEquip);
                }
                break;
            case ItemType.Posion:
                {
                    m_isEquip = false;
                    GameManager.GetInstance().ac_UpdatePosion += UpdatePosion;
                    m_EquipObj.SetActive(false);
                }
                break;                
        }
        if (_item.m_Amount > 0)
            m_txt_Amount.gameObject.SetActive(true);
        else
            m_txt_Amount.gameObject.SetActive(false);


        m_txt_Amount.text = _item.m_Amount.ToString();

        m_img_Icon.sprite = Storage.GetInstance().GetSpriteItem(_item.m_ItemCode);
        m_img_Icon.gameObject.SetActive(true);
    }


    private bool m_isDragOn = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_item == null)
            return;

            if (m_Type == ItemType.Weapon&& m_isEquip || m_Type==ItemType.None) {
            m_isDragOn = false;                   
        }
        else
        {
            m_isDragOn = true;
            m_DragObj.SetActive(true);
            m_DragObj.transform.position = this.transform.position;
            m_DragObj.GetComponent<Image>().sprite = this.m_img_Icon.sprite;
        }
        if (m_HoverItemObj.activeSelf)
            m_HoverItemObj.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_isDragOn = false;
        m_DragObj.SetActive(false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (m_isDragOn)
        {
            m_DragObj.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_isDragOn)
        {
            m_DragObj.SetActive(false);
            if(m_HoverItemObj.activeSelf)
                m_HoverItemObj.SetActive(true);
        }
       
    }

    public int GetItemCode() {
        return m_ItemCode;
    }
    public Sprite GetItemSprite()
    {
        return m_img_Icon.sprite;
    }

    private void UpdatePosion() {
        if (m_item!= null)
        {
            m_txt_Amount.text = m_item.m_Amount.ToString();
            if (m_item.m_Amount <= 0)
            {
                m_item = null;
                GameManager.GetInstance().ac_UpdatePosion -= UpdatePosion;
                GameManager.GetInstance().UpdateInventory();
            }
        }     
    }

    public void Off() {
        if(m_item!=null && m_item.m_Type==ItemType.Posion)
                GameManager.GetInstance().ac_UpdatePosion -= UpdatePosion;

        m_item = null;
        m_isEquip = false;
        m_EquipObj.SetActive(m_isEquip);     
        m_txt_Amount.gameObject.SetActive(false);
        m_img_Icon.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(m_item!=null)
        if (!m_isDragOn) {
            m_HoverItemObj.transform.position = this.transform.position + new Vector3(150, -150, 0);
            m_HoverItemObj.SetActive(true);
            m_txt_HoverItemName.text = m_item.m_Name;
            m_txt_HoverItemInfo.text = m_item.m_Content;
            m_txt_HoverItemPrice.text = string.Format("»óÁ¡°¡ : {0}", m_item.m_Price);
        }       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_item != null)
            if (!m_isDragOn)
                m_HoverItemObj.SetActive(false);
    }
}
