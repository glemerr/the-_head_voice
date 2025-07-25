using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/High Jump")]
public class HighJumpPowerUp : PowerUp
{
    public float jumpMultiplier = 2f;

    public override void Activate(GameObject player)
    {
        //FirstPersonController controller = player.GetComponent<FirstPersonController>();
        Jump controller = player.GetComponent<Jump>();
        if (controller != null)
        {
            controller.StartCoroutine(ApplyHighJump(controller));
        }
    }

    private IEnumerator ApplyHighJump(Jump controller)
    {
        float originalJump = controller.jumpStrength;
        controller.jumpStrength *= jumpMultiplier;

        yield return new WaitForSeconds(duration);

        controller.jumpStrength = originalJump;
    }
}
