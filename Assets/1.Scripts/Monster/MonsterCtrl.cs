using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MonsterCtrl : MonsterBone
{
    private Transform m_Target = null;

    #region Setting
    [SerializeField, Header("MonsterType")]
    private int m_MonsterType;


    [SerializeField, Header("MonsterName")]
    private string m_MonsterName;

    [SerializeField, Header("AttackSpeed")]
    private float m_AttackSpeed = 2.0f;
    [SerializeField,Header("Damage")]
    private int m_baseAttackDam = 10;
      
    private float m_MoveSpeed = 2.5f;
    private float m_AttackRange = 2f;
    private float m_TraceRange = 20.0f;

    #endregion
    private float m_addTimer = 1.0f;
    private float m_distance = 0.0f;
    private double hpNormal;

    private Vector3 m_DirVec = Vector3.zero;
    private Vector3 m_ReturnPosition = Vector3.zero; 

    private Animator m_Animator = null;
    private SphereCollider m_Collider = null;

    #region UI

    [SerializeField, Header("UI")]
    private GameObject m_UIObj = null;
    public Text m_txt_Name = null;
    public Image m_DamHpBar;
    public Image m_CurHpBar;
    private bool m_isTakeDam = false;
    #endregion



    public List<Material> m_MtrlList;
    private Material m_HitMaterial = null;
    private SkinnedMeshRenderer m_Render = null;

    public override int GetExp()
    {
        return m_Exp;
    }
    public override State GetState()
    {
        return m_State;
    }

    private void Awake()
    {

    }
    //    Start is called before the first frame update
    void Start()
    {

        m_txt_Name.text = m_MonsterName;
        m_UIObj.SetActive(false);

        m_ReturnPosition = this.transform.position;
        m_Animator = this.GetComponent<Animator>();
        m_Collider = this.transform.GetComponent<SphereCollider>();
        StartCoroutine(StateCo());


        m_MtrlList = new List<Material>();
        m_Render = this.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        m_Exp = 20;
        m_Render.GetMaterials(m_MtrlList);
        m_HitMaterial = m_MtrlList[1];
        m_MtrlList.Remove(m_HitMaterial); //처음 제거
        m_Render.materials = m_MtrlList.ToArray();
    }

    //   Update is called once per frame
    void FixedUpdate()
    {
        if (m_isTakeDam)
            m_DamHpBar.fillAmount = Mathf.Lerp(m_DamHpBar.fillAmount, m_CurHpBar.fillAmount, Time.deltaTime * 2.0f);

    }


    protected override void Attack()
    {
        m_DirVec = m_Target.position - this.transform.position;

        m_distance = m_DirVec.magnitude;

        this.transform.LookAt(m_Target);       

        if (m_AttackRange < m_distance)//공격 범위 안에 있는지 계속 체크
        {
            ChangeState(State.Chase);
            return;
        }



        m_addTimer -= Time.deltaTime; //공격 딜레이

        if (m_addTimer <= 0.0f)
        {
            m_Animator.SetTrigger("isAttack");
            PlayerCtrl.GetInstance().TakeDamage(m_baseAttackDam);
            m_addTimer = m_AttackSpeed;
        }

    }

    public override void TakeDam(int _dam, bool _cri, int _cnt = 1)
    {
        if (m_isReady)
            return;

        if (!m_UIObj.activeSelf)
            m_UIObj.SetActive(true);


       //Debug.Log("뎀지 받음");
        if (m_State == State.Die)
            return;

        if (DamageTextsCtrl.GetInstance() == null)
            return;

        /// if (HitEffectsCtrl.GetInstance() == null)
        //    return;
        //  HitEffectsCtrl.GetInstance().OnHit(this.transform.position);


        m_isTakeDam = true;

        var vec = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 1.0f, 0));
        DamageTextsCtrl.GetInstance().OnDamageText(
           _dam.ToString(), vec, _cri);

        m_CurHp -= _dam;
        hpNormal = (double)m_CurHp / (double)m_MaxHp;
        m_CurHpBar.fillAmount = Mathf.Clamp((float)hpNormal, 0f, 1f);


        if (m_CurHp <= 0)
            ChangeState(State.Die);
        else {

            if ((m_State == State.Idle || m_State == State.Return) && (m_State != State.Attack || m_State != State.Chase))
            {
                m_Target = PlayerCtrl.GetInstance().transform;
                ChangeState(State.Chase);
            }

            m_MtrlList.Add(m_HitMaterial);
            m_Render.materials = m_MtrlList.ToArray();
            m_Animator.SetTrigger("isHit");
            Invoke("EndOfHitMotion", 0.5f);
        }            
    }

    private void EndOfHitMotion() {
        m_MtrlList.Remove(m_HitMaterial);
        m_Render.materials = m_MtrlList.ToArray();
    }



    public void Die()
    {     
        m_CurHp = 0;
        m_Animator.SetTrigger("isDie");    //Die Animation
        this.GetComponent<SphereCollider>().enabled = false;
        PlayerCtrl.GetInstance().SetExp(m_Exp);
        GameManager.GetInstance().UpdateKillMonster(m_MonsterType);
        InGameManager.GetInstance().DropItem(this.transform.position);
        m_UIObj.SetActive(false);
    }
    public void EndOfDieMotion()
    {
        this.gameObject.SetActive(false);

        //  Storage.GetInstance().SetMission(MissionType.Kill_Normal, 1);
        //  DropItemsCtrl.GetInstance().RandomItemDrop(this.transform.position);
        //MapManager.GetInstance().KillMonster(m_Exp);
        //   PlayerCtrl.GetInstance().GainGage();
    }


    private void Idle()
    {
        //m_addTimer -= Time.deltaTime;
        //if (m_addTimer <= 0.0f)
        //{
        //    m_DirVec = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        //    m_DirVec = m_DirVec.normalized;

        //    this.transform.LookAt(m_DirVec);

        //    ChangeState(State.Move);
        //}
    }


    private void Return() //복귀 
    {
        m_DirVec = m_ReturnPosition - this.transform.position;

        this.transform.LookAt(m_ReturnPosition);

        this.transform.position += m_DirVec.normalized * Time.deltaTime * m_MoveSpeed;

        m_distance = m_DirVec.magnitude;

        if (m_distance <= 1.5f)
        {
            ChangeState(State.Idle);
        }
    }
    private void Chase() //공격 받을 시 일정 거리까지 추격 // 거리가 멀어지면 복귀
    {
        //m_Target의 위치 추적
        m_DirVec = m_Target.position - this.transform.position;

        this.transform.LookAt(m_Target);

        m_distance = m_DirVec.magnitude;
        //Debug.LogFormat("추격 거리 : {0}/{1}", m_distance, m_TraceRange);
        if (m_TraceRange < m_distance) //추격 포기 복귀
        {
            ChangeState(State.Return);
            return;
        }

        this.transform.position += m_DirVec.normalized * Time.deltaTime * m_MoveSpeed;

        if (m_AttackRange >= m_distance) //공격 범위 안에 존재- 공격
        {
            ChangeState(State.Attack);
        }
    }


    private void ChangeState(State _state) //몬스터 상태 변화
    {
        if (_state == m_State)
            return;
        m_MainAction = delegate { };

        m_State = _state;
        //Debug.Log("상태 변화 " + m_State.ToString());

        switch (_state)
        {
            case State.Ready:
                Ready();
                break;
            case State.Chase:
                m_Animator.SetTrigger("isMove");
                m_MainAction += Chase;
                break;
            case State.Attack:
                m_addTimer = 0.2f; //도착하자마자 공격 시도
                m_MainAction += Attack;
                break;
            case State.Return:
                m_Animator.SetTrigger("isMove");
                m_MainAction += Return;
                break;
            case State.Idle:
                m_Animator.SetTrigger("isIdle");
                m_Collider.isTrigger = true; //트리거 키기
                m_Collider.radius = 4f;
                m_addTimer = 1.0f;
                m_MainAction += Idle;
                break;
            case State.Die:
                Die();
                break;
        }


    }

    private bool m_isReady = true;
    private void Ready()
    { //일어나는 모션   
        m_Animator.SetBool("isReady", true);
    }
    //
    public void EndOfReadyMotion()
    {    // Chase
        m_Animator.SetBool("isReady", false);
        m_isReady = false;
        ChangeState(State.Chase);
    }


    private bool m_isAwake = false; //한번 깨어날때만
    private void OnTriggerEnter(Collider other)
    {
        if (m_State == State.Idle)
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {           //플레이어만 체크                   
                m_Target = other.GetComponent<PlayerCtrl>().transform;

                m_Collider.isTrigger = false; //트리거 제거
                m_Collider.radius = 0.5f;
                if(!m_isAwake)
                    ChangeState(State.Ready); //일어나는 모션
                else
                    ChangeState(State.Chase); //일어나는 모션

                m_isAwake = true;
            }

    }

    private UnityAction m_MainAction = delegate { };
    private WaitForFixedUpdate EndUpdate = new WaitForFixedUpdate();
    IEnumerator StateCo()
    {
        while (true)
        {
            if (!this.gameObject.activeSelf)
                break;
            m_MainAction.Invoke();

            yield return EndUpdate;
        }

        Debug.Log("끝");
        yield return null;

    }


}
