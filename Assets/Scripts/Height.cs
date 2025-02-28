using System;
using TMPro;
using UnityEngine;

public class Height : MonoBehaviour
{
    public Transform target;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
}