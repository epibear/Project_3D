using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusCtrl : MonoBehaviour
{
    private const int ATTACK = 0;
    private const int HP = 1;
    private const int MP =2;
    public Text m_txt_Gold = null;

    private bool m_isOn = false;

    public Button m_btn_Close = null;

    public StatusSlot m_slot = null;
    

    [SerializeField]
    private Text[] m_txt_Status = null;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetInstance().ac_UpdateStatus += UpdateStatus;

        //장착중인 아이템 정보를 알아야한다?
        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(() => {
                this.gameObject.SetActive(false);
            });


        UpdateStatus();




        m_isOn = false;
        this.gameObject.SetActive(false);
    }

    public void UpdateStatus() {

        m_txt_Status[ATTACK].text =Storage.GetInstance().GetDamage().ToString();
        m_txt_Status[HP].text = string.Format("{0}/{1}", PlayerCtrl.GetInstance().GetCurHp(), PlayerCtrl.GetInstance().GetMaxHp());
        m_txt_Status[MP].text = string.Format("{0}/{1}", PlayerCtrl.GetInstance().GetCurMp(), PlayerCtrl.GetInstance().GetMaxMp());

    }

    private void OnEnable()
    {
        if (m_isOn) { 
        
        
        }
        
    }


}
