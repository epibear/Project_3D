using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill_Thunder : ActiveSkill
{
    [SerializeField]
    private GameObject[] m_extraSkills = null;
    private int m_Count = 0;

    public override void OnSkill(Vector3 _pos, Vector3 _dir, int _dam)
    {
        this.transform.position = _pos;
        this.transform.LookAt(_dir);
        m_Count = m_extraSkills.Length;
        StartCoroutine(ActiveCo(_dam));
    }
    IEnumerator ActiveCo(int _dam) //추가 생성
    {       
        //SkillEffect[SKILL_THUNDER].transform.LookAt(m_ModelObj.transform.forward);.
        var _monsters = Physics.OverlapBox(this.transform.position + this.transform.forward, Vector3.one * 2, Quaternion.identity, LayerMask.GetMask("Monster"));
        if (_monsters != null)
        {
            foreach (Collider _monster in _monsters)
            {
                m_extraSkills[m_Count - 1].SetActive(true);
                m_extraSkills[m_Count-1].transform.position = _monster.transform.position;
                _monster.GetComponent<MonsterBone>().TakeDam(_dam, false);
                SoundManager.GetInstance().PlayEffect("Thunder");
                yield return new WaitForSeconds(0.2f);
                m_Count--;
                if(m_Count < 0){
                    break;
                }
            }
        }
        yield return new WaitForSeconds(1f);

        foreach (GameObject _skill in m_extraSkills)
            _skill.SetActive(false);       

    }
}