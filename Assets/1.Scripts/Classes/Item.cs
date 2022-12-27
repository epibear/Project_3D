using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BaseItem {
    ItemType m_Type;

    int m_Amount;

    int m_ItemCode;

    public BaseItem(ItemType _type, int _amount, int _code)
    {
        this.m_Type = _type;
        this.m_Amount = _amount;
        this.m_ItemCode = _code;
    }
    public int Amount { get { return m_Amount; } }
    public int ItemCode { get { return m_ItemCode; } }
    public ItemType Type { get { return m_Type; } }
}
public enum ItemType { 
None,
Weapon,
Posion,
TotalCount
}
public abstract class Item 
{
    public ItemType m_Type = ItemType.None; //아이템 구분

    public int m_Amount = 0; //현재 수량

    public int m_ItemCode = 00; //고유 식별 코드

    public int m_Price = 10; //상점가

    public string m_Name;
    public string m_Content;
    //이미지 코드
}

public class Weapon : Item {
    private int m_Damage = 10; //데미지

    private bool m_isEquip = false;

    public Weapon(int _code,string _name ,string _content,int _dam, int _price, int _amount=0) {
        m_ItemCode = _code;
        m_Name = _name;
        m_Content = _content;
        m_Type = ItemType.Weapon;
        m_Damage = _dam;
        m_Price = _price;

        m_Amount = _amount;
    }
    
    public void Equip() {
        m_isEquip = true;
    }
    public void UnEquip()
    {
        m_isEquip = false;
    }

    public int Damage { get{ return m_Damage; }}
    public int Price { get { return m_Price; } }

    public bool isEquip { get { return m_isEquip; } }
}

public class LootItem : Item {
    //퀘스트 템

    public LootItem(int _code,  int _price, int _amount = 0)
    {
        m_ItemCode = _code;
        m_Type = ItemType.Weapon;
    
        m_Price = _price;

        m_Amount = _amount;
    }
}

public class Posion : Item
{
    private float m_RecoveryRate = 0.3f;

    private bool m_isHp = false;     
 
    public Posion(int _code,string _name, string _content, float _rate, bool _hp, int _price, int _amount = 0)
    {
        m_ItemCode = _code;
        m_Name = _name;
        m_Content = _content;
        m_Type = ItemType.Posion;


        m_RecoveryRate = _rate;
        m_isHp = _hp;
        m_Price = _price;

        m_Amount = _amount;
    }

    public float RecoveryRate { get { return m_RecoveryRate; } }
    public bool isHp { get { return m_isHp; } }
}
