using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] CameraController cameraController;
    [SerializeField] TMP_InputField distanceInputField;
    
    public void onValueChanged()
    {
        cameraController._mouseSensitivity = slider.value;
    }

    public void SetDistance()
    {
       WebSwing._maxSwingDistance = int.Parse(distanceInputField.text);
    }
    
}