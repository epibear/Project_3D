using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUIManager : MonoBehaviour
{ 
    private static MainUIManager m_instance = null;
    public static MainUIManager GetInstance()
    {
        return m_instance;
    }

 
    public Button m_btn_BaseAttack = null;

    public Text m_txt_Gold = null;

    [SerializeField]
    private MinimapCamera m_MinimapCam = null;
    public Button m_btn_MiniMapPlus = null;
    public Button m_btn_MiniMapMinus = null;


    public Text m_txt_Level = null;

    private void OnClickMininMapBtns(int _index) {
        m_MinimapCam.ChangSize(_index);
    }

    public void UpdateGold() {
        m_txt_Gold.text = "x " + Storage.GetInstance().GetGold().ToString();
    }
    public void UpdateLevel(int _level)
    {
        m_txt_Level.text = _level.ToString();
    }
    public Image m_img_ExpBar = null;
    public Text m_txt_Exp = null;
    public void UpdateExp(int _cur, int _max, float _rate) {
        m_txt_Exp.text = string.Format("{0} / {1} ({2}%)", _cur, _max, _rate.ToString("N0"));
        m_img_ExpBar.fillAmount = _rate;
    }

    private void Awake()
    {
        if (m_instance != null)
            Destroy(m_instance);

        m_instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (m_btn_BaseAttack != null)
            m_btn_BaseAttack.onClick.AddListener(OnClickAttack);

        if (m_btn_Inven != null)
            m_btn_Inven.onClick.AddListener(OnClickOpenInvenObject);
        if (m_btn_Quest != null)
            m_btn_Quest.onClick.AddListener(OnClickOpenQuestObject);
        if (m_btn_Skill != null)
            m_btn_Skill.onClick.AddListener(OnClickOpenSkillObject);
        if (m_btn_Setting != null)
            m_btn_Setting.onClick.AddListener(OnClickOpenSettingObject);

        if (m_btn_MiniMapPlus != null)
            m_btn_MiniMapPlus.onClick.AddListener(()=> {
                OnClickMininMapBtns(-1);
            });
        if (m_btn_MiniMapMinus!= null)
            m_btn_MiniMapMinus.onClick.AddListener(() => {
                OnClickMininMapBtns(+1);
            });

        GameManager.GetInstance().ac_UpdateGold += UpdateGold;
    }

    [SerializeField]
    private GameObject m_MainCanvas = null;
    public void OffMainCanvas() { //SubCanvas is On? = Off
        m_MainCanvas.SetActive(false);
    }
    public void OnMainCanvas()
    {
        m_MainCanvas.SetActive(true);
    }
    private void OnClickAttack()
    {
        PlayerCtrl.GetInstance().OnAttack();
    }

 

    private bool m_isTakeDam = false;

    public Image m_DamHpBar = null;
    public Image m_CurHpBar = null;

    public Text m_txt_CurHp = null;


    public Image m_MpBar = null;
    public Text m_txt_Mp = null;


    // Update is called once per frame
    void Update()
    {
        if (m_isTakeDam)
            m_DamHpBar.fillAmount = Mathf.Lerp(m_DamHpBar.fillAmount, m_CurHpBar.fillAmount, Time.deltaTime * 2.0f);
    }
    public void UpdateMp(int _mans, float _rate) {
        m_MpBar.fillAmount = _rate;
        m_txt_Mp.text= _mans.ToString();
    }

    public void UpdateHp(int _hp, float _rate) {
        m_txt_CurHp.text = _hp.ToString();
        m_isTakeDam = true;
        m_CurHpBar.fillAmount = Mathf.Clamp(_rate, 0f, 1f);
    }
    [Header("Inven Object")]
    public GameObject m_InvenObj = null;
    public GameObject m_StatusObj = null;
    public Button m_btn_Inven = null;
    private void OnClickOpenInvenObject() {
        m_InvenObj.SetActive(true);
        m_StatusObj.SetActive(true);
    }

    public GameObject m_QuestObj = null;
    public Button m_btn_Quest = null;
    private void OnClickOpenQuestObject()
    {
        m_QuestObj.SetActive(true);
    }

    [Header("Skill Object")]
    public GameObject m_SkillObj = null;
    public Button m_btn_Skill = null;
    private void OnClickOpenSkillObject()
    {
        m_SkillObj.SetActive(true);
    }

    [Header("Setting Object")]
    public GameObject m_SettingObj = null;
    public Button m_btn_Setting = null;
    private void OnClickOpenSettingObject()
    {
        m_SettingObj.SetActive(true);
    }
    
}
