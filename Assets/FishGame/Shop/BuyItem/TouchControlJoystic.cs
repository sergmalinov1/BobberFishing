using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlJoystic : MonoBehaviour
{


    public GameObject m_objecttorotate;
    public GameObject m_CameraTransform;
    public Joystick _joystick;

    [Space]
    public int m_minScale;
    public int m_maxScale;


    [Space]
    public float m_speedYMove = 4f;
    public int m_maxInc = 9;

    private float initialFingersDistance;

    private Vector3 initialScale;
    public Camera _camera;

    void Update()
    {

        _MoveCameraY();
        _Rotating();

        if (Input.touches.Length == 2)
        {
            _Scaling();
            return;
        }
    }


    void _Rotating()
    {
       m_objecttorotate.transform.Rotate(Vector3.down * _joystick.Horizontal * Time.deltaTime * 200f);
    }


    void _MoveCameraY()
    {
        m_CameraTransform.transform.Translate(Vector3.down * _joystick.Vertical * Time.deltaTime * m_speedYMove);
        /*
        if (IsObjectVisible(newPos))
        {
            //m_CameraTransform.transform.Translate(newPos);
        }
        */
    }

    void _Scaling()
    {
        if (Input.touches.Length == 2)
        {
            Touch t1 = Input.touches[0];
            Touch t2 = Input.touches[1];

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(t1.position, t2.position);
                initialScale = m_objecttorotate.transform.localScale;
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {

                float currentFingersDistance = Vector2.Distance(t1.position, t2.position);
                var scaleFactor = currentFingersDistance / initialFingersDistance;

                Vector3 m_scale = initialScale * scaleFactor;

                m_scale.x = Mathf.Clamp(m_scale.x, m_minScale, m_maxScale);
                m_scale.y = Mathf.Clamp(m_scale.y, m_minScale, m_maxScale);
                m_scale.z = Mathf.Clamp(m_scale.z, m_minScale, m_maxScale);
                m_objecttorotate.transform.localScale = m_scale;
            }
        }

    }


    bool IsObjectVisible(Vector3 checkedPosition)
    {
        Vector3 viewPos = _camera.WorldToViewportPoint(checkedPosition);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
