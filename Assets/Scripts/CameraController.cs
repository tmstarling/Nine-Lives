using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Mobement Limits
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float cameraPosOrig;
    float rotX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cursor settings
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse movement
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;

        //Movement Clamps
        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        //Vertical rotation limits
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //Apply Rotation
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //Horizontal rotation
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
