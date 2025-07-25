using UnityEngine;


public class EnvirormentsSoudns : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayRandomEnvironment();

        }
        //Destroy(this, 3f);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this, 3f);
            //AudioManager.Instance.StopRandomEnvironment();
        }
    }
}
