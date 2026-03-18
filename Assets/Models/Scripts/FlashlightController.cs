using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private Light flashlightLight;

    public static FlashlightController Instance;

    private bool isOn = false;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetLight(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;
        SetLight(isOn);
    }

    void SetLight(bool state)
    {
        flashlightLight.enabled = state;
    }

    public bool IsOn()
    {
        return isOn;
    }
    public Light GetLight()
    {
        return flashlightLight;
    }
}