using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Game: MonoBehaviour
{
    [SerializeField] public float myShipXp, enemyShipXp, distance, newDistance, spaceTime = 0.001f;
    public int myBullSpeed;
    [SerializeField] public double playersAngle, correctAngle;
    public Text IDistanceText, IDebug, IBullSpeed, ICorrectAngle;
    public InputField IAngleText;
    public Slider IMyShipXpBar, IEnemyShipXpBar;
    public System.Random rnd = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        distance = rnd.Next(16000, 20000);
        newDistance = distance;
        myShipXp = 1000;
        enemyShipXp = myShipXp;
    }

    // Update is called once per frame
    void Update()
    {
        correctAngle = Math.Asin((distance/(myBullSpeed*myBullSpeed))*9.8)/2*180/3.1415;
        IDistanceText.text = distance.ToString();
        IMyShipXpBar.value = myShipXp;
        IEnemyShipXpBar.value = enemyShipXp;
        IBullSpeed.text = myBullSpeed.ToString();
        ICorrectAngle.text = correctAngle.ToString();
    }

    public void confirmButton() {
        correctAngle = Math.Asin((distance/(myBullSpeed*myBullSpeed))*9.8)/2*180/3.1415;
        playersAngle = Convert.ToDouble(IAngleText.text);

        if(Math.Abs(correctAngle - playersAngle) <= 1) {
            IDebug.text = "ПОПАЛ";
            enemyShipXp -= 100;
        } 
        else IDebug.text = "ПРОМАЗАЛ";

        if(enemyShipXp <= 0) IDebug.text = "ПОБЕДА";

        newDistance -= 1000;

        StartCoroutine(waiter());
        
    }

    IEnumerator waiter() {
        while(distance > newDistance) {
            distance -= 5;
            yield return new WaitForSeconds(spaceTime);
            }
    }

    private void Awake () {
        if(!getIntentData()) IDebug.text = "NOT Get Intent Data";
    }

    private bool getIntentData () {
        
    #if (!UNITY_EDITOR && UNITY_ANDROID)
        return CreatePushClass (new AndroidJavaClass ("com.unity3d.player.UnityPlayer"));
    #endif
        return false;
    }

    public bool CreatePushClass (AndroidJavaClass UnityPlayer) {
    #if UNITY_ANDROID
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject> ("getIntent");
        AndroidJavaObject extras = GetExtras (intent);

        IDebug.text = "started CreatPushClass";

        if (extras != null) {
            string ex = GetProperty (extras, "bull_speed");
            return true;
        }
        #endif
        return false;
    }

    private AndroidJavaObject GetExtras (AndroidJavaObject intent) {
        AndroidJavaObject extras = null;

        IDebug.text = "GetExtras";

        try {
            extras = intent.Call<AndroidJavaObject> ("getExtras");
        } catch (Exception e) {
            Debug.Log (e.Message);
            IDebug.text = "Error in GetExtras";
        }

        return extras;
    }

    private string GetProperty (AndroidJavaObject extras, string name) {
        string s = string.Empty;

        try {
            s = extras.Call<string> ("getString", name);
            myBullSpeed = int.Parse(s);
            IDebug.text = "Set bull speed";
            
        } catch (Exception e) {
            Debug.Log (e.Message);
            IDebug.text = "Error in GetProperty";
        }

        return s;
    }
}

