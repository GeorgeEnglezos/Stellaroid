using UnityEngine;

public class GyroControl : MonoBehaviour
{
    
    private bool gyroEnabled;
    private Quaternion defaultRot;
    public float orientation;
    #region Singleton_region
    public static GyroControl Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion


    private Gyroscope gyro;
    // Start is called before the first frame update
    void Start()
    {
        resetGyro();
    }

    private void resetGyro()
    {
        gyroEnabled = EnableGyroscope();
        orientation = 0;
        defaultRot=new Quaternion(0, 0, 1, 0);
    }

    // Update is called once per frame
    private bool EnableGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Debug.Log("GyroControl: Supports Gyro");
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }else return false;
    }

    private void Update()
    {
        if (gyroEnabled)
        {
            if (Input.gyro.rotationRate.z != Input.gyro.attitude.z)
            {
//                Debug.Log("GyroControl: RotationRate: " + Input.gyro.rotationRate.z + " Unbiased: " +Input.gyro.attitude.z);

            }

            orientation = Input.gyro.attitude.z;
        }
    }
}
