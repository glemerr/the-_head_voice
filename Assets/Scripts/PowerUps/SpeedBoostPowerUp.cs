using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/Speed Boost")]
public class SpeedBoostPowerUp : PowerUp
{
    public float speedMultiplier = 2f;

    public override void Activate(GameObject player)
    {
        FirstPersonMovement controller = player.GetComponent<FirstPersonMovement>();
        if (controller != null)
        {

            controller.StartCoroutine(ApplySpeedBoost(controller));
        }
    }


    private IEnumerator ApplySpeedBoost(FirstPersonMovement controller)
    {
        float originalSpeed = controller.speed ;
        controller.speed  *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        controller.speed  = originalSpeed;
    }
}
