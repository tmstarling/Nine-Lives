using UnityEngine;

public class KillObjective : TransformObjective, IObjective
{
    [Tooltip("Must have {0} for string replacement for enemy count")]
    [TextArea]
    [SerializeField]
    string description;
    [Tooltip("defaults to this transform on awake. enemies that need to be killed must be a child of this transform.")]
    [SerializeField]
    Transform _enemiesTransform;
    private void Awake()
    {
        if (_enemiesTransform == null)
            _enemiesTransform = transform;
    }
    private void Update() => UpdateLoop(description, _enemiesTransform);
}
