using UnityEngine;

public class PickupObjective : TransformObjective, IObjective
{
    [Tooltip("Must have {0} for string replacement for pickup count")]
    [TextArea]
    [SerializeField]
    string description;
    [Tooltip("defaults to this transform on awake. Pickups must be a child of this transform.")]
    [SerializeField]
    Transform _pickupTransform;
    private void Awake()
    {
        if (_pickupTransform == null)
            _pickupTransform = transform;
    }
    private void Update() => UpdateLoop(description, _pickupTransform);
}
