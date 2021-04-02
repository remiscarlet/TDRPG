//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIRaycasterUtil : MonoBehaviour {
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Awake()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = ReferenceManager.EventSystemComponent;

        print("Attempted to initialize UIRaycasterUtil");
        print(m_Raycaster);
        print(m_EventSystem);
    }

    public bool IsPlayerFacingUIElem(out GameObject hitGameObject) {
        if (m_EventSystem == null) {
            // Awake() and Start() aren't called until gameobject is enabled seems like
            hitGameObject = null;
            return false;
        }
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = new Vector2(Screen.height / 2, Screen.width / 2);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
        
        if (results.Count == 0) {
            hitGameObject = null;
            return false;
        }

        // TODO: Are results sorted by distance?
        hitGameObject = results[0].gameObject;
        return true;
    }
}