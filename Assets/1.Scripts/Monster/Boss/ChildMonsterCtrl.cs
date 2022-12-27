using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMonsterCtrl : MonsterBone
{
    [SerializeField]
    private BossCtrl bossCtrl = null;
    public override int GetExp()
    {
        return m_Exp;
    }

    public override State GetState()
    {
        return m_State;
    }

    public override void TakeDam(int _dam, bool _cri, int _cnt = 1)
    {
        bossCtrl.TakeDam(_dam, _cri);
    }

    protected override void Attack()
    {
        
    }
    private bool m_isEventOn = false;
    private void OnTriggerEnter(Collider other) //플레이어가 이벤트 콜리더와 부딪혔을떄
    {

        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        if (m_isEventOn)
            return;

        Debug.Log("BossRaid On" + other.name);

        m_isEventOn = true;
        
        bossCtrl.SetEventOn();
    }
}
