using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/Speed Boost")]
public class SpeedBoostPowerUp : PowerUp
{
    public float speedMultiplier = 2f;

    public override void Activate(GameObject player)
    {
        FirstPersonController controller = player.GetComponent<FirstPersonController>();
        if (controller != null)
        {
            controller.StartCoroutine(ApplySpeedBoost(controller));
        }
    }


    private IEnumerator ApplySpeedBoost(FirstPersonController controller)
    {
        float originalSpeed = controller.walkSpeed;
        controller.walkSpeed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        controller.walkSpeed = originalSpeed;
    }
}
