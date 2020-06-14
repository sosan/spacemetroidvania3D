using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemEvents : MonoBehaviour
{
    [SerializeField] UnityEvent[] eventsToDo;

    public void DoEvent(int i)
    {
        eventsToDo[i].Invoke();
    }
}
