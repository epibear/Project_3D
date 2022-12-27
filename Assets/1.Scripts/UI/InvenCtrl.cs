using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenCtrl : MonoBehaviour
{

    public Button m_btn_Close = null;


    private InvenItem[] m_Items = null;

    [SerializeField]
    private Text m_txt_Gold = null;

    private bool m_isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Items = this.transform.GetComponentsInChildren<InvenItem>();
        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(OnClickClose);

        GameManager.GetInstance().ac_UpdateInventory += UpdateInventory;

        var _list = Storage.GetInstance().GetItemList();

        if (_list.Count <= m_Items.Length)
        {
            for (int i = 0; i < _list.Count; i++)
                m_Items[i].InitItem(_list[i]);
        }

        m_txt_Gold.text = "x " + Storage.GetInstance().GetGold();
        m_isOn = true;
        this.gameObject.SetActive(false);
    }

    private void OnClickClose() {
        this.gameObject.SetActive(false);
    }

    private void UpdateInventory() {
        var _list = Storage.GetInstance().GetItemList();

        if (_list.Count <= m_Items.Length)
        {
            for (int i = 0; i < m_Items.Length; i++)
                if (i < _list.Count)
                    m_Items[i].InitItem(_list[i]);
                else
                    m_Items[i].Off();

        }
    }

    private void OnEnable()
    {
        if (m_isOn) {
            m_txt_Gold.text = "x " + Storage.GetInstance().GetGold();
        }

    }

}
