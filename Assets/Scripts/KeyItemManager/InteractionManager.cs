using UnityEngine;

public class InteractionManager : InterestPointManager
{
    //[Header("Interaction Settings")]

    //private bool wasInRangeLastFrame;
    //[SerializeField]
    //private bool isInRangeNow= false;

    // private void Update()
    // {
    //     base.Update(); // Mantenemos la funcionalidad base

    //     CheckForInteractables();
    //     UpdateInteractionState();
    // }

    // private void CheckForInteractables()
    // {
    //     Collider[] hitColliders = Physics.OverlapSphere(
    //         player.position, 
    //         interactionRange, 
    //         interactionLayer
    //     );

    //     if (hitColliders.Length > 0)
    //     {
    //         // Priorizamos el más cercano
    //         Collider closest = null;
    //         float closestDistance = float.MaxValue;

    //         foreach (var col in hitColliders)
    //         {
    //             float dist = Vector3.Distance(player.position, col.transform.position);
    //             if (dist < closestDistance)
    //             {
    //                 closestDistance = dist;
    //                 closest = col;
    //             }
    //         }

    //         currentInteractable = closest;
    //     }
    //     else
    //     {
    //         currentInteractable = null;
    //     }
    // }

    public void UpdateInteractionState(InterestPointData point, bool isInRangeNow)

    {

        if (isInRangeNow)
        {

            // InitializeIndicators();
            // point.icon.gameObject.SetActive(true);
            // point.visibilityTimer = showDuration;
            // point.velocity = Vector3.zero;
            Debug.Log("Interaction state updated: " + point.target.name + " is now in range.");

            }
        else
        {
            Debug.Log("No interactable in range");
        }
        }

}