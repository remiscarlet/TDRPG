using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour {

    public GameObject target;
    // Start is called before the first frame update
    void Start() {
        // Default to targetting player
        target = ReferenceManager.PlayerObject;
    }

    public void SetTarget(GameObject target) {
        // With option to override target
        target = target;
    }

    // Update is called once per frame
    void Update() {
        FaceTarget(gameObject, target);
    }

    private void FaceTarget(GameObject self, GameObject target) {
        Vector3 directionToTarget = self.transform.position - target.transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget); 
        self.transform.rotation = rotationToTarget;
    }
}
