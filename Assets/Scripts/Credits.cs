using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 20f;
    [SerializeField] float fastScrollMultiplier = 3f;
    [SerializeField] float endYThreshold = 2000f;

    private RectTransform rectTransform;
    private Vector2 startPosition = new Vector2(0, -450f);
    private float endPosition = 2110f;
    private bool isPaused;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Scoll();
        Skip();
    }

    void Scoll()
    {
        if (isPaused)
        {
            return;
        }

        float currentSpeed = scrollSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed *= fastScrollMultiplier;
        }

        rectTransform.anchoredPosition += new Vector2(0, currentSpeed * Time.deltaTime);

        if (rectTransform.anchoredPosition.y >= endPosition)
        {
            isPaused = true;
        }
    }

    void Skip()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ReturnToMainMenu();
        }
    }

    void OnEnable()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        rectTransform.anchoredPosition = startPosition;
        isPaused = false;
    }

    void ReturnToMainMenu()
    {
        gamemanager.instance.menuActive.SetActive(false);

        if (gamemanager.instance.menuActive == gameObject)
        {
            gamemanager.instance.menuActive = null;
        }
    }
}
