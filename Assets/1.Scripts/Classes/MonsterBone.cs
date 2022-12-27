using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public abstract class MonsterBone : MonoBehaviour
{
    protected int m_Exp = 5;
    protected int m_CurHp = 50;
    protected int m_MaxHp = 50;
    [SerializeField]
    protected State m_State = State.Idle;

    public abstract int GetExp();

    public abstract State GetState();
    protected abstract void Attack();

    public abstract void TakeDam(int _dam, bool _cri, int _cnt = 1);

}
