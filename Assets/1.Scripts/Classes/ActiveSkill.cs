using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ActiveSkill :MonoBehaviour
{  
    //������ġ ��������, ���ӽð�, ������, ������
    public abstract void OnSkill(Vector3 _pos, Vector3 _dir, int _dam);     
    

}
