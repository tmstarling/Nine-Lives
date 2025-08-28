using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DirectionalDamage : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public Vector3 damageWorldPosition;
    public Transform indicatorPivot;
    public CanvasGroup canvasGroup;

    public Image indicatorImage;

    [Header("Fade Settings")]
    public float fadeDelay;
    public float fadeDuration;

    private float fadeTimer;

    void Start()
    {
        Debug.Log("DirectionalDamage: Initialized");

        if (playerTransform == null || indicatorPivot == null || canvasGroup == null)
        {
            //Debug.LogWarning("DirectionalDamage: Missing references.");
            Destroy(gameObject);
            return;
        }

        damageWorldPosition.y = playerTransform.position.y;
        Vector3 directionToDamage = (damageWorldPosition - playerTransform.position).normalized;
        float angle = Vector3.SignedAngle(directionToDamage, playerTransform.forward, Vector3.up);
        indicatorPivot.localEulerAngles = new Vector3(0, 0, angle);

        //Debug.Log($"DirectionalDamage: angle = {angle}, adjusted = {angle - 90f}");
        //Debug.Log($"Indicator Position: {transform.position}, Rotation: {indicatorPivot.localEulerAngles}");

        if (indicatorImage != null)
        {
            indicatorImage.color = Color.magenta;
        }

        canvasGroup.alpha = 1f;
        transform.localPosition = Vector3.zero;

        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        yield return new WaitForSeconds(fadeDelay);
        fadeTimer = fadeDuration;

        while (fadeTimer > 0f)
        {
            fadeTimer -= Time.deltaTime;
            canvasGroup.alpha = fadeTimer / fadeDuration;
            yield return null;
        }

        Destroy(gameObject);
    }
}