//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Rotate : MonoBehaviour
//{
//    [SerializeField]
//    private Transform m_Target = null;
//    [SerializeField]
//    private float m_rotata = 2f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        Application.targetFrameRate = 60;

//        Debug.LogFormat("{0}", this.transform.eulerAngles.y);
//        Debug.LogFormat("{0}", this.transform.rotation.y);
//    }

//    private float m_AddTimer = 0.1f;
//    // Update is called once per frame
//    void Update()
//    {
//        Action();

//        if (m_Angle > 0f)
//        {
//           // var value = Mathf.Clamp((float)m_Angle, 0f, 1f);
//           var value = Mathf.Lerp(0, m_Angle, Time.deltaTime* m_rotata);
          
//            if (m_reverse)
//                this.transform.RotateAround(this.transform.position, -Vector3.up, value);
//            else
//                this.transform.RotateAround(this.transform.position, Vector3.up, value);      

//        }
//    }


//    private bool m_reverse = false;
//    private float m_Angle = 0;
//    private void Action() {
//       var m_DirVec = m_Target.position - this.transform.position;
//        m_Angle = Vector3.Angle(transform.forward, m_DirVec); //180이하 양수다 
//        var dir = Vector3.Cross(transform.forward, m_Target.position);
//        if (m_Angle <=0.2f) {
//            m_Angle = 0f;
//        }
//            m_reverse = dir.y >= 0 ? false : true;

//        Debug.LogFormat("m_Angle {0}", m_Angle);
//    }
//}
