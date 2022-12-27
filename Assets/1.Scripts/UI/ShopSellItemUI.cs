using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopSellItemUI : MonoBehaviour
{
    [SerializeField]
    private Button m_btn_Minus= null;
    [SerializeField]
    private Button m_btn_Add= null;
    [SerializeField]
    private Text m_txt_Amount = null;


    [SerializeField]
    private Text m_txt_Info = null;

    [SerializeField]
    private Image m_img_Icon = null;


    private int m_Amount = 0;

    private void Start()
    {
        if (m_btn_Minus != null)
            m_btn_Minus.onClick.AddListener(()=> {
                OnClickAdd(-1);
            });
        if (m_btn_Add != null)
            m_btn_Add.onClick.AddListener(()=> {
                OnClickAdd(1);
            });
    }

    private Item m_item = null;
    public void InitItem(Item _item)
    {
        m_item = _item;
      
        m_img_Icon.sprite = Storage.GetInstance().GetSpriteItem(m_item.m_ItemCode);

        m_txt_Info.text = string.Format("{1}\n{0}\nPrice : {2}", m_item.m_Content, m_item.m_Name, m_item.m_Price * 0.5f);
        m_Amount = 0;
        m_txt_Amount.text = string.Format("{0}/{1}", m_Amount, m_item.m_Amount);

        this.gameObject.SetActive(true);
    }

    private void OnClickAdd(int _index)
    {
        m_Amount += _index;
        if (m_Amount < 0) {
            GameManager.GetInstance().SetMessage("수량은 0이하로 내려갈수없습니다");
            m_Amount = 0;
        }
        if (m_Amount > m_item.m_Amount) {
            GameManager.GetInstance().SetMessage("최대치입니다");
            m_Amount = m_item.m_Amount;
        }

        m_txt_Amount.text = string.Format("{0}/{1}", m_Amount, m_item.m_Amount);
    }
    public int SellItem() {
        if (m_Amount != 0) { 
            m_item.m_Amount -= m_Amount;
            var _price = m_Amount * (int)(m_item.m_Price * 0.5f);         
            m_Amount = 0;
            m_txt_Amount.text = m_Amount.ToString();
            if (m_item.m_Amount <= 0)
                Off();

            m_txt_Amount.text = string.Format("{0}/{1}", m_Amount, m_item.m_Amount);

            return _price;
        }

        return 0;
    }
    public void Off()
    {
        this.gameObject.SetActive(false);
    }
}
