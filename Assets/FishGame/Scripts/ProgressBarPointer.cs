using UnityEngine;

public class ProgressBarPointer : MonoBehaviour
{
    public Transform Target;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
    }
}
