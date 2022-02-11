using UnityEngine;

public class TouchController : MonoBehaviour
{
    public Transform SpawnObject;
    public Transform CameraTransform;
    public float rotateSpeedModifier = 0.6f;
    public float moveSpeedModifier = 0.03f;
    public float zoomSpeedModifier = 1f;
    private Quaternion rotationY;

    private Touch touch;
    private bool zoom = false;

    public float minZ = 50f;
    public float maxZ = 140f;

    private Vector3 initialTouch0Position;
    private Vector3 initialTouch1Position;

    void Start()
    {

    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //Vector3 CammeraPos = new Vector3(0f, CameraTransform.position.y + (-touch.deltaPosition.y * moveSpeedModifier), CameraTransform.position.z);
                //CameraTransform.position = CammeraPos;

                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                
                SpawnObject.position = Vector3.MoveTowards(SpawnObject.position, touchPos, Time.deltaTime * moveSpeedModifier);


                //SpawnObject.position = Vector3.Lerp(SpawnObject.position, newPos, Time.deltaTime * zoomSpeedModifier);
            }
        }
    }


    private void ControlMultitouch()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            zoom = false;

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 CammeraPos = new Vector3(0f, CameraTransform.position.y + (-touch.deltaPosition.y * moveSpeedModifier), CameraTransform.position.z);
                CameraTransform.position = CammeraPos;


                rotationY = Quaternion.Euler(0f,
                -touch.deltaPosition.x * rotateSpeedModifier,
                0f);

                SpawnObject.rotation = rotationY * SpawnObject.rotation;

            }
        }
        else
        {
            if (Input.touchCount == 2)
            {

                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                if (!zoom)
                {
                    initialTouch0Position = touch0.position;
                    initialTouch1Position = touch1.position;
                    zoom = true;
                }
                else
                {

                    float scaleFactor = GetScaleFactor(initialTouch0Position,
                                                       initialTouch1Position,
                                                       touch0.position,
                                                       touch1.position
                                                       );
                    //Vector3 newPos = new Vector3(Rod.position.x, Rod.position.y, Mathf.Clamp(Rod.position.z * scaleFactor, minZ, maxZ));

                    float xPos = GetXMove(initialTouch0Position,
                                          initialTouch1Position,
                                          touch0.position,
                                          touch1.position,
                                          SpawnObject.position.x
                                          );

                    Vector3 newPos = new Vector3(SpawnObject.position.x, SpawnObject.position.y, Mathf.Clamp(SpawnObject.position.z * scaleFactor, minZ, maxZ));

                    //Vector3 newPos = new Vector3(Rod.position.x + (touch.deltaPosition.x * moveSpeedModifier), Rod.position.y, Mathf.Clamp(Rod.position.z * scaleFactor, minZ, maxZ));


                    SpawnObject.position = Vector3.Lerp(SpawnObject.position, newPos, Time.deltaTime * zoomSpeedModifier);
                    zoom = false;
                }
            }
            else
            {
                zoom = false;
            }
        }
    }


    public static float GetScaleFactor(Vector2 position1, Vector2 position2, Vector2 oldPosition1, Vector2 oldPosition2)
    {
        float distance = Vector2.Distance(position1, position2);
        float oldDistance = Vector2.Distance(oldPosition1, oldPosition2);

        if (oldDistance == 0 || distance == 0)
        {
            return 1.0f;
        }

        return distance / oldDistance;
    }



    public static float GetXMove(Vector2 position1, Vector2 position2, Vector2 oldPosition1, Vector2 oldPosition2, float currentX)
    {
        float distance = position1.x - oldPosition1.x;
        float oldDistance = position2.x - oldPosition2.x;

        if (oldDistance == 0 || distance == 0)
        {
            return currentX;
        }
        else
        {
            return currentX + distance * 0.1f;
        }

        //return distance / oldDistance;
    }


}
