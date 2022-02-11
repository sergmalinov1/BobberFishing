using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMass : MonoBehaviour
{


    public Vector3 centerOfMass;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }

}
