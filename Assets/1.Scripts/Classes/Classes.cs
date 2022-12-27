using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    None,
    Ready,
    Idle,
    Chase,
    Return,
    Attack,
    Die,
    Pattern,
    Skill,
    Recovery,
    TotalCount
}

public class Skill {
    private string m_SkillCode;
    private string m_Name;
    private string m_Info;
    private int m_Mana;
    private float m_DamageRate;
    private float m_CoolTime;

    private bool m_isGain = false;
    private bool m_isEquip = false;
    private int m_SkillSlot;
    private int m_SpiteIndex = 0;

    public Skill(string _code, string _name, string _info, int _mana, float _damRate, float _cool, bool _isGain,int _sprite) {
        m_SkillCode = _code;
        m_Name = _name;
        m_Info = _info;
        m_Mana = _mana;
        m_CoolTime = _cool;
        m_DamageRate = _damRate;
        m_isGain = _isGain;
        m_SpiteIndex = _sprite;
    }

    public string SkillCode { get { return m_SkillCode; } }
    public string Name { get { return m_Name; } }
    public string Info { get { return m_Info; } }
    public float DamageRate { get { return m_DamageRate; } }
    public float CoolTime { get { return m_CoolTime; } }
    public bool isGain { get { return m_isGain; } }
    public int Mana { get { return m_Mana; } }
    public int SpiteIndex { get { return m_SpiteIndex; } }
    public bool isEquip { get { return m_isEquip; } }
    public int SkillSlot { set { m_SkillSlot = value; } get { return m_SkillSlot; } }

}



public enum QuestType
{
    None,
    Main,
    Sub,
    Repeat,
    TotalCount
}

public enum QuestContentType
{
    None,
    KillMonster,
    KillBoss,
    Item,
    TotalCount
}

public struct QuestReward
{
    public int m_RewardCode;
    public int m_Amount;
    public QuestReward(int _Code, int _amout)
    {
        m_RewardCode = _Code;
        m_Amount = _amout;
    }
}

#region QUEST
public class Quest
{
    private QuestType m_QType = QuestType.None;    //퀘스트 타입
    private QuestContentType m_ContentType = QuestContentType.None; //퀘스트 내용
    private int m_TargetType = 0; //Monster라면 몬스터 코드 .아이템이면 아이템 코드
    private int m_Count = 0; //현재 진행도
    private int m_TargetCount = 10; //목표

    private bool m_isClear = false;   //완료여부 
    private bool m_isAccept = false; //수락 여부
    private string m_Contents = ""; //퀘스트 내용 문자열

    private bool m_isReceiveReward = false;
    //List<QuestReward> m_RewardItems ; //보상

    QuestReward[] m_RewardItems; //보상
    public Quest(QuestType _qtype, QuestContentType _questContent, int _targetType, int _targetcount, string _content, QuestReward[] _list)
    {
        m_QType = _qtype;
        m_ContentType = _questContent;
        m_TargetType = _targetType;
        m_TargetCount = _targetcount;
        m_Contents = _content;
        m_RewardItems = _list;
    }
    public void Accept()
    {
        GameManager.GetInstance().AddQuestAction(m_ContentType, CountUp);
        Storage.GetInstance().AddAcceptQuest(this);
        m_isAccept = true;
    }

    protected void Clear()
    {
        m_isClear = true;
        //ReceiveReward();
        GameManager.GetInstance().RemoveQuestAction(m_ContentType, CountUp);

    }

    private void CountUp(int _targetType)
    {
        if (m_TargetType != _targetType)
            return;

        if (m_isClear)
            return;

       // Debug.Log("퀘스트 진행도 +1");

        m_Count++;

        if (m_Count >= m_TargetCount)
            Clear();
    }
    public void ReceiveReward()
    {
        m_isReceiveReward = true;
        Storage.GetInstance().ReceiveQuestReward(m_RewardItems);
        Storage.GetInstance().AddClearQuest(this);
        GameManager.GetInstance().UpdateInventory();
    }

    public string Contents { get { return m_Contents; } }
    public int Count { get { return m_Count; } }
    public int TargetCount { get { return m_TargetCount; } }
    public float Rate { get { return (float)m_Count / m_TargetCount; } } //진행도

    public bool isAccept { get { return m_isAccept; } }
    public bool isClear { get { return m_isClear; } }
    public bool isReceiveReward { get { return m_isReceiveReward; } }
    public QuestType QType { get { return m_QType; } }
    public QuestContentType ContentType { get { return m_ContentType; } }
}

#endregion





