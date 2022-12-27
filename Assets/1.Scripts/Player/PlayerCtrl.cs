using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerCtrl : MonoBehaviour
{
    private const int SKILL_POWERSLASH = 0;
    private const int SKILL_THUNDER = 1;
    private const int SKILL_MAGMA = 2;
    private const int SKILL_DASH = 3;

    private static PlayerCtrl m_instance = null;

    public static PlayerCtrl GetInstance()
    {
        return m_instance;
    }

    private void Awake()
    {
        if (m_instance != null)
            Destroy(m_instance);

        m_instance = this;
    }
    [SerializeField]
    private GameObject m_ModelObj = null;

    public Transform m_WeaponTr = null;
    private Vector3 m_WeaponBasePosion = new Vector3(0, 0, 0.2f);
    private Vector3 m_WeaponBaseRotation = new Vector3(90f, 0, 0);



    private Rigidbody m_Rigid = null;

    [SerializeField, Header("이동 속도")]
    private float m_MoveSpeed = 2.0f; //이동속도

    [SerializeField, Header("회전 속도")]
    private float m_RotateSpeed = 90.0f; //이동속도

    [SerializeField, Header("이동 속도")]
    private SkinnedMeshRenderer m_MeshRender = null;

    private Animator m_Animator = null;

    private int m_CurExp = 0;
    private int m_MaxExp = 100;
    private int m_Level = 1;

    public void SetExp(int _exp)
    {
        m_CurExp += _exp;
        if (m_CurExp >= m_MaxExp)
        {
            m_Level += 1;
            m_CurExp = m_MaxExp - m_CurExp;
            m_MaxExp = m_Level * 100 + 50;
            MainUIManager.GetInstance().UpdateLevel(m_Level);
        }
        var rate = (float)m_CurExp / m_MaxExp;
        MainUIManager.GetInstance().UpdateExp(m_CurExp, m_MaxExp, rate);
    }
    //Move
    // Start is called before the first frame update
    void Start()
    {
        m_Rigid = this.GetComponent<Rigidbody>();
        m_Animator = this.GetComponent<Animator>();

        //시작시 무기 정보 불러와서 장착시키기

        SetStartWeapon();
        InitUI();

        GameManager.GetInstance().ac_EventOn += EventOn;
        GameManager.GetInstance().ac_EventOff += EventOff;
    }
    private bool m_isEvent = false;
    private void EventOn() {
        m_isEvent = true;
    }    
    private void EventOff()
    {
        m_isEvent = false;
    }
    private void InitUI() {

        if (MainUIManager.GetInstance() == null)
        {
            Invoke("InitUI", 1);
            return;
        }
        MainUIManager.GetInstance().UpdateHp(m_CurHp, 1);
        MainUIManager.GetInstance().UpdateMp(m_CurMp, 1);
        MainUIManager.GetInstance().UpdateGold();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_isEvent)
            return;

        Move();

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                SoundManager.GetInstance().PlayUISound("Click");
        }

    }

    float v, h;
    private void Move()
    {
        if (m_isAttackOn) //공격 모션중 x
            return;

        if (VirtualJoyStick.GetInstance() == null)
            return;

        h = VirtualJoyStick.GetInstance().GetHorizontalValue();
        v = VirtualJoyStick.GetInstance().GetVerticalValue();
        //v = Input.GetAxis("Vertical");
       // h = Input.GetAxis("Horizontal");


        if (v != 0 || h != 0)
        {
            m_Animator.SetBool("isMove", true);
            var vec = new Vector3(h, 0, v) * Time.deltaTime * m_MoveSpeed;

            m_ModelObj.transform.LookAt(this.transform.position + new Vector3(h, 0, v));
            this.transform.position += vec;

        }
        else
            m_Animator.SetBool("isMove", false);


        //float x = Input.GetAxis("Vertical");
        //float y = Input.GetAxis("Horizontal");
        //if (x != 0) {
        //    m_Animator.SetBool("isMove", true);
        //    this.transform.position += this.transform.forward * Time.deltaTime * m_MoveSpeed * x;
        //}else
        //    m_Animator.SetBool("isMove", false);
        //this.transform.Rotate(new Vector3(0, y* m_RotateSpeed, 0) * Time.deltaTime);
    }


    public void UsePosion(Posion _posion)
    {
        SoundManager.GetInstance().PlayEffect("Posion");
        if (_posion.isHp)
        {
            m_CurHp += (int)(m_MaxHp * _posion.RecoveryRate);
            if (m_CurHp > m_MaxHp)
                m_CurHp = m_MaxHp;
            MainUIManager.GetInstance().UpdateHp(m_CurHp, (float)m_CurHp / m_MaxHp);

        }
        else
        {
            m_CurMp += (int)(m_MaxMp * _posion.RecoveryRate);
            if (m_CurMp > m_MaxMp)
                m_CurMp = m_MaxMp;

            MainUIManager.GetInstance().UpdateMp(m_CurMp, (float)m_CurMp / m_MaxMp);
        }
    }





    #region CHANGE OUTSIDE
    private int m_skinIndex = 0;
    private int m_weaponIndex = 0;
    public void ChangeCharacterMaterial()
    {//Max 4

        m_MeshRender.material = Storage.GetInstance().m_Resources.GetMaterial(m_skinIndex);
        m_skinIndex++;
        if (m_skinIndex >= 4)
            m_skinIndex = 0;
    }

    public void ChangeWeapon()
    {//Max10 
        Debug.Log("Weapon change Index : " + m_weaponIndex);

        if (m_EquipWeapon != null)
            Destroy(m_EquipWeapon);

        m_EquipWeapon = null;

        var weapon = Storage.GetInstance().m_Resources.GetWeapon(m_weaponIndex);
        m_EquipWeapon = Instantiate(weapon);
        m_EquipWeapon.transform.SetParent(m_WeaponTr);
        m_EquipWeapon.transform.localPosition = m_WeaponBasePosion;
        m_EquipWeapon.transform.localRotation = Quaternion.Euler(m_WeaponBaseRotation.x, 0, 0);

        m_weaponIndex++;
        if (m_weaponIndex >= 6)
            m_weaponIndex = 0;
    }
    [SerializeField, Header("무기")]
    private GameObject m_EquipWeapon = null;
    private void SetStartWeapon()
    {
        var weapon = Storage.GetInstance().m_Resources.GetWeapon(m_weaponIndex);
        m_EquipWeapon = Instantiate(weapon);
        m_EquipWeapon.transform.SetParent(m_WeaponTr);
        m_EquipWeapon.transform.localPosition = m_WeaponBasePosion;
        m_EquipWeapon.transform.localRotation = Quaternion.Euler(m_WeaponBaseRotation.x, m_WeaponBaseRotation.y, m_WeaponBaseRotation.z);

    }


    #endregion

    #region ATTACK
    //이동과 공격 중 우선 순위 -? 
    private bool m_isAttackOn = false;
    public void OnAttack()
    {
        if (m_isAttackOn)
            return;

        m_isAttackOn = true;
        m_Animator.SetTrigger("isAttack");
        m_Animator.SetInteger("isAttackType", 0);
        SoundManager.GetInstance().PlayEffect("Slash");
    }
    public void DamageInAttackAction()
    {  //애니메이션에서 불러옴//데미지 이펙트 타이밍//앞에 존재하는 몬스터에게 공격 
       // Debug.Log("뿅");
        var _monsters = Physics.OverlapBox(this.transform.position + m_ModelObj.transform.forward, Vector3.one, Quaternion.identity, LayerMask.GetMask("Monster"));
        if (_monsters != null)
        {
            foreach (Collider _monster in _monsters)
            {
                _monster.GetComponent<MonsterBone>().TakeDam(Storage.GetInstance().GetDamage(), false);
            }
        }
    }
    public void EndOfAttackAnimation()
    {
        m_isAttackOn = false;
    }

    public int GetCurHp()
    {
        return m_CurHp;
    }
    public int GetMaxHp()
    {
        return m_MaxHp;
    }
    public int GetCurMp()
    {
        return m_CurMp;
    }
    public int GetMaxMp()
    {
        return m_MaxMp;
    }

    private int m_MaxHp = 150;
    private int m_CurHp = 150;

    private int m_CurMp = 50;
    private int m_MaxMp = 50;
    private float hpNormal = 0;
    public void TakeDamage(int _dam)
    {
        if (m_CurHp <= _dam)
        {

            m_CurHp = m_MaxHp;
            Debug.Log("Die");
            return;
        }


        m_CurHp -= _dam;

        hpNormal = (float)m_CurHp / m_MaxHp;
        Camera.main.GetComponent<FollowCamVer2>().ShakeCam(0.2f);
        MainUIManager.GetInstance().UpdateHp(m_CurHp, hpNormal);

       // Debug.Log("플레이어 공격 당함" + _dam);
    }


    public void OnSkill()
    {
        if (m_isAttackOn)
            return;

        m_isAttackOn = true;
        m_Animator.SetTrigger("isSkill");
    }
    private string m_ActiveSkillCode = "";
    private Skill m_Skill = null;

    public bool OnActiveSkill(string _skillcode)
    {
        if (m_isAttackOn)
            return false;

       // if (!SkillCo.ContainsKey(_skillcode))
          //  return false;

        var _mana = Storage.GetInstance().GetSkillinAllList(_skillcode).Mana;
        if (m_CurMp < _mana)
        {
            GameManager.GetInstance().SetMessage("마나가 부족합니다");
            return false;
        }

        m_CurMp -= _mana;
        if (m_CurMp <= 0)
            m_CurMp = 0;

        MainUIManager.GetInstance().UpdateMp(m_CurMp, (float)m_CurMp / m_MaxMp);

        //Debug.Log(_skillcode);

        m_isAttackOn = true;
        //if (m_ActiveSkillCode == "SDASH") {
        //    m_Animator.SetTrigger("isDash");
        //    m_ActiveSkillCode = _skillcode;
        //    return true;
        //}
        m_Animator.SetTrigger("isAttack");
        m_Animator.SetInteger("isAttackType", 1);
        m_ActiveSkillCode = _skillcode;



        return true;

    }


    //1, 플레이어 직접 사용
    //2 스킬 매니저 통해서 사용
    public GameObject m_skillObj = null;
    private GameObject m_instansObj = null;


    public void DamageInSkillMotion()
    {   //스킬 사용
       //m_Skill = Storage.GetInstance().GetSkillinAllList(m_ActiveSkillCode);
        //  StartCoroutine(SkillCo[m_ActiveSkillCode]);

        m_Skill = Storage.GetInstance().GetSkillinAllList(m_ActiveSkillCode);
        var _dam = m_Skill.DamageRate * Storage.GetInstance().GetDamage();
        SkillManager.GetInstance().OnActiveSkill(m_ActiveSkillCode, this.transform.position, this.transform.position + m_ModelObj.transform.forward, (int)_dam);
    }
    public void EndOfSkillMotion()
    {
        m_Animator.SetInteger("isAttackType", 0);
        m_isAttackOn = false;
        m_ActiveSkillCode = string.Empty;
        Destroy(m_instansObj, 1.5f);
    }
        #endregion
}
