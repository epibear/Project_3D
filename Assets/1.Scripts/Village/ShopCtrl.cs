using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCtrl : MonoBehaviour
{
    private const int WEAPON = 0;
    private const int POSION = 1;

    [SerializeField]
    private TalkSystem m_TalkSystem = null; //��ȭ �ý���


    [SerializeField]
    private GameObject m_SubCanvasObj = null; //����UI
    [SerializeField]
    private GameObject m_SubCamera = null; //ĳ���� ���� ī�޶�

    [SerializeField]
    private Button m_btn_Close = null; //���� �ݱ�
    [SerializeField]
    private Button[] m_btn_BuyG = null; //���� ���� �׷� ��ư
    [SerializeField]
    private Button m_btn_SellG = null;//���� �Ǹ� �׷� ��ư
    [SerializeField]
    private GameObject m_BuyObj = null;//������ ��� ������Ʈ��ư
    [SerializeField]
    private GameObject m_SellObj = null;

    [SerializeField]
    private GameObject m_ItemObj = null; //���� ������ ������Ʈ



    [SerializeField]
    private Transform m_ItemTr = null; //���� ��� ������ ui ��ġ�� ���� transform
    private ShopItemUI[] m_Items;//

    [SerializeField]
    private Transform m_SellItemTr = null; //
    private ShopSellItemUI[] m_SellItems;
    [SerializeField]
    private GameObject m_SellItemObj = null;
    [SerializeField]
    private Button m_btn_SellAllItem = null;


    [SerializeField]
    private Button m_btn_Back = null;

    [SerializeField]
    private Text m_txt_Gold = null;

    // Start is called before the first frame update
    void Start()
    {
      
        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(OnClickClose);


        if (m_btn_BuyG[WEAPON] != null)
            m_btn_BuyG[WEAPON].onClick.AddListener(()=> {
                OnClickBuy(WEAPON);
            });

        if (m_btn_BuyG[POSION] != null)
            m_btn_BuyG[POSION].onClick.AddListener(() => {
                OnClickBuy(POSION);
            });

        if (m_btn_SellAllItem != null)
            m_btn_SellAllItem.onClick.AddListener(OnClickSellItem);
        

        if (m_btn_SellG != null)
            m_btn_SellG.onClick.AddListener(OnClickSell);
        if (m_btn_Back != null)
            m_btn_Back.onClick.AddListener(OnClickBack);


        
        m_BuyObj.SetActive(false);
        m_SellObj.SetActive(false);
        m_SubCanvasObj.SetActive(false);
        m_SubCamera.SetActive(false);
    }
    private void OnClickBack() {
        m_BuyObj.SetActive(false);
        m_SellObj.SetActive(false);
        m_btn_Back.gameObject.SetActive(false);
        m_btn_SellAllItem.gameObject.SetActive(false);
    }
    private void OnClickClose() {
        m_SubCamera.SetActive(false);
        m_SubCanvasObj.SetActive(false);
        MainUIManager.GetInstance().OnMainCanvas();
        GameManager.GetInstance().EndEvent();
    }

    private void OnClickBuy(int _index) {
        m_BuyObj.SetActive(true);
        m_Items = m_BuyObj.transform.GetComponentsInChildren<ShopItemUI>();      
        UpdateShopItem(_index);
        m_btn_Back.gameObject.SetActive(true);
    }
    private void UpdateShopItem(int _index) {
        if (_index == WEAPON) {
            var items = Storage.GetInstance().GetItemList(ItemType.Weapon);

            if (items.Count >= m_Items.Length)//Ȯ��
            {                
                var count = items.Count - m_Items.Length;
                for (int i = 0; i < count; i++)
                {
                    var obj = Instantiate(m_ItemObj);
                    obj.transform.SetParent(m_ItemTr);
                }
                m_Items = m_BuyObj.transform.GetComponentsInChildren<ShopItemUI>();

                for (int i = 0; i < m_Items.Length; i++)
                {
                    m_Items[i].InitItem(items[i], m_TalkSystem);
                }
            }
            else if (items.Count < m_Items.Length)
            {
                var count = m_Items.Length - items.Count;
                for (int i = 0; i < m_Items.Length; i++)
                {
                    if (i >= m_Items.Length - count)
                        m_Items[i].gameObject.SetActive(false);
                    else
                        m_Items[i].InitItem(items[i], m_TalkSystem);
                }
            }      
        }
        if (_index == POSION)
        {
            Debug.Log("ad");
            var items = Storage.GetInstance().GetItemList(ItemType.Posion);

            if (items.Count >= m_Items.Length)//Ȯ��
            {
                var count = items.Count - m_Items.Length;
                for (int i = 0; i < count; i++) //Ȯ��
                {
                    var obj = Instantiate(m_ItemObj);
                    obj.transform.SetParent(m_ItemTr);
                }
                m_Items = m_ItemTr.transform.GetComponentsInChildren<ShopItemUI>();

                for (int i = 0; i < m_Items.Length; i++)
                {
                    m_Items[i].InitItem(items[i], m_TalkSystem);
                }
            }
            else if (items.Count < m_Items.Length) {//���
                var count = m_Items.Length - items.Count;
                for (int i = 0; i < m_Items.Length; i++)
                {
                    if (i >= m_Items.Length - count)
                        m_Items[i].Off();
                    else
                        m_Items[i].InitItem(items[i], m_TalkSystem);
                }
            }
   
              
        }
    }
    private void OnClickSell()
    {
        m_SellObj.SetActive(true);
        m_SellItems = m_SellObj.transform.GetComponentsInChildren<ShopSellItemUI>();
        UpdateSellItem();
        m_btn_SellAllItem.gameObject.SetActive(true);
        m_btn_Back.gameObject.SetActive(true);
    }
    private void UpdateSellItem() {
        var _invenitems = Storage.GetInstance().GetItemList(); //�� �κ� ������
        Debug.Log(_invenitems.Count);
        if (_invenitems.Count >=m_SellItems.Length)//Ȯ��
        {
            var count = _invenitems.Count - m_SellItems.Length;
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(m_SellItemObj);
                obj.transform.SetParent(m_SellItemTr);
            }
            m_SellItems = m_SellObj.transform.GetComponentsInChildren<ShopSellItemUI>();

            for (int i = 0; i < m_SellItems.Length; i++)
            {
                m_SellItems[i].InitItem(_invenitems[i]);
            }
        }
        else if (_invenitems.Count < m_SellItems.Length)
        {
            var count = m_SellItems.Length - _invenitems.Count;
            for (int i = 0; i < m_SellItems.Length; i++)
            {
                if (i >= m_SellItems.Length - count)
                    m_SellItems[i].Off();
                else
                    m_SellItems[i].InitItem(_invenitems[i]);
            }
        }

      
    }
    private void OnClickSellItem() { //������ ��ü �Ǹ�
        int totalPrice = 0;
        for (int i = 0; i < m_SellItems.Length; i++)
        {
            totalPrice += m_SellItems[i].SellItem();
             
        }

        if (totalPrice > 0) {
            m_TalkSystem.StartTalk(2);
            SoundManager.GetInstance().PlayEffect("SellItem");
            GameManager.GetInstance().SetMessage(string.Format("{0}��带 �޾ҽ��ϴ�", totalPrice));
            GameManager.GetInstance().UpdateInventory();
            Storage.GetInstance().SetGold(totalPrice);
           
        }            
    }

    private void UpdateGold() {
        m_txt_Gold.text = "x " + Storage.GetInstance().GetGold().ToString();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {

            //SubCamera On
            m_SubCamera.SetActive(true);
            m_SubCanvasObj.SetActive(true);
            MainUIManager.GetInstance().OffMainCanvas();
            m_btn_SellAllItem.gameObject.SetActive(false);
            UpdateGold();
            GameManager.GetInstance().ac_UpdateGold += UpdateGold;
            m_TalkSystem.StartTalk(0);
            GameManager.GetInstance().StartEvent();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_SubCamera.SetActive(false);
            m_SubCanvasObj.SetActive(false);
            m_BuyObj.SetActive(false);
            m_SellObj.SetActive(false);
            MainUIManager.GetInstance().OnMainCanvas();
            GameManager.GetInstance().ac_UpdateGold -= UpdateGold;
            GameManager.GetInstance().EndEvent();
        }
    }
}
