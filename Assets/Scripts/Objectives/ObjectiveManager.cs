using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    [TextArea]
    [SerializeField] string finalDescription;
    [SerializeField] TextMeshProUGUI description;

    [System.Serializable]
    class Objective
    {
        public UnityEvent onComplete;
        // public UnityEvent onUndo; TODO: Checkpoints
        public GameObject objective;
        public IObjective _objective;

        public void Init()
        {
            if (objective != null)
                _objective = objective.GetComponent<IObjective>();
        }
    }


    [SerializeField] Objective[] objectives;
    bool finished;
    Objective current => objectives[currentIdx];
    int currentIdx;
    void Awake()
    {
        instance = this;
        for (int i = 0; i < objectives.Length; i++)
            objectives[i].Init();
    }

    void UpdateDescription(string description)
    {
        if (this.description != null)
            this.description.text = description;
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
    }

    void Start()
    {
        if (objectives.Length == 0)
            return;
        if (current._objective != null)
            current._objective.Register(UpdateDescription, CompleteObjective);
    }
}
