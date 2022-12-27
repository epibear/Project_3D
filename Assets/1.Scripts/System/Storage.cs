using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Storage : MonoBehaviour
{
    private static Storage instance = null;
    public static Storage GetInstance() {

        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
    #region STATUS & BASE
    private int g_Gold = 50000;

    public void SetGold(int _gold) {
        g_Gold += _gold;
        GameManager.GetInstance().UpdateGold();
    }

    public int GetGold() {
        return g_Gold;
    }
    private int m_baseDamage = 10;
    
    public int GetDamage() {
        var dam = m_baseDamage;

        if (g_EquipWeapon!=null)
            dam = m_baseDamage + g_EquipWeapon.Damage;
      

        return dam;
    }
    #endregion

    public ResoucesConfig m_Resources = null;
    // Start is called before the first frame update
    void Start()
    {
        //Item List Setting -csv? or ?




        AddSprite();
        AddAllQuest();
        AddAllItem();
        AddAllSkill();
    }



    #region ITEM
    //모든 아이템- > 코드 받아서 내 아이템으로 amount 변경
    private Dictionary<int, Item> g_Items = new Dictionary<int, Item>();
    private List<Item> g_InvenItem = new List<Item>();
    private void AddAllItem() {
        g_Items.Add(1010, new Weapon(1010,"짧은 검","무기로 사용하기엔 조금 짧은 검",5,100));
        g_Items.Add(1011, new Weapon(1011, "날카로운 장검", "매우 날카로운 장검", 15, 500));
        g_Items.Add(1012, new Weapon(1012, "시미터", "베기에 적당한 사막전사의 검", 35, 1500));
        g_Items.Add(1013, new Weapon(1013, "광휘의 검", "빛나는 유물의 검", 100, 5000));

        g_Items.Add(2010, new Posion(2010, "초급 체력 포션", "사용시 체력의 30%를 회복한다", 0.3f, true, 50));
        g_Items.Add(2011, new Posion(2011, "중급 체력 포션", "사용시 체력의 50%를 회복한다", 0.5f, true, 150));
        g_Items.Add(2012, new Posion(2012, "상급 체력 포션", "사용시 체력의 100%를 회복한다", 1f, true, 350));

        g_Items.Add(2020, new Posion(2020, "초급 마나 포션", "사용시 마나의 30%를 회복한다", 0.3f, false, 100));
        g_Items.Add(2021, new Posion(2021, "중급 마나 포션", "사용시 마나의 50%를 회복한다", 0.5f, false, 250));

        var item = GetItem(1010);
        item.m_Amount = 1;
     
        g_InvenItem.Add(item);
        g_EquipWeapon = (Weapon)GetItem(1010);
        g_EquipWeapon.Equip();
    }

    
    public void AddItem(Item _item)
    {
        if (!g_InvenItem.Contains(_item))
            g_InvenItem.Add(_item);   

    }
    public Item GetItem(int _code) {
        if (!g_Items.ContainsKey(_code))
            return null;

        return g_Items[_code];
    }

    public Item GetAllItemList(int _code)
    {
        if (!g_Items.ContainsKey(_code))
            return null;

        return g_Items[_code];
    }
 
    public List<Item> GetItemList()
    {
        return g_InvenItem.Where(item=>item.m_Amount>0).ToList();
    }
    public List<Item> GetItemList(ItemType _type)
    {
        return g_Items.Values.ToList().Where(item => item.m_Type == _type).ToList();
    }

    #endregion


    //-------------------------------



    private Dictionary<int, Sprite> g_Sprite = new Dictionary<int, Sprite>();
    public Sprite GetSpriteItem(int _code) {
        if (!g_Sprite.ContainsKey(_code))
            return null;

        return g_Sprite[_code];
    }

    private void AddSprite() {

        g_Sprite.Add(1010, Resources.Load<Sprite>("Sprites/1010"));
        g_Sprite.Add(1011, Resources.Load<Sprite>("Sprites/1011"));
        g_Sprite.Add(1012, Resources.Load<Sprite>("Sprites/1012"));
        g_Sprite.Add(1013, Resources.Load<Sprite>("Sprites/1013"));

        g_Sprite.Add(2010, Resources.Load<Sprite>("Sprites/2010"));
        g_Sprite.Add(2011, Resources.Load<Sprite>("Sprites/2011"));
        g_Sprite.Add(2012, Resources.Load<Sprite>("Sprites/2012"));

        g_Sprite.Add(2020, Resources.Load<Sprite>("Sprites/2020"));
        g_Sprite.Add(2021, Resources.Load<Sprite>("Sprites/2021"));
    }

    #region SKILL
    //--SKILL
    private Dictionary<string, Skill> g_AllSkills = new Dictionary<string, Skill>();
    private List<Skill> g_SKills = new List<Skill>();


    private void AddAllSkill() {

        g_AllSkills.Add("S01", new Skill("S01", "강력한 베기", "전방의 적에게 {0}%의 데미지를 준다", 5, 2.5f,1f, true,0));
        g_AllSkills.Add("S02", new Skill("S02", "낙뢰", "전방의 적들에게 {0}%의 데미지를 준다", 15, 3.5f, 5f, false,1));
        g_AllSkills.Add("S03", new Skill("S03", "마그마강타", "전방의 적들에게 {0}%의 데미지를 준다", 25, 5.5f, 5f, false,2));
        g_AllSkills.Add("SDASH", new Skill("SDASH", "돌진", "전방을 향해 돌진한다", 0, 0f, 0.5f, true,3));

        g_SKills.Add(GetSkillinAllList("S01"));
        g_SKills.Add(GetSkillinAllList("S02"));
        g_SKills.Add(GetSkillinAllList("S03"));
        g_SKills.Add(GetSkillinAllList("SDASH"));
    }
    public Skill GetSkillinAllList(string _code) //미리 모든 스킬 정보 넣어놓고 내 스킬 리스트에 연결
    {
        if (!g_AllSkills.ContainsKey(_code))
            return null;

        return g_AllSkills[_code];
    }

    public void GainSkill(string _code) {
        if (!g_AllSkills.ContainsKey(_code))
            return;

        if(!g_SKills.Contains(g_AllSkills[_code]))
            g_SKills.Add(g_AllSkills[_code]);
    }
    
    public List<Skill> GetMySkillList()
    {
        return g_SKills;
    }
    #endregion


    #region QUEST
    //--QUEST
    private Dictionary<string, List<Quest>> AllQuestList = new Dictionary<string, List<Quest>>(); //모든 퀘스트

    private List<Quest> m_AcceptQuest = new List<Quest>(); //수락한 퀘스트
    private List<Quest> m_ClearQuest = new List<Quest>(); //클리어한 퀘스트
    private void AddAllQuest() {
        var _npc1 = new List<Quest>();
        _npc1.Add(new Quest(QuestType.Main, QuestContentType.KillBoss, 0, 1, "망령의 보스를 처치하시오", new QuestReward[] { new QuestReward(1013,1), new QuestReward(2012, 10) } ));
        _npc1.Add(new Quest(QuestType.Repeat, QuestContentType.KillMonster, 0, 2, "날뛰는 망령을 처치하시오", new QuestReward[] {  new QuestReward(2011, 10), new QuestReward(2021, 5) }));

        //var _npc2 = new List<Quest>();
        //_npc2.Add(new Quest(QuestType.Sub, QuestContentType.Item, 0, 10, "망령의 두건을 모아오시오", new List<QuestReward>() { }));

        AllQuestList.Add("N0", _npc1);
      //  AllQuestList.Add("NS0", _npc2);
    }

    public void AddAcceptQuest(Quest _quest) {
        if (!m_AcceptQuest.Contains(_quest))
            m_AcceptQuest.Add(_quest);
    }

    public void AddClearQuest(Quest _quest)
    {
        if (m_AcceptQuest.Contains(_quest))
            m_AcceptQuest.Remove(_quest);

        if (!m_ClearQuest.Contains(_quest))
            m_ClearQuest.Add(_quest);
    }

    public List<Quest> GetNpcQuestList(string _npc) {
        if (!AllQuestList.ContainsKey(_npc))
            return null;

        return AllQuestList[_npc];
    }

    public List<Quest> GetAcceptQuestList() {
        return m_AcceptQuest;
    }


    public void ReceiveQuestReward(QuestReward[] _list)
    {
        foreach (QuestReward _item in _list)
        {
            var item = GetItem(_item.m_RewardCode);
            item.m_Amount += _item.m_Amount;
            AddItem(item);        
        }
    }
    #endregion

    public Weapon g_EquipWeapon = null;

    public void EquipItem(int _code) {
        if (g_EquipWeapon != null)
            g_EquipWeapon.UnEquip();

        g_EquipWeapon =(Weapon)GetItem(_code);
        g_EquipWeapon.Equip();

        GameManager.GetInstance().UpdateInventory();
    }
}
