using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class BossCtrl : MonsterBone
{
    //skills
    [SerializeField]
    private GameObject[] Meteos;
    [SerializeField]
    private GameObject RecoverySkill;
    [SerializeField]
    private GameObject SpecialSkill;


    [SerializeField, Header("Name")]
    private string m_BossName;
    [SerializeField, Header("Type"), Tooltip("0~10")]
    private int m_BossType;



    private Transform m_Target = null;

    #region Setting  
    private int m_baseAttackDam = 10;
    private float m_MoveSpeed = 2.5f;
    private float m_AttackRange = 6f;
    #endregion


    private float m_addTimer = 1.0f;
    private float m_distance = 0.0f;
    private double hpNormal;

    private Vector3 m_DirVec = Vector3.zero;
    private Animator m_Animator = null;
    #region UI

    [SerializeField, Header("UI")]
    private GameObject m_UIObj = null; //���ο� ĵ���� �����ؾ���
    public Text m_txt_Name = null;
    public Image m_DamHpBar;
    public Image m_CurHpBar;
    public Text m_txt_Hp = null;
    public Text m_txt_HpRate = null;
    private bool m_isTakeDam = false;
    #endregion


    private bool m_isChangePhase = true; // ������ü���� �߿� ����
    //��ų ����Ʈ - �극��, �⺻ ���� , Ŭ�� ���� 

    //--EVENT


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

        m_txt_Name.text = m_BossName;
        m_UIObj.SetActive(false);
        m_Animator = this.GetComponent<Animator>();

        StartCoroutine(StateCo());//������Ʈ ���
        m_CurHp = 500;
        m_MaxHp = 500;
        UpdateHp();
    }

    //   Update is called once per frame
    void FixedUpdate() //hpbar�� �׽� ����
    {
        if (m_isTakeDam)
            m_DamHpBar.fillAmount = Mathf.Lerp(m_DamHpBar.fillAmount, m_CurHpBar.fillAmount, Time.deltaTime * 2.0f);

        //if (m_isRotate) {
        //    if (m_Angle > 0f)
        //    {
        //        m_RotateAngle = Mathf.Lerp(0, m_Angle, Time.deltaTime);
        //        if (m_isReverse)
        //            this.transform.RotateAround(this.transform.position, -Vector3.up, m_RotateAngle);
        //        else
        //            this.transform.RotateAround(this.transform.position, Vector3.up, m_RotateAngle);                      
        //    }
        //}
    }
    private float m_RotateAngle = 0;
    private bool m_isReverse = false;
    private bool m_isRotate = false;
    private float m_Angle = 0;

    protected override void Attack() //Random cpzm
    {
        //m_DirVec = m_Target.position - this.transform.position;

        //m_distance = m_DirVec.magnitude;

        ////this.transform.LookAt(m_Target, Vector3.zero);

        //if (m_AttackRange < m_distance)//���� ���� �ȿ� �ִ��� ��� üũ
        //{
        //    ChangeState(State.Chase);
        //    return;
        //}


        m_Animator.SetInteger("Attack",1);
        m_Animator.SetTrigger("isAttack");
        PlayerCtrl.GetInstance().TakeDamage(m_baseAttackDam);

    }

    private void UpdateHp() {

        hpNormal = (double)m_CurHp / (double)m_MaxHp;
        m_CurHpBar.fillAmount = Mathf.Clamp((float)hpNormal, 0f, 1f);
        m_txt_HpRate.text = string.Format("{0}%", (hpNormal*100).ToString("N0"));
        m_txt_Hp.text = string.Format("{0}/{1}",m_CurHp,m_MaxHp);
    }
    private int m_CurPattern = 1;
    private int m_LimitHp = 0;
    public override void TakeDam(int _dam, bool _cri, int _cnt = 1)
    {
        if (m_isChangePhase)
            return;

        if (!m_UIObj.activeSelf)
            m_UIObj.SetActive(true);


       // Debug.Log("���� ����");

        if (m_State == State.Die)
            return;

        if (DamageTextsCtrl.GetInstance() == null)
            return;

        /// if (HitEffectsCtrl.GetInstance() == null)
        //    return;
        //  HitEffectsCtrl.GetInstance().OnHit(this.transform.position);


        m_isTakeDam = true;

        if(m_Target==null)
            m_Target = PlayerCtrl.GetInstance().transform;




        var vec = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 1.0f, 0));
        DamageTextsCtrl.GetInstance().OnDamageText(
           _dam.ToString(), vec, _cri);

        m_CurHp -= _dam;
        if (m_CurHp < m_LimitHp)
            m_CurHp = m_LimitHp;
        //���� �������� ����Ʈ �Ѿ�� ����
        UpdateHp();


        if (m_CurPattern != 3 && m_CurHp <= m_LimitHp)
        {
            ChangePattern(++m_CurPattern);
        }
        else if (m_CurHp <= 0)
        {

            ChangeState(State.Die);
        }
        //else
          //  m_Animator.SetTrigger("isHit");
    }


    public void Die()
    {
        m_CurHp = 0;
        m_Animator.SetTrigger("isDie");    //Die Animation       
        GameManager.GetInstance().UpdateKillBoss(m_BossType);
        Debug.Log("���� ų");
        // InGameManager.GetInstance().DropItem(this.transform.position);
        //  m_UIObj.SetActive(false);
    }
    //  private void EndOfDieMotion()
    //   {
    //this.gameObject.SetActive(false);

    //  Storage.GetInstance().SetMission(MissionType.Kill_Normal, 1);
    //  DropItemsCtrl.GetInstance().RandomItemDrop(this.transform.position);
    //MapManager.GetInstance().KillMonster(m_Exp);
    //   PlayerCtrl.GetInstance().GainGage();
    //  }


    //Phase 1. �⺻ ���� 2. ���߿��� ��ų 1������  3. ����x ,ȸ��
    //���������� ���� ���� Queue
    private Queue<UnityAction> PhasePattern = new Queue<UnityAction>();
    private void ChangePattern(int _phase)   //���� ����
    {    
        ChangeState(State.None);
        PhasePattern.Clear();
        switch (_phase){
            case 1:
                m_Target = PlayerCtrl.GetInstance().transform;
                m_LimitHp = (int)(m_MaxHp * 0.7f);
                PhasePattern.Enqueue(PatternIdle);
                PhasePattern.Enqueue(PatternAttack);
                NextPattern();
                break;
            case 2:
                m_LimitHp = (int)(m_MaxHp * 0.3f);
                PatternFly();
                PhasePattern.Enqueue(PatternIdle);
                PhasePattern.Enqueue(PatternAttack);
                PhasePattern.Enqueue(PatternIdle);
                PhasePattern.Enqueue(PatternAttack);
                PhasePattern.Enqueue(PatternIdle);
                PhasePattern.Enqueue(PatternRecovery);
                PhasePattern.Enqueue(PatternIdle);
                break;
            case 3:
                m_LimitHp = 0;
                PatternRecovery();
                PhasePattern.Enqueue(PatternAttack);
                PhasePattern.Enqueue(PatternIdle);
                PhasePattern.Enqueue(PatternMeteo);
                PhasePattern.Enqueue(PatternIdle);
                PhasePattern.Enqueue(PatternAttack);
                PhasePattern.Enqueue(PatternIdle);
                PhasePattern.Enqueue(PatternWildSkill);                
                break;
        }     
    }

    private void NextPattern() {//���� ��������      
        var _action = PhasePattern.Dequeue();
        _action.Invoke(); //���� ����
        PhasePattern.Enqueue(_action);
    }
    private void Idle()
    {
        FindTarget();
        m_addTimer -= Time.deltaTime;
        if (m_addTimer <= 0)
        {
            m_addTimer = 5.0f;
            ChangeState(State.None); //������ ȣ��ɱ��
            NextPattern();            
        }
    }

    private void Chase() //���� ���� �� ���� �Ÿ����� �߰� // �Ÿ��� �־����� ����
    {
        //m_Target�� ��ġ ����
        FindTarget();
        // this.transform.LookAt(m_Target);

        m_distance = m_DirVec.magnitude;
    
        this.transform.position += m_DirVec.normalized * Time.deltaTime * m_MoveSpeed;

        if (m_AttackRange >= m_distance) //���� ���� �ȿ� ����- ����
        {
            m_Animator.SetBool("isMove", false);
            ChangeState(State.Attack);
        }
    }


    private void ChangeState(State _state) //���� ���� ��ȭ
    {
        if (_state == m_State)
            return;

        m_MainAction = delegate { };

        m_State = _state;

        if (m_Animator.GetBool("isMove"))
            m_Animator.SetBool("isMove", false);

        switch (_state)
        {
            case State.None: //���� ����                       
                break;
            case State.Idle:
                m_MainAction = Idle;
                break;
            case State.Chase:
                m_Animator.SetBool("isMove",true);
                m_MainAction = Chase;
                break;
            case State.Attack:
                Attack();
                break;               
            case State.Die:
                Die();
                break;
        }
    }

 
    public void EndOfReadyMotion()
    {    // Chase
        m_Animator.SetBool("isReady", false);
        ChangeState(State.Idle);
    }


    private UnityAction m_MainAction = delegate { };
    private WaitForFixedUpdate EndUpdate = new WaitForFixedUpdate();
    IEnumerator StateCo()
    {
        while (true)
        {
            if (m_State == State.Die)//�ױ������� ��� ����
                break;

            m_MainAction.Invoke(); 

            yield return EndUpdate;
        }
        yield return null;
    }
    //--
    #region EVENT ACTION
    [SerializeField, Header("EventTriggerObject")]
    private GameObject m_EventTriggerObj = null;
    [SerializeField, Header("EventCameraObject")]
    private GameObject m_EventCameraObj = null;

    public void SetEventOn() {
        m_EventTriggerObj.SetActive(false);
        StartCoroutine(EventCo());
    }
    private bool m_isEventMoveEnd = false;
    IEnumerator EventCo()
    {
        GameManager.GetInstance().StartEvent();
        ChangeState(State.None);
        MainUIManager.GetInstance().OffMainCanvas();
        m_EventCameraObj.SetActive(true); //�̺�Ʈ ķ
        m_Animator.SetTrigger("isEventMove"); //�ȴ� ���
        //Ư�� �Ÿ� �̵���
        while (!m_isEventMoveEnd)
        {
            this.transform.position += new Vector3(0, 0, -1) * Time.deltaTime * m_MoveSpeed;
            if (this.transform.position.z <= 235)
                m_isEventMoveEnd = true;

            yield return EndUpdate;
        }

        PlayerCtrl.GetInstance().transform.position = new Vector3(15,0,210); //�÷��̾� ��ġ ���� �̵�
        m_EventCameraObj.SetActive(false);
        MainUIManager.GetInstance().OnMainCanvas();
        m_Animator.SetTrigger("isEventScream");
        yield return null;
    }
    private void EndOfScreamMotion()
    { //���� �� 1���� ����       
        Invoke("OnBossUI", 1f);
    }
    private void OnBossUI()
    {
        GameManager.GetInstance().EndEvent();
        ChangeState(State.None);     
        m_UIObj.SetActive(true);
        m_isChangePhase = false;
        ChangePattern(1);
    }
    #endregion


    #region PATTERN
    //

    private void PatternRecovery()
    {
        StartCoroutine(PatternRecoveryCo());
    }
    IEnumerator PatternRecoveryCo()
    {
        ChangeState(State.None);
        yield return new WaitForSeconds(0.1f);
        m_Animator.SetTrigger("isDeffence");
        RecoverySkill.transform.position = this.transform.position;
        RecoverySkill.SetActive(true);
        //�� ȸ�� �ֱ������� +20%
        //�ʴ� 4%�� 5�ʰ�
 
        var count = 20; //10%
        while (count > 0)
        {
            m_CurHp += (int)(m_MaxHp * 0.01f);
            UpdateHp();
            yield return new WaitForSeconds(0.25f);
            count--;
        }
        RecoverySkill.SetActive(false);
        NextPattern();
    }

    private void PatternMeteo()
    {
        StartCoroutine(PatternMeteoCo());
    }
    IEnumerator PatternMeteoCo()
    {
        ChangeState(State.None);
        yield return new WaitForSeconds(0.1f);
        m_Animator.SetInteger("Attack", 3);
        m_Animator.SetTrigger("isAttack");
        var _basePos = this.transform.position + this.transform.forward * 8f;

        yield return new WaitForSeconds(1.5f); //���

        foreach (GameObject _skill in Meteos) {
            var _randPos = _basePos + new Vector3(Random.Range(-3.0f, 3.0f)*3f, 0, Random.Range(-3.0f, 3.0f)* 3f);
            _skill.transform.position = _randPos;
            _skill.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            var colliders = Physics.OverlapSphere(_skill.transform.position, 3f, LayerMask.GetMask("Player"));
            if (colliders.Length > 0)            
                PlayerCtrl.GetInstance().TakeDamage(10);            

        }  
        yield return new WaitForSeconds(0.5f);      
        foreach (GameObject _skill in Meteos)
        {
            _skill.SetActive(false);
        }
        
        NextPattern();
    }
    private void PatternWildSkill()
    {
        StartCoroutine(PatternWildSkillCo());       
    }
    private void FindTarget() {
        m_DirVec = m_Target.position - this.transform.position;
        this.transform.LookAt(m_Target);
        //m_Angle = Vector3.Angle(transform.forward, m_DirVec); //
        //var dir = Vector3.Cross(transform.forward, m_Target.position);
        //m_isReverse = dir.y >= 0 ? false : true;
        //if (m_Angle <= 0.5f) {
        //    m_Angle = 0;
        //    m_isRotate = false;
        //}
        //if (m_Angle > 0)
        //    m_isRotate = true;
    }

    IEnumerator PatternWildSkillCo() {
        //������ x 
        //��ų��o
        ChangeState(State.None);
        yield return new WaitForSeconds(0.1f);
        m_Animator.SetInteger("Attack", 3);
        m_Animator.SetTrigger("isAttack");
        SpecialSkill.SetActive(true);
        SpecialSkill.transform.position = PlayerCtrl.GetInstance().transform.position;
        Debug.LogFormat("Pattern3 {0}", "WildSkill");

        yield return new WaitForSeconds(3.0f);

        m_DirVec = m_Target.position - this.transform.position;
        m_Angle = Vector3.Angle(transform.forward, m_DirVec); //
        var dir = Vector3.Cross(transform.forward, m_Target.position);
        m_isReverse = dir.y >= 0 ? false : true;
        if (m_Angle > 0)
            m_isRotate = true;
        //this.transform.LookAt(m_Target);
        NextPattern();

        yield return new WaitForSeconds(3.0f);

        var count = 18;
        while (count>0) {
            var colliders = Physics.OverlapSphere(SpecialSkill.transform.position, 6.5f, LayerMask.GetMask("Player"));
            if (colliders.Length>0)
            {           
               PlayerCtrl.GetInstance().TakeDamage(10);
            }          
            yield return new WaitForSeconds(0.3f);
            count--;
        }
        yield return new WaitForSeconds(1f);
        //yield return null;
        SpecialSkill.SetActive(false);
      
    }
    private void EndOfLandMotion() {
        m_isChangePhase = false;
        NextPattern();
    }
    private void PatternFly()
    {
        //��� ����
        m_isChangePhase = true;
        m_Animator.SetInteger("Phase", 2);
        m_Animator.SetTrigger("isChangePhase");
        Debug.Log("�����մ���");
       // NextPattern();
    }
    private void PatternAttack()
    {
        m_distance = m_DirVec.magnitude;   
        if (m_AttackRange < m_distance)//���� ���� �ȿ� �ִ��� ��� üũ
        {
            ChangeState(State.Chase);//���� ���ο��� ���� 
            return;
        }
        Attack();
    }
    private void EndOfAttackMotion() {
        NextPattern();
    }

    private void PatternIdle()
    {
        ChangeState(State.Idle);//���� ���ο��� ���� 
    }

    #endregion
}
