using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill_MagmaSmash : ActiveSkill
{
    [SerializeField]
    private GameObject m_extraSkills = null;

    //시작위치 시전방향, 지속시간, 데미지, 시전자
    public override void OnSkill(Vector3 _pos, Vector3 _dir, int _dam)
    {
        this.transform.position = _pos;
        this.transform.LookAt(_dir);

        StartCoroutine(ActiveCo(_dam));
    }
    IEnumerator ActiveCo(int _dam)
    {
        m_extraSkills.gameObject.SetActive(true);        
        SoundManager.GetInstance().PlayEffect("Magma1");

        int _step = 3;
        var _pos = this.transform.position + this.transform.forward;
        while (_step > 0)
        {
            var _monsters = Physics.OverlapBox(_pos, Vector3.one * 1.5f, Quaternion.identity, LayerMask.GetMask("Monster"));
            if (_monsters != null)
            {
                foreach (Collider _monster in _monsters)             
                    _monster.GetComponent<MonsterBone>().TakeDam(_dam, false);                
            }
            _pos += m_extraSkills.transform.forward * 1.5f;
            _step--;
            yield return new WaitForSeconds(0.12f);
        }

        yield return new WaitForSeconds(1f);
        m_extraSkills.gameObject.SetActive(false);

    }
}