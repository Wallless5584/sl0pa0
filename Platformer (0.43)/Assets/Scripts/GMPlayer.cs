using UnityEngine;

public class GMPlayer : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        GameManager.instance.SwitchState(GameManager.GameState.Play);
        Destroy(gameObject);
    }
}
