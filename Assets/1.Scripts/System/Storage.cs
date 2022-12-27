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
    //��� ������- > �ڵ� �޾Ƽ� �� ���������� amount ����
    private Dictionary<int, Item> g_Items = new Dictionary<int, Item>();
    private List<Item> g_InvenItem = new List<Item>();
    private void AddAllItem() {
        g_Items.Add(1010, new Weapon(1010,"ª�� ��","����� ����ϱ⿣ ���� ª�� ��",5,100));
        g_Items.Add(1011, new Weapon(1011, "��ī�ο� ���", "�ſ� ��ī�ο� ���", 15, 500));
        g_Items.Add(1012, new Weapon(1012, "�ù���", "���⿡ ������ �縷������ ��", 35, 1500));
        g_Items.Add(1013, new Weapon(1013, "������ ��", "������ ������ ��", 100, 5000));

        g_Items.Add(2010, new Posion(2010, "�ʱ� ü�� ����", "���� ü���� 30%�� ȸ���Ѵ�", 0.3f, true, 50));
        g_Items.Add(2011, new Posion(2011, "�߱� ü�� ����", "���� ü���� 50%�� ȸ���Ѵ�", 0.5f, true, 150));
        g_Items.Add(2012, new Posion(2012, "��� ü�� ����", "���� ü���� 100%�� ȸ���Ѵ�", 1f, true, 350));

        g_Items.Add(2020, new Posion(2020, "�ʱ� ���� ����", "���� ������ 30%�� ȸ���Ѵ�", 0.3f, false, 100));
        g_Items.Add(2021, new Posion(2021, "�߱� ���� ����", "���� ������ 50%�� ȸ���Ѵ�", 0.5f, false, 250));

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

        g_AllSkills.Add("S01", new Skill("S01", "������ ����", "������ ������ {0}%�� �������� �ش�", 5, 2.5f,1f, true,0));
        g_AllSkills.Add("S02", new Skill("S02", "����", "������ ���鿡�� {0}%�� �������� �ش�", 15, 3.5f, 5f, false,1));
        g_AllSkills.Add("S03", new Skill("S03", "���׸���Ÿ", "������ ���鿡�� {0}%�� �������� �ش�", 25, 5.5f, 5f, false,2));
        g_AllSkills.Add("SDASH", new Skill("SDASH", "����", "������ ���� �����Ѵ�", 0, 0f, 0.5f, true,3));

        g_SKills.Add(GetSkillinAllList("S01"));
        g_SKills.Add(GetSkillinAllList("S02"));
        g_SKills.Add(GetSkillinAllList("S03"));
        g_SKills.Add(GetSkillinAllList("SDASH"));
    }
    public Skill GetSkillinAllList(string _code) //�̸� ��� ��ų ���� �־���� �� ��ų ����Ʈ�� ����
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
    private Dictionary<string, List<Quest>> AllQuestList = new Dictionary<string, List<Quest>>(); //��� ����Ʈ

    private List<Quest> m_AcceptQuest = new List<Quest>(); //������ ����Ʈ
    private List<Quest> m_ClearQuest = new List<Quest>(); //Ŭ������ ����Ʈ
    private void AddAllQuest() {
        var _npc1 = new List<Quest>();
        _npc1.Add(new Quest(QuestType.Main, QuestContentType.KillBoss, 0, 1, "������ ������ óġ�Ͻÿ�", new QuestReward[] { new QuestReward(1013,1), new QuestReward(2012, 10) } ));
        _npc1.Add(new Quest(QuestType.Repeat, QuestContentType.KillMonster, 0, 2, "���ٴ� ������ óġ�Ͻÿ�", new QuestReward[] {  new QuestReward(2011, 10), new QuestReward(2021, 5) }));

        //var _npc2 = new List<Quest>();
        //_npc2.Add(new Quest(QuestType.Sub, QuestContentType.Item, 0, 10, "������ �ΰ��� ��ƿ��ÿ�", new List<QuestReward>() { }));

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
