using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCtrl : MonoBehaviour
{
    private const int WEAPON = 0;
    private const int POSION = 1;

    [SerializeField]
    private TalkSystem m_TalkSystem = null; //대화 시스템


    [SerializeField]
    private GameObject m_SubCanvasObj = null; //상점UI
    [SerializeField]
    private GameObject m_SubCamera = null; //캐릭터 비출 카메라

    [SerializeField]
    private Button m_btn_Close = null; //상점 닫기
    [SerializeField]
    private Button[] m_btn_BuyG = null; //물건 구매 그룹 버튼
    [SerializeField]
    private Button m_btn_SellG = null;//물건 판매 그룹 버튼
    [SerializeField]
    private GameObject m_BuyObj = null;//구매할 목록 오브젝트버튼
    [SerializeField]
    private GameObject m_SellObj = null;

    [SerializeField]
    private GameObject m_ItemObj = null; //구매 아이템 오브젝트



    [SerializeField]
    private Transform m_ItemTr = null; //구매 목록 증가시 ui 배치를 위한 transform
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

            if (items.Count >= m_Items.Length)//확장
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

            if (items.Count >= m_Items.Length)//확장
            {
                var count = items.Count - m_Items.Length;
                for (int i = 0; i < count; i++) //확장
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
            else if (items.Count < m_Items.Length) {//축소
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
        var _invenitems = Storage.GetInstance().GetItemList(); //내 인벤 아이템
        Debug.Log(_invenitems.Count);
        if (_invenitems.Count >=m_SellItems.Length)//확장
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
    private void OnClickSellItem() { //아이템 전체 판매
        int totalPrice = 0;
        for (int i = 0; i < m_SellItems.Length; i++)
        {
            totalPrice += m_SellItems[i].SellItem();
             
        }

        if (totalPrice > 0) {
            m_TalkSystem.StartTalk(2);
            SoundManager.GetInstance().PlayEffect("SellItem");
            GameManager.GetInstance().SetMessage(string.Format("{0}골드를 받았습니다", totalPrice));
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
