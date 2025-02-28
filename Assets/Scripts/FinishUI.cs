using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishUI : MonoBehaviour
{
    public TMP_Text text;
    public Button button;
    private void Start()
    {
        text.text = $"за {TimerUI.Instance.seconds} сек.";
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) button.onClick.Invoke();
    }
}