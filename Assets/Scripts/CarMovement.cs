using System;
using CnControls;
using UnityEngine;

public class CarMovement : MonoBehaviour, IMovement
{
    private Character _character;
    
    private Rigidbody2D _rb = null;
    
    private readonly Vector2 _forward = new Vector2(0.0f, 0.5f);
    private Vector2 _rightAngleFromForward = Vector2.zero;
    private Vector2 _relativeForce = Vector2.zero;
    
    [SerializeField]
    private float _x = 0f;
    [SerializeField]
    private float _y = 0f;
    
    private float _acceleration = 0f;
    private float _steering = 0f;
    
    private bool isGas = false;
    private bool isBrake = false;
    
    private const int AccelerationDebuff = 10;
    private const int SteeringDebuff = 10;
    private const int Sensitivity = 2;

    public event Action OnMovement = delegate {  };

    private CarMovement()
    {
        OnMovement += Movement;
        OnMovement += Rotation;
        OnMovement += DriftMovement;
    }
    
    ~CarMovement()
    {
        OnMovement -= Movement;
        OnMovement -= Rotation;
        OnMovement -= DriftMovement;
    }

    private void Start() 
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _character = GetComponentInChildren<Character>();

        _acceleration = _character.acceleration;
        _steering = _character.steering;
    }

    void FixedUpdate()
    {
        _x = -CnInputManager.GetAxis("Horizontal") / Sensitivity;
        
        if (isGas && _y <= 1)
            _y += Time.deltaTime;
        if (isBrake && _y >= -1)
            _y -= Time.deltaTime;
        if (!isGas && !isBrake)
            _y = 0;
            
        OnMovement();
    }

    public void Movement()
    {
        var speed = transform.up * (_y * _acceleration / AccelerationDebuff);
        _rb.AddForce(speed);
        float direction = Vector2.Dot(_rb.velocity, _rb.GetRelativeVector(Vector2.up));
        
        if(direction >= 0.0f) 
        {
            _rb.rotation += _x * _steering / SteeringDebuff * (_rb.velocity.magnitude / 5.0f);
            //_rb.AddTorque((_x * _steering) * (_rb.velocity.magnitude / 10.0f));
        } 
        else 
        {
            _rb.rotation -= _x * _steering / SteeringDebuff * (_rb.velocity.magnitude / 5.0f);
            //_rb.AddTorque((-_x * _steering) * (_rb.velocity.magnitude / 10.0f));
        }
    }
    
    public void Rotation()
    {
        float steeringRightAngle;
        
        if(_rb.angularVelocity > 0) 
        {
            steeringRightAngle = -90;
        } 
        else 
        {
            steeringRightAngle = 90;
        }
        
        _rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * _forward;
    }
    
    public void DriftMovement()
    {
        float driftForce = Vector2.Dot(_rb.velocity, _rb.GetRelativeVector(_rightAngleFromForward.normalized));
        _relativeForce = _rightAngleFromForward.normalized * (-1.0f * (driftForce * 10.0f));
        _rb.AddForce(_rb.GetRelativeVector(_relativeForce));
    }

    public void PointerGasDown() => isGas = true;

    public void PointerBrakeDown() => isBrake = true;

    public void PointerUp()
    {
        if (isGas)
            isGas = false;
        if (isBrake) 
            isBrake = false;
    }
    

}