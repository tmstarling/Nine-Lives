using UnityEngine;

public class FloatingPlatforms : MonoBehaviour
{
    public float Speed;
    public float Height;

    private Vector3 StartPos;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * Speed;
        float offsetY = Mathf.Sin(timer) * Height;
        transform.position = new Vector3(StartPos.x, StartPos.y + offsetY, StartPos.z);
    }
}
