using System;
using UnityEngine;
using UnityEngine.Events;

public class EndPointTrigger : MonoBehaviour
{
    public UnityEvent OnTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTrigger?.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) OnTrigger?.Invoke();
    }
}