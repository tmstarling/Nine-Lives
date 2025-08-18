using TMPro;
using UnityEngine;
using UnityEngine.Events;

// before checkpoint manager, after regular objective manager
[DefaultExecutionOrder(1000)]
public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    class Objective
    {
        public UnityEvent onComplete;
        public GameObject objective;
        public IObjective _objective;
        public int checkpointID;

        public void Init()
        {
            if (objective != null)
                _objective = objective.GetComponent<IObjective>();
        }
    }

    public static ObjectiveManager instance;
    [TextArea]
    [SerializeField] string finalDescription;
    [SerializeField] Objective[] objectives;
    bool finished;
    Objective current => objectives[currentIdx];
    int currentIdx;

    public void SkipTo(int checkpointID)
    {
        foreach (Objective o in objectives)
        {
            if (o.checkpointID < checkpointID)
                currentIdx++;
            else
                break;
        }
    }

    void Awake()
    {
        instance = this;
        for (int i = 0; i < objectives.Length; i++)
            objectives[i].Init();
    }

    void UpdateDescription(string description)
    {
        if (gamemanager.instance.gameObjectiveText != null)
            gamemanager.instance.gameObjectiveText.text = description;
    }

    void CompleteObjective()
    {
        if (finished)
            return;
        if (objectives.Length == 0)
            return;
        current.onComplete?.Invoke();
        if (currentIdx + 1 >= objectives.Length)
        {
            finished = true;
            UpdateDescription(finalDescription);
            return;
        }
        currentIdx++;
        if (current._objective != null)
            current._objective.Register(UpdateDescription, CompleteObjective);
        else
            Debug.LogError("_objective was null, objects need to implement IObjective");
    }

    void Start()
    {
        if (objectives.Length == 0)
            return;
        if (current._objective != null)
            current._objective.Register(UpdateDescription, CompleteObjective);
        else 
            Debug.LogError("_objective was null, objects need to implement IObjective");
    }
}
