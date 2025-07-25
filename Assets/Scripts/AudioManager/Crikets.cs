using UnityEngine;

public enum CricketsType
{
    Criket1,
    water,
    Cricket3,
}
public class Crikets : MonoBehaviour
{
    public CricketsType cricketsType;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //AudioManager.Instance.PlayCrickets(); // Start crickets in forest
            switch (cricketsType)
            {
                case CricketsType.Criket1:
                    AudioManager.Instance.PlayCrickets();
                    break;
                case CricketsType.water:
                    AudioManager.Instance.PlayWater();
                    break;
                case CricketsType.Cricket3:
                    AudioManager.Instance.PlayCrickets();
                    break;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //AudioManager.Instance.StopCrickets(); // Stop crickets in forest
            switch (cricketsType )
            {
                case CricketsType.Criket1:
                    AudioManager.Instance.StopCrickets();
                    break;
                case CricketsType.water:
                    AudioManager.Instance.StopWater() ;
                    break;
                case CricketsType.Cricket3:
                    AudioManager.Instance.StopCrickets();
                    break;
            }
            
        }
    }
}
