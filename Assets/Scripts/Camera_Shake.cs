using UnityEngine;

public class Camera_Shake : MonoBehaviour
{
    public static Camera_Shake Instance { get; private set; }
    private Unity.Cinemachine.CinemachineVirtualCamera _camera;
    [SerializeField]private float shakeTimer;
    private void Awake()
    {
        Instance = this;
        _camera = GetComponent<Unity.Cinemachine.CinemachineVirtualCamera>();
        Unity.Cinemachine.CinemachineBasicMultiChannelPerlin tmp = _camera.GetCinemachineComponent<Unity.Cinemachine.CinemachineBasicMultiChannelPerlin>();
        tmp.AmplitudeGain = 0f;
    }

    public void ShakeCamera(float intensity,float time)
    {
        this.shakeTimer = time;
        Unity.Cinemachine.CinemachineBasicMultiChannelPerlin tmp = _camera.GetCinemachineComponent<Unity.Cinemachine.CinemachineBasicMultiChannelPerlin>();
        tmp.AmplitudeGain = intensity;
    }

    private void Update()
    {
        if (shakeTimer >= 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f) 
            {
                Unity.Cinemachine.CinemachineBasicMultiChannelPerlin tmp = _camera.GetCinemachineComponent<Unity.Cinemachine.CinemachineBasicMultiChannelPerlin>();
                tmp.AmplitudeGain = 0f;
            }
        }
    }
}
