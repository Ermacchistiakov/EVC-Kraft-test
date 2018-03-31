using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Shape : MonoBehaviour {

    SpriteRenderer shapeRenderer;
    int numberOfColors;
    public int rc = -1;
    public GameManager GM;
    int shapeIndex;

	// Use this for initialization
	void Start () 
    {
        this.shapeRenderer = this.GetComponent<SpriteRenderer>();
        GM.ReadingFile();
        numberOfColors = GM.NumberOfColors;
        if (this.GetComponent<Circle>())
        {
                shapeIndex = GM.circleNumber;
                GM.circleNumber++;
        }
        if (this.GetComponent<Square>() && this.transform.position.y < 6)
        {
            shapeIndex = GM.squareNumber;
            GM.squareNumber++;
        }
        SetColor();


	}

    void OnMouseDown()
    {
       Debug.Log(shapeIndex);
    }

    public void SetColor()
    {
        if (this.GetComponent<Circle>())
        {
            rc = Random.Range(0, numberOfColors);
            for (int i = 0; i < 3; i++)
            {
                if (rc == GM.circleColors[i])
                {
                    rc++;
                    if (rc > numberOfColors)
                    {
                        rc = 0;
                    }
                    i = -1;
                }
            }
            this.shapeRenderer.color = new Color(GM.colorList[rc, 0], GM.colorList[rc, 1], GM.colorList[rc, 2]);
            GM.circleColors[shapeIndex] = rc;
            
            GM.lccIndex++;
            if (GM.lccIndex == 3)
            {
                for (int j = 1; j < 3; j++)
                {
                    GM.lastCircleCols[j-1] = GM.lastCircleCols[j];
                }
                GM.lccIndex = 2;
            }
            GM.lastCircleCols[GM.lccIndex] = rc;
            
            
        }

        if (this.GetComponent<Square>())
        {
            int rc = Random.Range(0, numberOfColors);
            for (int k = 0; k < 3; k++)
            {
                if (rc == GM.squareColors[k])
                {
                    rc++;
                    if (rc > numberOfColors)
                    {
                        rc = 0;
                    }
                        k = -1;
                }
            }
            this.shapeRenderer.color = new Color(GM.colorList[rc, 0], GM.colorList[rc, 1], GM.colorList[rc, 2]);
            GM.squareColors[shapeIndex] = rc;

            GM.lscIndex++;
            if (GM.lscIndex == 3)
            {
                for (int l = 1; l < 3; l++)
                {
                    GM.lastSquareCols[l - 1] = GM.lastSquareCols[l];
                }
                GM.lscIndex = 2;
            }
            GM.lastSquareCols[GM.lscIndex] = rc;

        }
    }

    public int ShapeIndex
    {
        get { return shapeIndex; }
        set { shapeIndex = value; }
    }


    // Update is called once per frame
    void Update()
    {
        
	}
}
