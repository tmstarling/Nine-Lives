using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{

    [SerializeField] float Speed;
    [SerializeField] float Height;
    [SerializeField] float ForwardRange;
    public Vector3 ForwardDirection = Vector3.forward;

    private Vector3 startpos;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startpos = transform.position;
        ForwardDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * Speed;

        float offsetY = Mathf.Sin(timer) * Height;

        float offsetFor = Mathf.Cos(timer) * ForwardRange;

        Vector3 offset = new Vector3(0, offsetY, 0) + ForwardDirection * offsetFor;
        transform.position = startpos + offset;
    }
}
