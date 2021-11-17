using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] 
    private Transform _target = null;

    [SerializeField]
    private Vector3 _offset = Vector3.zero ;
    
    private Vector3 _destinationPoint = Vector3.zero;

    [SerializeField] 
    private float _smoothness = 0;
    private void FixedUpdate()
    {
        var targetPosition = _target.position;
        _destinationPoint = targetPosition + _offset;
        transform.position = Vector3.Lerp(transform.position, _destinationPoint, _smoothness);
    }
}
