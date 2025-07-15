// Create test script NotificationTester.cs
using UnityEngine;

public class NotificationTester : MonoBehaviour
{
    public Sprite testIcon;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Showing Info Notification");
            NotificationManager.Instance.ShowItemNotification(
                "Health Potion", 
                "Restores 50 HP", 
                testIcon
            );
        }
        
        if(Input.GetKeyDown(KeyCode.K))
        {
            NotificationManager.Instance.ShowMissionNotification(
                "New Mission: Rescue Villagers",
                "Find the hostages in the forest camp"
            );
        }
        
        if(Input.GetKeyDown(KeyCode.L))
        {
            NotificationManager.Instance.ShowWarning(
                "Low Health!",
                "Find healing items immediately"
            );
        }
    }
}