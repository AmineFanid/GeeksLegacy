// EventManager
// Permet d'applique le patron de conception Observer
// Auteur: Amine Fanid 
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using static Unity.VisualScripting.FlowStateWidget;

public class EventManager : MonoBehaviour
{

    public enum PossibleEvent
    {
        eVieJoueurChange = 0,
        eMortJoueur = 1,
        eMortEnnemi = 2,
        eMAX
    }

    private List<UnityEvent<object>> eventList;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindFirstObjectByType(typeof(EventManager)) as EventManager; 

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventList == null)
        {
            eventList = new List<UnityEvent<object>>();
            for (int i = 0; i < (int)PossibleEvent.eMAX; i++)
            {
                eventList.Add(new UnityEvent<object>());
            }
        }
    }

    public static void StartListening(PossibleEvent eventEnum, UnityAction<object> listener)
    {
        instance.eventList[(int)eventEnum].AddListener(listener);
    }

    public static void StopListening(PossibleEvent eventEnum, UnityAction<object> listener)
    {
        instance.eventList[(int)eventEnum].RemoveListener(listener);
    }

    public static void TriggerEvent(PossibleEvent eventEnum, object data)
    {
        instance.eventList[(int)eventEnum].Invoke(data);
    }
}
