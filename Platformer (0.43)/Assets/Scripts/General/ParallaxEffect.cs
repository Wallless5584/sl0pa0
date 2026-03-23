using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] Vector3 RefPoint;
    [SerializeField] float ParallaxSpeed;
    Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = RefPoint+(RefPoint - GameManager.instance.PlayerInstance.transform.position) * ParallaxSpeed;
        material.mainTextureOffset = RefPoint + gameObject.transform.position * ParallaxSpeed;
    }
}
