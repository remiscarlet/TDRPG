using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour {
    public float MaxInteractDistance { set; get; } = 10.0f;
    public virtual bool IsPlayerInRange() {
        return Vector3.Distance(ReferenceManager.PlayerObject.transform.position, transform.position) <
               MaxInteractDistance;
    }

    public virtual void Activate() {
        throw new System.Exception("Unimplemented Exception");
    }
}
