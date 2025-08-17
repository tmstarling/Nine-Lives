using UnityEngine;

public class PickUpAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] float floatAmplitude;
    [SerializeField] float floatFrequency;
    [SerializeField] float rotationSpeed;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}