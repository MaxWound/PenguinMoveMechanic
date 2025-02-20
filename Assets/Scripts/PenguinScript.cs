using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinScript : MonoBehaviour
{
    
    [SerializeField]
    AudioSource HitByYetiSound;
    [SerializeField]
    AudioSource BirdSound;
    [SerializeField]
    AudioSource BumpSound;
    public bool inSimulation;
    public static PenguinScript penguinScript;
    [SerializeField, Range(1f, 3f)]
    float BumpForce;
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    Transform floorTransform;
    [SerializeField]
    Object BirdObj;
    [SerializeField]
    GameObject massGo;
    AngleScript angleInstance;
    public bool AaPVisible;
    Transform penguinTransform;
    [SerializeField]
    Transform squareTransform;
    public Rigidbody2D rb;
    public float startXpos;
    [SerializeField]
    GameObject PenguinSprite;
    public bool Grounded;
    private bool ExittedGround;
    public bool rotSet;
    private bool StartHitted;
    public SpriteRenderer[] allSpriteRenderer;
    private void Awake()
    {
       
        allSpriteRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>();
        penguinScript = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        startXpos = gameObject.transform.position.x;
        rotSet = false;
        
        penguinTransform = gameObject.transform;
        angleInstance = AngleScript.angleInstance;
        
        rb.freezeRotation = true;
        rb.isKinematic = true;
    }
    private void FixedUpdate()
    {
        print($"Grounded = {Grounded}");

        if (penguinTransform.position.y - floorTransform.position.y >= 6f)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.black;
        }

        if (Grounded != true)
        {
            Rotate();
        }
        if (rb.velocity == new Vector2(0f, 0f))
        {
            //����� Grounded
            if (StartHitted == true && AaPVisible != true && inSimulation != true)
            {
                if (GameManager.gameManager.GameOver != true)
                {
                    AngleAndPower.angleAndPowerInstance.SetAngleAndPowerVisible();
                    AaPVisible = true;
                }
                else
                {
                    GameManager.gameManager.ShowRestartWindow();
                }
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Grounded != true)
        {

            if (other.tag == "Ground")
            {
               
                Grounded = true;
                SetAngle();
                if (rb.velocity.y <= -5)
                {
                    Bump();
                }
            }
            if(other.tag == "Sky")
            {
                Instantiate(BirdObj, penguinTransform.position, Quaternion.identity);
                BirdSound.Play();
            }
        }

        if (other.tag == "Snake")
        {
            other.GetComponent<SnakeScript>().HitSnake();
        }
        if(other.tag == "Giraffe")
        {
            other.GetComponent<GiraffeScript>().SpawnAndThrow();
        }
        if(other.tag == "Bird")
        {
            other.GetComponent<BirdScript>().HoldAndThrow();
        }


    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground" && Grounded != true)
        Grounded = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            rotSet = false;
            Grounded = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Ground")
        {
            BumpSound.Play();
        }
    }

    

    public void hitPenguin(float angle, float power, bool IsHitByYeti)
    {
        if (IsHitByYeti == true)
        {
            if (GameManager.gameManager.TriesCount != 0)
            {
                
                if (StartHitted != true)
                {
                    StartHitted = true;
                }
                HitByYetiSound.Play();
                print($"{power}");
                rotSet = false;
                rb.isKinematic = false;
                Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
                rb.AddForce(dir * power, ForceMode2D.Impulse);

            }
        }
        else
        {
            if (StartHitted != true)
            {
                StartHitted = true;
            }
            print($"{power}");
            rotSet = false;
            rb.isKinematic = false;
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            rb.AddForce(dir * power, ForceMode2D.Impulse);
        }
    }
    private void SetGrounded(bool _bool)
    {
        Grounded = _bool;
    }
    void Bump()
    {
        
        rb.AddForce(Vector2.up * (rb.velocity.y * -1) * BumpForce, ForceMode2D.Impulse);

    }
    void SetAngle()
    {
        if (Grounded == true)
        {
            if (rotSet == false)
            {
                //penguinTransform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                print("rot");
                //squareTransform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                rb.SetRotation(90f);
                rotSet = true;
                Grounded = true;
                BumpSound.Play();
            }

            //penguinTransform.localEulerAngles = new Vector3(0f, 0f, -90f);
        }

    }
    void Rotate()
    {
        rotSet = false;
        var direction = rb.velocity;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        //penguinTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        print("r");
        rb.SetRotation(angle);

    }
    IEnumerator SetGroundedTrueForSeconds(float seconds)
    {
        Grounded = true;
        yield return new WaitForSeconds(seconds);
        if (ExittedGround == true)
        {
            Grounded = false;
        }
        else
            Grounded = true;
    }
    public void SetVisible(bool _bool)
    {
        for (int i = 0; i < allSpriteRenderer.Length; i++)
        {
            allSpriteRenderer[i].enabled = _bool;
        }
    }

}
