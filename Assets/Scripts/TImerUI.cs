using System;
using TMPro;
using UnityEngine;

public class TimerUI : Singleton<TimerUI>
{
    [Header("Links")]
    [SerializeField] private TMP_Text timerText;   // Ссылка на текст таймера
    [SerializeField] private TMP_Text gameResult;   // Ссылка на текст результата 

    public bool isActive { get; set; }

    public int seconds = 0;  // Время таймера
    private float timer; // Переменная для отсчета 1 секунды
    
    private void Start()
    {
        isActive = true;
        seconds = 0 ; 
        timer = 0;
    }

    void Update()
    {
        if (!isActive) return;
        timer += Time.deltaTime;
     
        if (!(timer >= 1)) return;  // если секунда не прошла 
       
        // секунда прошла, обнуляем счетчик
        timer = 0;   
        // увеличиваем на 1 секунды
        seconds++;
        UpdateTimerDisplay(seconds);
    }

    private void OnValidate()
    {
        if (timerText == null) timerText = GetComponent<TMP_Text>();
    }

    private void UpdateTimerDisplay(float timeInSeconds)
    {
        // Отображаем время в формате "XX секунд"
        timerText.text = seconds.ToString() + " сек.";
    }


    // оценка результата
    public void EvaluatePerformance()
    {
        gameResult.text = "Вы прошли уровень за " + seconds + " сек.";
    }
    // сброс нашего таймера 
    public void ResetTimer()
    {
        timer = 0;
        seconds = 0;
    }
}