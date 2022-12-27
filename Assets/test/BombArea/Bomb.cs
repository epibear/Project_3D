using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float radius = 4.5f;

    [SerializeField]
    private Material material = null;


    [SerializeField]
    private Material basematerial = null;


    public MeshRenderer[] m_objs = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {         
           // var colliders = Physics.OverlapSphere(this.transform.position, radius);
            
            var colliders = Physics.OverlapSphere(this.transform.position, radius, LayerMask.GetMask("Player"));
            if (colliders != null)
            {             
                foreach (Collider collider in colliders)
                {
                    collider.transform.GetComponent<MeshRenderer>().material = material;
                }
            }            
        }
        if (Input.GetMouseButtonDown(1)) {
            foreach (MeshRenderer obj in m_objs) {
                obj.material = basematerial;
            }        
        }


    }

}
