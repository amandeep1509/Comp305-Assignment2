using UnityEngine;
using System.Collections;

// VELOCITY RANGE UTILITY Class +++++++++++++++++++++++
[System.Serializable]
public class VelocityRange
{
    // PUBLIC INSTANCE VARIABLES ++++
    public float minimum;
    public float maximum;

    // CONSTRUCTOR ++++++++++++++++++++++++++++++++++++
    public VelocityRange(float minimum, float maximum)
    {
        this.minimum = minimum;
        this.maximum = maximum;
    }
}

public class HeroController : MonoBehaviour
{
    // PRIVATE  INSTANCE VARIABLES
    private Animator _animator;
	private float _move;
	private float _jump;
	private bool _facingRight;
	private Transform _transform;
    private Rigidbody2D _rigidBody2D;
    private bool _isGrounded;

    //sounds
    private AudioSource[] _audioSources;
    private AudioSource _enemySound;
    private AudioSource _rewardSound;
    private AudioSource _winSound;

    // PUBLIC INSTANCE VARIABLES
    public VelocityRange velocityRange;
    public float moveForce;
    public float jumpForce;
    public Transform groundCheck;
    public Transform camera;
    public GameController gameController;

    // Use this for initialization
    void Start () {
       
        // Initialize Public Instance Variables
        this.velocityRange = new VelocityRange(300f, 30000f);

        this._transform = gameObject.GetComponent<Transform> ();
		this._animator = gameObject.GetComponent<Animator> ();
        this._rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        this._move = 0f;
		this._jump = 0f;
		this._facingRight = true;

        //initialising the sounds
        this._audioSources = gameObject.GetComponents<AudioSource>();
        this._enemySound = this._audioSources[1];
        this._rewardSound = this._audioSources[2];
        this._winSound = this._audioSources[3];

        // place the hero in the starting position
        this._spawn();
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        this._isGrounded = Physics2D.Linecast(
            this._transform.position,
            this.groundCheck.position,
            1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(this._transform.position, this.groundCheck.position);
        Vector3 currentPosition = new Vector3(this.transform.position.x, this.transform.position.y, -5);
        this.camera.position = currentPosition;


        float forceX = 0f;
        float forceY = 0f;

        // get absolute value of velocity for our gameObject
        float absVelX = Mathf.Abs(this._rigidBody2D.velocity.x);
        float absVelY = Mathf.Abs(this._rigidBody2D.velocity.y);

        // Ensure the player is grounded before any movement checks
        if (this._isGrounded)
        {
            
            // gets a number between -1 to 1 for both Horizontal and Vertical Axes
            this._move = Input.GetAxis("Horizontal");
            this._jump = Input.GetAxis("Vertical");

            if (this._move != 0)
            {
                if (this._move > 0)
                {
                    // movement force
                    if (absVelX < this.velocityRange.maximum)
                    {
                        forceX = this.moveForce;
                    }
                    this._facingRight = true;
                    this._flip();
                }
                if (this._move < 0)
                {
                    // movement force
                    if (absVelX < this.velocityRange.maximum)
                    {
                        forceX = -this.moveForce;
                    }
                    this._facingRight = false;
                    this._flip();
                }

                // call the walk clip
                this._animator.SetInteger("AnimState", 1);
            }
            else {

                // set default animation state to "idle"
                this._animator.SetInteger("AnimState", 0);
            }

            
            if (this._jump > 0)
            {
                // jump force
                if (absVelY < this.velocityRange.maximum)
                {
                    forceY = this.jumpForce;
                }

            }
            
        }
        else {
            // set the jump clip
            this._animator.SetInteger("AnimState", 2);
        }

        // Apply the forces to the player
        this._rigidBody2D.AddForce(new Vector2(forceX, forceY));
       
        if (this._transform.position.x < -50)
        {
            this._transform.position = new Vector3(-50, 53, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //collision with enemy or falling down
        if (other.gameObject.CompareTag("Death"))
        {
            this._enemySound.Play();
            this._spawn();
            this.gameController.LivesValue--;
            if (this.gameController.LivesValue <= 0)
                Destroy(gameObject);
        }

        //collect the crystals
        if(other.gameObject.CompareTag("crystal") ) {

            this._rewardSound.Play();
            Destroy(other.gameObject);
			this.gameController.ScoreValue += 10;
		}

        //collect the Big Diamond and win the game
        if (other.gameObject.CompareTag("Diamond"))
        {
            this._winSound.Play();
            this.gameController.Win = 1;
            Destroy(other.gameObject);
            this.gameController.ScoreValue += 100;
            Destroy(gameObject);
            
        }
        
       
    }


    // PRIVATE METHODS

    // Flip the player left or right
    private void _flip() {
		if (this._facingRight) {
			this._transform.localScale = new Vector2 (1, 1);
		} else {
			this._transform.localScale = new Vector2 (-1, 1);
		}
	}

    //spawn the player
    private void _spawn()
    {
        this._transform.position = new Vector3(-29f, 74f, 0);
    }
}

