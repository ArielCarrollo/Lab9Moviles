using UnityEngine;
using Firebase.Analytics;
using Firebase;

public class FireBaseInit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(
            task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                var app = FirebaseApp.DefaultInstance;
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
