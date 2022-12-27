using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class StatusSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IDropHandler {

    public Image m_img_Icon = null;

    private int m_previosEquipItemCode = 0;
    private int m_NowEquipItemCode = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Storage.GetInstance().g_EquipWeapon != null)
        {

            m_NowEquipItemCode = Storage.GetInstance().g_EquipWeapon.m_ItemCode;
            m_img_Icon.sprite = Storage.GetInstance().GetSpriteItem(m_NowEquipItemCode);
        }
    }

    public void ChangeEquipIcon(Sprite _sprite) {
        m_img_Icon.sprite = _sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_img_Icon.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_img_Icon.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            var _ItemCode = eventData.pointerDrag.transform.GetComponent<InvenItem>().GetItemCode();

            m_img_Icon.sprite = eventData.pointerDrag.transform.GetComponent<InvenItem>().GetItemSprite();
            //Player Item Equip On
            m_previosEquipItemCode = m_NowEquipItemCode;
            m_NowEquipItemCode = _ItemCode;
            Storage.GetInstance().EquipItem(_ItemCode);
            GameManager.GetInstance().UpdateStatus();
        }
    }
}
