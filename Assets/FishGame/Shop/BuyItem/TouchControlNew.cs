using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlNew : MonoBehaviour
{
    /*
    public Transform Spawn;
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch screenTouch = Input.GetTouch(0);

            if (screenTouch.phase == TouchPhase.Moved)
            {
                Spawn.transform.Rotate(screenTouch.deltaPosition.y, screenTouch.deltaPosition.x, 0f);
            }

        }
    }
    */


    public GameObject m_objecttorotate;
    public GameObject m_CameraTransform;


    [Space]
    public int m_minScale;
    public int m_maxScale;


    [Space]
    public float m_speedYMove = 4f;
    public int m_maxInc = 9;

    private float initialFingersDistance;

    private Vector3 initialScale;
    private float m_firstpoint;
    private float m_secondpoint;


    private float m_firstpointY;
    private float m_secondpointY;

    private int m_inc = 0;



    private bool isMove = false;
    private bool isRotate = false;



    void Update()
    {

        if (Input.touchCount == 0)
        {
            m_inc = 0;
            return;
        }


        if (m_objecttorotate == null)
        {
            return;
        }


        if (Input.touchCount == 1)
        {
            if (m_inc == 0)
            {
                m_firstpoint = (int)Input.GetTouch(0).position.x;
                m_secondpoint = (int)Input.GetTouch(0).position.x;

                m_firstpointY = (int)Input.GetTouch(0).position.y;
                m_secondpointY = (int)Input.GetTouch(0).position.y;
            }

            m_inc++;

            if (m_inc <= m_maxInc)
            {
                return;
            }

            m_secondpoint = (int)Input.GetTouch(0).position.x;
            m_secondpointY = (int)Input.GetTouch(0).position.y;



            if (m_firstpoint < m_secondpoint)
            {
                isRotate = true;
                _Rotating(false);
            }
            else if (m_firstpoint > m_secondpoint)
            {
                isRotate = true;
                _Rotating(true);
            } else {
                isRotate = false;
            }

            if (m_firstpointY < m_secondpointY)
            {
                isMove = true;
                _MoveCameraY(false);
            }
            else if (m_firstpointY > m_secondpointY)
            {
                isMove = true;
                _MoveCameraY(true);
            } else
            {
                isMove = false;
            }


            return;
        }

        if (Input.touches.Length == 2)
        {
            _Scaling();
            return;
        }
    }


    private void LateUpdate()
    {
        if (m_inc >= m_maxInc)
        {
            m_firstpoint = (int)Input.GetTouch(0).position.x;
            m_firstpointY = (int)Input.GetTouch(0).position.y;
        }
    }

    void _Rotating(bool m_right)
    {
        if (isMove) {
            return;
        }

        if (m_right)
        {
            m_objecttorotate.transform.Rotate(Vector3.up * Time.deltaTime * 200f);
        }
        else
        {
            m_objecttorotate.transform.Rotate(Vector3.down * Time.deltaTime * 200f);
        }
    }



    void _MoveCameraY(bool m_Up)
    {
        if(isRotate)
        {
            return;
        }

        if (m_Up)
        {
            m_CameraTransform.transform.Translate(Vector3.up * Time.deltaTime * m_speedYMove); // .transform.position = Vector3.up * Time.deltaTime * 20f;
        }
        else
        {
            m_CameraTransform.transform.Translate(Vector3.down * Time.deltaTime * m_speedYMove);
        }

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



}
