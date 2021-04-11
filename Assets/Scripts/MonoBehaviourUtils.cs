using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonoBehaviourUtils {

    /// <summary>
    /// Unused for now.
    ///
    /// Returns list of colliders applied to any child transform of <c>parent</c>
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Given a <c>potentialChildOrSelf</c> <c>GameObject</c>, will see if the potential child is either a
    /// child of <c>self</c> or is <c>self</c> itself.
    ///
    /// Traverses up the parent graph of the <c>potentialChildOrSelf</c> in a loop, checking each parent along the way.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="potentialChildOrSelf"></param>
    /// <returns>Bool representing whether <c>potentialChildOrSelf</c> is in the descendant graph of <c>self</c></returns>
    public static bool IsChildOrSelf(GameObject self, GameObject potentialChildOrSelf) {
        while (potentialChildOrSelf != null) {
            if (potentialChildOrSelf == self) {
                return true;
            } else if (potentialChildOrSelf.transform.parent == null) {
                break;
            }

            potentialChildOrSelf = potentialChildOrSelf.transform.parent.gameObject;
        }

        return false;
    }
}
