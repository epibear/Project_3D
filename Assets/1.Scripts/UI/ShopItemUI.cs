using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemUI : MonoBehaviour
{  
    private TalkSystem m_TalkSystem = null;

    [SerializeField]
    private Button m_btn_Buy = null;


    [SerializeField]
    private Text m_txt_Info = null;

    [SerializeField]
    private Image m_img_Icon = null;

    private void Start()
    {
        if (m_btn_Buy != null)
            m_btn_Buy.onClick.AddListener(OnClickBuy);
    }

    private Item m_item = null;
    public void InitItem(Item _item, TalkSystem _talk) {
        m_item = _item;       
        m_img_Icon.sprite = Storage.GetInstance().GetSpriteItem(m_item.m_ItemCode);

        m_txt_Info.text = string.Format("{1}\n{0}", m_item.m_Content, m_item.m_Name);
        m_btn_Buy.transform.GetChild(0).GetComponent<Text>().text= string.Format("구입\n{0}G",m_item.m_Price);
        m_TalkSystem = _talk;
        this.gameObject.SetActive(true);
    }

    private void OnClickBuy()
    {
        if (m_item == null) {
            Debug.Log("???");
            return;
        }
        if (m_item.m_Price > Storage.GetInstance().GetGold())      //골드 체크
        {
            GameManager.GetInstance().SetMessage("골드가 부족합니다");
            return;
        }

        Storage.GetInstance().SetGold(-m_item.m_Price);
        m_item.m_Amount += 1;
        Storage.GetInstance().AddItem(m_item);
        if(m_item.m_Type==ItemType.Posion)
            GameManager.GetInstance().UpdatePosion();
        GameManager.GetInstance().UpdateInventory();
        //골드 감소 
        SoundManager.GetInstance().PlayEffect("BuyItem");
        GameManager.GetInstance().SetMessage(string.Format("{0}을 구입했습니다",m_item.m_Name));
        m_TalkSystem.StartTalk(1);
    }

    public void Off() {
        this.gameObject.SetActive(false);
    }
}
