using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance = null;
    public static SkillManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
        DontDestroyOnLoad(this);
    }


    private Dictionary<string, ActiveSkill> Skill = new Dictionary<string, ActiveSkill>();

    [SerializeField]
    private ActiveSkill[] m_Skill = null;


    //스킬 오브젝트 들고 있다가 
    //스킬 사용시 구분 어덯게 햇드라
    // Start is called before the first frame update
    void Start()
    {
        Skill.Add("S01", m_Skill[0]);
        Skill.Add("S02", m_Skill[1]);
        Skill.Add("S03", m_Skill[2]);
        Skill.Add("SDASH", m_Skill[3]);
    }

    public bool OnActiveSkill(string _Type, Vector3 _pos, Vector3 _dir, int _dam) {

        if (!Skill.ContainsKey(_Type))
            return false;

        Skill[_Type].OnSkill(_pos, _dir, _dam);
        return true;
    }
}
