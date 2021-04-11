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
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = ReferenceManager.EventSystemComponent;
    }

    /// <summary>
    /// Returns if the center of the screen is facing a Canvas UI element.
    ///
    /// This is separate from physics raycast checks due to UI elems needing
    /// to explicitly use GraphicRaycaster.Raycast and not Physics.Raycast
    /// </summary>
    /// <param name="hitGameObject"></param>
    /// <returns>True if center of screen is facing a UI elem. False otherwise. </returns>
    public bool IsPlayerFacingUIElem(out GameObject hitGameObject) {
        if (m_EventSystem == null) {
            // Awake() and Start() aren't called until gameobject is enabled seems like
            hitGameObject = null;
            return false;
        }
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = new Vector2(Screen.height / 2, Screen.width / 2);

        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count == 0) {
            hitGameObject = null;
            return false;
        }

        // TODO: Are results sorted by distance?
        hitGameObject = results[0].gameObject;
        print(hitGameObject);
        return true;
    }
}
