using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/Invisibility")]
public class InvisibilityPowerUp : PowerUp
{
    public override void Activate(GameObject player)
    {
        FirstPersonController controller = player.GetComponent<FirstPersonController>();
        if (controller != null)
        {
            controller.StartCoroutine(ApplyInvisibility(controller));
        }
    }

    private IEnumerator ApplyInvisibility(FirstPersonController controller)
    {
        controller.isInvisible = true;

        yield return new WaitForSeconds(duration);

        controller.isInvisible = false;
    }
}
