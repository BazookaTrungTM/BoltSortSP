using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEndEvent : MonoBehaviour
{
    public void CloseEvent()
    {
        gameObject.SetActive(false);
    }
}
