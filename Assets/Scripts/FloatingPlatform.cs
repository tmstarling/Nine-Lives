using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{

    [SerializeField] float Speed;
    [SerializeField] float Height;

    private Vector3 startpos;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * Speed;
        float offsetY = Mathf.Sin(timer) * Height;
        transform.position = new Vector3(startpos.x, startpos.y + offsetY, startpos.z);
    }
}
