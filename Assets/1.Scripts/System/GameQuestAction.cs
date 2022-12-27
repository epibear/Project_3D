using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public partial class GameManager : MonoBehaviour
{

    public UnityAction<int> ac_Quest_Kill_Monster = delegate { };
    public UnityAction<int> ac_Quest_Kill_Boss = delegate { };
    public UnityAction<int> ac_Quest_Item = delegate { };


    public void AddQuestAction(QuestContentType _type,UnityAction<int> _action) {

        switch (_type) {
            case QuestContentType.KillMonster:
                ac_Quest_Kill_Monster += _action;
                break;
            case QuestContentType.Item:
                ac_Quest_Item += _action;
                break;
            case QuestContentType.KillBoss:
                ac_Quest_Kill_Boss += _action;
                break;
        }
    }
    public void RemoveQuestAction(QuestContentType _type, UnityAction<int> _action) {
        switch (_type)
        {
            case QuestContentType.KillMonster:
                ac_Quest_Kill_Monster -= _action;
                break;
            case QuestContentType.Item:
                ac_Quest_Item -= _action;
                break;
            case QuestContentType.KillBoss:
                ac_Quest_Kill_Boss -= _action;
                break;
        }
    }

    public void UpdateKillMonster(int _monsterType) {
        ac_Quest_Kill_Monster.Invoke(_monsterType);
    }

    public void UpdateKillBoss(int _bossType)
    {
        ac_Quest_Kill_Boss.Invoke(_bossType);
    }

    public void UpdateItem(int _type)
    {
        ac_Quest_Item.Invoke(_type);
    }


    public UnityAction ac_UpdateStatus = delegate { };

    public void UpdateStatus() {
        ac_UpdateStatus.Invoke();
    }

    public UnityAction ac_UpdateInventory = delegate { };

    public void UpdateInventory()
    {
        ac_UpdateInventory.Invoke();
    }

    public UnityAction ac_UpdateGold = delegate { };
    public void UpdateGold()
    {
        ac_UpdateGold.Invoke();
    }


    public UnityAction ac_UpdatePosion = delegate { };
    public void UpdatePosion()
    {
        ac_UpdatePosion.Invoke();
    }


    public UnityAction ac_EventOn = delegate { };
    public UnityAction ac_EventOff = delegate { };
    public void StartEvent()
    {
        ac_EventOn.Invoke();
    }
    public void EndEvent() {
        ac_EventOff.Invoke();
    }

}
