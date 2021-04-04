using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourUtils {
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
