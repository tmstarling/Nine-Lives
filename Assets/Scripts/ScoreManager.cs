using TMPro;
using UnityEngine;
using System.Linq;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI[] scoreList;
    [System.Serializable]
    class ScoreData
    {
        public string scoreName;
        public int score;

        public ScoreData(int score, string scoreName)
        {
            this.score = score;
            this.scoreName = scoreName;
        }
    }

    ScoreData[] scores = new ScoreData[10];
    public void SaveScore() => AddScore(new ScoreData(CheckpointManager.instance.GetScore(), scoreText.text));
    void AddScore(ScoreData newScore)
    {
        var notnull = scores.Where((x) => x != null).ToList();
        notnull.Add(newScore);
        notnull.Sort((x, y) => x.score.CompareTo(y.score));
        if (notnull.Count > 10)
            notnull.RemoveAt(notnull.Count() - 1);
        while (notnull.Count < 10)
            notnull.Add(null);
        scores = notnull.ToArray();
        SaveScores();
    }

    void BlitScores()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == null)
            {
                scoreList[i].gameObject.SetActive(false);
                continue;
            }
            scoreList[i].gameObject.SetActive(true);
            scoreList[i].text = string.Format("{0}: {1}", scores[i].scoreName, scores[i].score);
        }
    }


    private void Awake()
    {
        LoadScoresInit();
    }

    private void OnDestroy()
    {
        SaveScores();
    }

    public void LoadScoresInit() {
        for (int i = 0; i < 10; i++)
            scores[i] = JsonUtility.FromJson<ScoreData>(PlayerPrefs.GetString(string.Format("PlayerScore{0}", i), "null"));
        BlitScores();
    }

    public void SaveScores()
    {
        for (int i = 0; i < 10; i++)
            PlayerPrefs.SetString(string.Format("PlayerScore{0}", i), JsonUtility.ToJson(scores[i]));
        BlitScores();
    }
}
