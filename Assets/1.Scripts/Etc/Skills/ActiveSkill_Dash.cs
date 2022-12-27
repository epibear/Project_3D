using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill_Dash : ActiveSkill
{
    [SerializeField]
    private GameObject m_extraSkills = null;
    // Start is called before the first frame update
    void Start()
    {
        
    } 


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
        SoundManager.GetInstance().PlayEffect("Dash");
        Ray ray = new Ray(PlayerCtrl.GetInstance().transform.position, this.transform.forward * 4f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {            
            var dist = (PlayerCtrl.GetInstance().transform.transform.position - hit.transform.position).magnitude;
            if (dist > 0.0f && dist < 3.5f)
                PlayerCtrl.GetInstance().transform.transform.position += this.transform.forward * dist;
            else
                PlayerCtrl.GetInstance().transform.transform.position += this.transform.forward * 3.5f;
        }
        else
            PlayerCtrl.GetInstance().transform.transform.position += this.transform.forward * 3.5f;

        yield return new WaitForSeconds(0.3f);
        m_extraSkills.gameObject.SetActive(false);
        yield return null;

    }
}
