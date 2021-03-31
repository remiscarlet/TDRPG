using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float shootForce;
    private Rigidbody projectileRb;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        projectileRb = GetComponent<Rigidbody>();
        projectileRb.AddForce(player.transform.forward * shootForce, ForceMode.Impulse); 
    }

    // Update is called once per frame
    void Update()
    {
    }
}
