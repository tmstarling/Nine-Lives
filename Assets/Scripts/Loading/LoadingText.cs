using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    [SerializeField] string[] textAnim;
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        StartCoroutine(Infinite());
    }

    IEnumerator Infinite()
    {
        int i = 0;
        while (true)
        {
            text.text = textAnim[i];
            yield return new WaitForSeconds(0.2f);
            i++;
            if (i > textAnim.Length - 1)
            {
                i = 0;
            }
        }
    }
}
