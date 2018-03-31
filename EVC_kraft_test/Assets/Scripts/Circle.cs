using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

    public float timeLeft; // остаток времени до изменения цвета
    public float timeSetting; // максимально возможное время, в течение которого круг будет держать цвет
    public GameManager GM; // объект Game Manager, управляющий событиями игры
    public UnityEngine.UI.Image bar; // полоска времени для timeLeft

	// Use this for initialization
	void Start () 
    {
        timeSetting = GM.circleTime;
        SetTimeLeft();
	}

    void SetTimeLeft()
    {
        timeLeft = timeSetting * Random.Range(0.3f, 1f);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (GM.playingAllowed == true)
        {
            if (timeLeft > 0)
                timeLeft -= Time.deltaTime;
            else
            {
                this.GetComponent<Shape>().SetColor();
                SetTimeLeft(); 
            }
        }
        bar.fillAmount = timeLeft / timeSetting;
        bar.color = this.GetComponent<SpriteRenderer>().color;
	}
}
