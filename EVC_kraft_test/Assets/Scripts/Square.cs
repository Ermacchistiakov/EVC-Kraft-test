using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

    public AudioSource mainAudio;
    BoxCollider2D squareBoxCollider;
    Rigidbody2D squareRigidBody;
    bool moused; // удерживается ли объект мышкой / сенсором
    bool exactCollision; // есть ли в данный момент соприкосновение с кругом такого же цвета
    Vector3 startMousePosition;
    GameObject squareObject;
    Vector3 squareStartPosition;
    public GameManager gameManager;
    public Shape thisShape;
    float tapTimer; // таймер для фиксации двойного нажатия

	// Use this for initialization
	void Start () 
    {
        this.thisShape = this.GetComponent<Shape>();
        this.squareBoxCollider = this.GetComponent<BoxCollider2D>();
        this.squareRigidBody = this.GetComponent<Rigidbody2D>();
        this.squareStartPosition = this.transform.position;
        mainAudio.Play();
	}

    void OnMouseDown()
    {
        tapTimer += 0.5f;
        if (gameManager.playingAllowed == true)
        {
            mainAudio.Play();
            moused = true;
            startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
       // Debug.Log(this.GetComponent<Shape>().ShapeIndex);
    }

    void OnMouseUp()
    {
        if (gameManager.playingAllowed == true)
        {
            if (exactCollision == false)
            {
                mainAudio.Play();
                moused = false;
                this.transform.position = squareStartPosition;
            }
            else
            {
                gameManager.GetComponent<AudioSource>().Play();
                Recreation();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Circle")
        {
            GameObject circleObject = other.gameObject;
            var circleColor = circleObject.GetComponent<SpriteRenderer>().color;
            if (this.GetComponent<SpriteRenderer>().color == circleColor)
            {
                exactCollision = true;
            }
            else
            {
                exactCollision = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Circle")
        {
            exactCollision = false;
        }
    }

    void Recreation()
    {
        gameManager.newSquare(squareStartPosition, thisShape.ShapeIndex);
        Destroy(gameObject);
    }

	
	// Update is called once per frame
    void Update()
    {
        if (moused == true)
        {
            squareRigidBody.transform.position += ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - startMousePosition));
            startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (gameManager.playingAllowed == false)
        {
            moused = false;
            this.transform.position = squareStartPosition;
        }

        if (tapTimer > 0f) tapTimer -= Time.deltaTime;
        if (tapTimer > 0.6f) 
        {
            for (int i = 0; i < 3; i++) // проверка на совпадение цвета текущего квадрата с кругами
            {
                if (gameManager.squareColors[this.GetComponent<Shape>().ShapeIndex] == gameManager.circleColors[i])
                {
                    break;
                }
                if (i == 2)
                {
                    tapTimer = 0f; gameManager.score--; Recreation(); // двойной тап - пересоздание квадрата
                    break;
                }
            }
        } 
    }
}
