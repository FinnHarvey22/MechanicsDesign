using UnityEngine;

public static class BulletTime 
{
    private static float timeScale = 1.0f;
    public static float deltaTime { get { return Time.deltaTime * timeScale; } }
    static public void StartBulletTime()
    {
        timeScale = 0.33f; 
    }

    static public void StopBulletTime()
    {
        timeScale = 1f;
    }
}



//globally accesable - static
// adjusted time scale variable
//function to get adjusted delta time 