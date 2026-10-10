using UnityEngine;

public class Spaceship_Controller : MonoBehaviour
{
    
    public static Spaceship_Controller Instance;
    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        stopAllThrusters();
    }

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    
    [Header("Movemenet_Vars")]
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private bool _thrusting;
    [SerializeField] private float _turboSpeed;
    [SerializeField] private bool _sprinting;
    [SerializeField] private float _turnDirection;
    [SerializeField] private float orientationValue;
    
    [Header("Thruster_Effects")] 
    [SerializeField] private ParticleSystem middleThruster; 
    [SerializeField] private ParticleSystem leftThruster;
    [SerializeField] private ParticleSystem rightThruster;
    [SerializeField] private TrailRenderer nitroTrail;
    


    private void resetSpaceshipController()
    {
        Debug.Log("Reseted Spaceship");
        _rb.linearVelocity=Vector2.zero;
        _thrusting = false;
        _sprinting = false;
        _turnDirection = 0;
        transform.position=new Vector3(0,0,0);
        //transform.rotation=new Quaternion(0,0,0,0);
    }
    private void Update()
    {
        if (GameManager.Instance.DesktopBuild)
        {
            _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            _sprinting = Input.GetKey(KeyCode.LeftShift);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _turnDirection = 1f;
            }else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _turnDirection = -1f;
            }
            else //Stopped moving
            {
                _turnDirection = 0;
            }
        }else if (GameManager.Instance.AndroidBuild)
        {
            if (GyroControl.Instance.orientation<-orientationValue)
            {
                _turnDirection = -1f;
            }else if (GyroControl.Instance.orientation>orientationValue)
            {
                _turnDirection = 1f;
            }           
            else //Stopped moving
            {
                _turnDirection = 0;
            }
        }

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameStarted)
        {
                    if (!_sprinting)
                    {
                        nitroTrail.emitting=false;
                    }
                    else
                    {
                        nitroTrail.emitting=true;
            
                    }
            
                    if (_thrusting)
                    {
                        if (!_sprinting)
                        {
                            _rb.AddForce(transform.up*_speed);
                        }
                        else
                        {
                            _rb.AddForce(transform.up*_turboSpeed);
            
                        }
                        startMiddleThruster();
                    }
                    else
                    {
                        middleThruster.Stop();
                    }
            
                    if (_turnDirection!=0)
                    {
                        _rb.AddTorque(_turnDirection*_turnSpeed);
                        if (_turnDirection>0)
                        {
                            leftThruster.Stop();
                            StartRightThruster();
                        }
                        else if (_turnDirection<0)
                        {
                            rightThruster.Stop();
                            StartLeftThruster();
                        }
                    }else
                    {
                        rightThruster.Stop();
                        leftThruster.Stop();
                    }
        }

    }

    private void startMiddleThruster()
    {
        if (!middleThruster.isPlaying)
        {
            middleThruster.Play();
        }
    }
    private void StartRightThruster()
    {
        if (!rightThruster.isPlaying)
        {
            rightThruster.Play();
        }
    }
    private void StartLeftThruster()
    {
        if (!leftThruster.isPlaying)
        {
            leftThruster.Play();
        }
    }
    private void stopAllThrusters()
    {
        middleThruster.Stop();
        leftThruster.Stop();
        rightThruster.Stop();
    }
    
    
    
    
    //UI INPUT
    public void pressedThrust()
    {
        _thrusting = true;
    }
    public void pressedNitro()
    {
        _sprinting = true;
    }
    public void releasedThrust()
    {
        Debug.Log("released thrust");
        _thrusting = false;
    }
    public void releasedNitro()
    {
        Debug.Log("released nitro");
        _sprinting = false;
    }
}
