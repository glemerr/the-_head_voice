using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/High Jump")]
public class HighJumpPowerUp : PowerUp
{
    public float jumpMultiplier = 2f;

    public override void Activate(GameObject player)
    {
        FirstPersonController controller = player.GetComponent<FirstPersonController>();
        if (controller != null)
        {
            controller.StartCoroutine(ApplyHighJump(controller));
        }
    }

    private IEnumerator ApplyHighJump(FirstPersonController controller)
    {
        float originalJump = controller.jumpPower;
        controller.jumpPower *= jumpMultiplier;

        yield return new WaitForSeconds(duration);

        controller.jumpPower = originalJump;
    }
}
