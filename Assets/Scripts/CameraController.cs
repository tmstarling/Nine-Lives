using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Limits")]
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] float followSpeedSide;
    [SerializeField] float followSpeedVertical;
    [SerializeField] float yMomentum;
    [SerializeField] float yClampMin, yClampMax;
    [SerializeField] float cameraShakePointBounds;
    [SerializeField] float shakeSpeed;
    [SerializeField] int maxShakes;
    Transform _parent;
    CharacterController _charachterController;
    PlayerController _playerController;
    Transform _child;
    Vector3 _childOriginalPos;
    Vector3 _currentShakePos;
    float _cameraShakePointTimer;
    int _shakesLeft = 0;
    float _rotX;
    float _rotY;
    Vector3 _shakeTarget;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _charachterController = GetComponentInParent<CharacterController>();
        _playerController = GetComponentInParent<PlayerController>();
        _playerController.takeDamage += StartDamageShake;
        _parent = transform.parent;
        _child = transform.GetChild(0);
        _childOriginalPos = _child.localPosition;
        _currentShakePos = _childOriginalPos;
        _shakeTarget = _childOriginalPos;
        transform.parent = null;
    }
    private void OnDestroy()
    {
        _playerController.takeDamage -= StartDamageShake;
    }

    void StartDamageShake()
    {
        if (_shakesLeft > 0)
            return;
        _shakesLeft = maxShakes;
        NewShakeTarget();
    }

    void NewShakeTarget()
    {
        var ranX = Random.Range(-cameraShakePointBounds, cameraShakePointBounds);
        var ranY = Random.Range(-cameraShakePointBounds, cameraShakePointBounds);
        _shakeTarget = _childOriginalPos + new Vector3(ranX, ranY, 0.0f);
    }

    void UpdatePosition()
    {
        float x = Mathf.Lerp(transform.position.x, _parent.position.x, followSpeedSide);
        float z = Mathf.Lerp(transform.position.z, _parent.position.z, followSpeedSide);
        float y = Mathf.Lerp(transform.position.y, _parent.position.y + _charachterController.velocity.y * yMomentum, followSpeedVertical);
        float yTarget = Mathf.Clamp(y, _parent.position.y + yClampMin, _parent.position.y + yClampMax);
        transform.position = new Vector3(x, yTarget, z);
        if (_shakesLeft > 0)
        {
            _cameraShakePointTimer += Time.deltaTime * shakeSpeed;
            _currentShakePos = Vector3.Lerp(_currentShakePos, _shakeTarget, _cameraShakePointTimer);
            if (_cameraShakePointTimer > 1.0f)
            {
                _shakesLeft--;
                _cameraShakePointTimer = 0.0f;
                if (_shakesLeft > 1)
                    NewShakeTarget();
                else
                    _shakeTarget = _childOriginalPos;
            }
        }
        _child.transform.localPosition = Vector3.Lerp(_child.transform.localPosition, _currentShakePos, 0.025f);
    }


    void UpdateRotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        if (invertY)
            _rotX += mouseY;
        else
            _rotX -= mouseY;
        _rotX = Mathf.Clamp(_rotX, lockVertMin, lockVertMax);
        _rotY += mouseX;
        transform.rotation = Quaternion.Euler(_rotX, _rotY, 0);
        _parent.localRotation = Quaternion.Euler(0, _rotY, 0);
    }

    void Update()
    {
        UpdatePosition();
        UpdateRotation();
    }
}
