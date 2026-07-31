using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Update()
    {
        if (GameManager.instance.PlayerInstance)
        {
            var player = GameManager.instance.PlayerInstance;
            Vector3 cam_pos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            transform.position = cam_pos;
        }
    }
}
