using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Game: MonoBehaviour
{
    [SerializeField] public float myShipXp, enemyShipXp, distance, newDistance = 18000, my_bull_speed = 500;
    [SerializeField] public double playersAngle, correctAngle;
    public float spaceTime = 0.001f;
    public Text IDistanceText;
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
        IDistanceText.text = distance.ToString();
        IMyShipXpBar.value = myShipXp;
        IEnemyShipXpBar.value = enemyShipXp;
    }

    public void confirmButton() {
        correctAngle = Math.Asin((distance/(my_bull_speed*my_bull_speed))*9.8)/2*180/3.1415;
        playersAngle = Convert.ToDouble(IAngleText.text);

        if(Math.Abs(correctAngle - playersAngle) <= 1) {
            Debug.Log("Попал");
            enemyShipXp -= 100;
        } 
        else Debug.Log("Промазал");

        if(enemyShipXp <= 0) Debug.Log("ПОБЕДА");

        newDistance -= 1000;

        StartCoroutine(waiter());
        
    }

    public static String getExtraText() {
    String txtName = getIntent().getStringExtra("name");
    // Store extra parameter for later.
    Intent intent = UnityPlayer.currentActivity.getIntent();

    if (intent != null) {
        String action = intent.getAction();
        String type = intent.getType();

        if (action.equals(Intent.ACTION_VIEW) && type != null) {
            if (type.equals("text/plain")) {
                extraText = intent.getStringExtra(Intent.EXTRA_TEXT);
                DebugBridge.log_d("Extra Text: " + extraText);
            } else {
                DebugBridge.toast("Unknown MIME type");
            }
        }
    }

    return extraText;
}

    IEnumerator waiter() {
        while(distance > newDistance) {
            distance -= 5;
            yield return new WaitForSeconds(spaceTime);
            }
    }

    
}
