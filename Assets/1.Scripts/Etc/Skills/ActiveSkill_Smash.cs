using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill_Smash : ActiveSkill
{
    [SerializeField]
    private GameObject m_extraSkills = null;
    public override void OnSkill(Vector3 _pos, Vector3 _dir, int _dam)
    {
        this.transform.position = _pos;
        this.transform.LookAt(_dir);

        StartCoroutine(ActiveCo(_dam));
    }
    IEnumerator ActiveCo(int _dam)
    {
        m_extraSkills.gameObject.SetActive(true);
        SoundManager.GetInstance().PlayEffect("Smash");
        var _monsters = Physics.OverlapBox(this.transform.position + this.transform.forward, Vector3.one, Quaternion.identity, LayerMask.GetMask("Monster"));
        if (_monsters != null)
        {
            foreach (Collider _monster in _monsters)
            {                   
                _monster.GetComponent<MonsterBone>().TakeDam(_dam, false);
            }
        }
        yield return new WaitForSeconds(1f);

        m_extraSkills.gameObject.SetActive(false); 

    }
}