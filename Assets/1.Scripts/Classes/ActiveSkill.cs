using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ActiveSkill :MonoBehaviour
{  
    //시작위치 시전방향, 지속시간, 데미지, 시전자
    public abstract void OnSkill(Vector3 _pos, Vector3 _dir, int _dam);     
    

}
