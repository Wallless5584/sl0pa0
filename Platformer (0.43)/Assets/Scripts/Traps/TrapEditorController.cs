using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trap))]
public class TrapEditorController : MonoBehaviour
{
    const float c_pairRayMaxDistance = 30f;
    const float c_minimal_distance = 2;
    const float c_colliderOffset = 0.5f;

    Trap targetTrap;
    TrapAnimator target;
    TrapAnimator targetPair;

    void OnValidate()
    {
        targetTrap = GetComponent<Trap>();
        target = targetTrap.animators[0];
        if (targetTrap.animators.Length > 1)
            targetPair = targetTrap.animators[1];
    }



    //Pair
    public void SetPairBasePosition(bool castRay = true)
    {
        Vector2 rayStartPos = target.transform.position;

        // Raycast
        Vector2 rayHitPos = Vector2.zero;

        if (gameObject.scene.IsValid() && castRay)
        {
            PhysicsScene2D _scene = gameObject.scene.GetPhysicsScene2D();
            Physics2D.queriesHitTriggers = false;
            RaycastHit2D rayHit = _scene.Raycast(
                origin: rayStartPos,
                direction: target.transform.right,
                distance: c_pairRayMaxDistance,
                layerMask: targetTrap.groundLayer);
            //Physics2D.queriesHitTriggers = true; //commented out because whats the point?

            rayHitPos = rayHit.point;
        }

        if (rayHitPos == Vector2.zero)
        {
            //Debug.Log($"Position is zero: {rayHitPos == Vector2.zero}");
            rayHitPos = rayStartPos + (Vector2)target.transform.right * c_minimal_distance;
        }


        // calculations for pair pos
        Vector2 baseVsTrapOffset = (Vector2)transform.position - rayStartPos;
        Vector2 pairNewPos = new Vector2(
            x: Mathf.Round(rayHitPos.x) + baseVsTrapOffset.x,
            y: Mathf.Round(rayHitPos.y) + baseVsTrapOffset.y);
        targetPair.transform.position = pairNewPos;

        //Debug.Log($"SetPairBasePosition || rayHitPos {rayHitPos}, baseVsTrapOffset {baseVsTrapOffset}, pairNewPos {pairNewPos}");

        // Line Effect
        if (targetTrap.lineEffect)
        {   // Currently only for Lightning. Other throw an error
            Vector2 localEndPosition = pairNewPos - (Vector2)transform.position;
            targetTrap.lineEffect.GetComponent<Lightning>().SetEndPosition(localEndPosition);
        }
        else
            Debug.LogWarning($"{gameObject.name} : SetPairBasePosition || LineEffect not set!");


        targetTrap.boxCollider.offset = new Vector2(((rayHitPos - (Vector2)transform.position) / 2).magnitude, 0);      //half-size offset = center
        targetTrap.boxCollider.size = new Vector2((rayHitPos - (Vector2)transform.position).magnitude, targetTrap.boxCollider.size.y);
    }

    public bool CheckIfInDefaultPosition()
    {
        float c_marginOfError = 0.0001f;
        Vector2 defaultPosition = Vector2.right * c_minimal_distance;
        //Debug.Log($"targetPairLocalPos: {(Vector2)targetPair.transform.localPosition}, defaultPos: {defaultPosition}, diffMagnitude: {((Vector2)targetPair.transform.localPosition - defaultPosition).magnitude}");
        return ((Vector2)targetPair.transform.localPosition - defaultPosition).magnitude < c_marginOfError;
    }

    public void SetPairToDefaultPosition()
    {
        SetPairBasePosition(false);
    }

    public bool CheckForPair()
    {
        return targetPair != null;
    }

    

    // Position
    public void MoveByHalfGrid(Vector2 direction)
    {
        transform.position += (Vector3)(direction * 0.5f);
    }
    public void AllignToGrid()
    {
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }
    public bool CheckIsAllignedToGrid()
    {
        float c_marginOfError = 0.0001f;

        Vector3 roundedPos = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        return (transform.position - roundedPos).magnitude < c_marginOfError;
    }



    // Rotation
    public void Rotate(float angle)
    {
        transform.Rotate(new Vector3(0, 0, angle));
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public bool CheckIsRotated()
    {
        return transform.rotation != Quaternion.Euler(0, 0, 0);
    }
}
