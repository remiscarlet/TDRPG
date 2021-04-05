using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonoBehaviourUtils {

    public static List<Collider> GetChildColliders(GameObject parent) {
        List<Collider> colliders = new List<Collider>();

        Collider parentCollider = parent.GetComponent<Collider>();
        if (parentCollider != null) {
            colliders.Add(parentCollider);
        }

        foreach (Transform childTransform in parent.transform) {
            colliders = colliders.Concat(GetChildColliders(childTransform.gameObject)).ToList();
        }

        return colliders;
    }

    public static bool IsChild(GameObject self, GameObject potentialChild) {
        while (potentialChild != null) {
            if (potentialChild == self) {
                return true;
            } else if (potentialChild.transform.parent == null) {
                break;
            }

            potentialChild = potentialChild.transform.parent.gameObject;
        }

        return false;
    }
}
