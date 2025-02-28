using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public TabMenu Tabmenu;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        TimerUI.Instance.ResetTimer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Tabmenu == null)
            {
                Tabmenu = FindFirstObjectByType<TabMenu>(findObjectsInactive : FindObjectsInactive.Include);
            }
            
            Tabmenu.gameObject.SetActive(!Tabmenu.gameObject.activeSelf);
            Time.timeScale = Tabmenu.gameObject.activeSelf ? 0 : 1;
            CursorLock = Tabmenu.gameObject.activeSelf;
        }
          
    }

    public bool CursorLock
    {
        get => Cursor.lockState == CursorLockMode.Locked;
        set
        {
            if (!value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            Debug.Log(Cursor.lockState);
        }
    }
}