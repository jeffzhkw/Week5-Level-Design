using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    public GameObject _lightRoot;
    public Slider _slider;
    public Slider _powerSlider;
    public Text _sliderText;
    public GameObject _winRoot, _loseRoot;
    public Button _btnWinRestart, _btnLoseRestart;
    public Animator animator;

    public float speed;
    public float jumpForce;

    private Vector2 movement;
    private Rigidbody2D playerRb;
    private BoxCollider2D playerBox;
    private bool isGrounded;

    public int _currentHp = 100;
    public int MaxHp = 100;
    public int _lockLightSubHp = 2;
    public int _unLockLightAddHp = 1;
    public float _lockLightTickTime = 1;
    public float _unLockLightTickTime = 2;


    public int _powerAdd = 1;
    public int _powerSub = 1;
    public float _powerTickTime = 1;
    float _powerUpdateTime = 0;

    public int MaxPower = 100;
    public int CurrentPower = 100;

    public class BoolEvent : UnityEvent<bool> { }
	public BoolEvent OnCrouchEvent;


    List<EnemyItem> _allEnemyItems = new List<EnemyItem>();
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerBox = GetComponent<BoxCollider2D>();

        _allEnemyItems.AddRange(GameObject.FindObjectsOfType<EnemyItem>());

        _btnWinRestart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
        _btnLoseRestart.onClick.AddListener(() =>
        {
            var sName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sName);
        });
    }

    bool _checkJump;
    float _tickTime = 0;

    bool _canController = true;
    bool _isFlashLight = true;
    float _lightTickTime = 0;
    void Update()
    {
        if (_canController == false) { return; }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CurrentPower > 0)
            {
                _isFlashLight = !_isFlashLight;
                _lightTickTime = 0;
                _lightRoot.SetActive(_isFlashLight);
            }
        }
        if (_isFlashLight)
        {
            _lightTickTime += Time.deltaTime;
            if (_lightTickTime >= _unLockLightTickTime)
            {
                _lightTickTime = 0;
                _currentHp += _unLockLightAddHp;

                CheckResult();
            }
        }
        else
        {
            _lightTickTime += Time.deltaTime;
            if (_lightTickTime >= _lockLightTickTime)
            {
                _lightTickTime = 0;
                _currentHp -= _lockLightSubHp;
                CheckResult();
            }
        }
        if (_checkJump)
        {
            _tickTime += Time.deltaTime;
            if (_tickTime >= 1.5f)
            {
                if (isGrounded == false)
                {
                    isGrounded = true;
                    _checkJump = false;
                    _tickTime = 0;
                }
            }
        }
        //Power Check
        if (_isFlashLight)
        {
            _powerUpdateTime += Time.deltaTime;
            if (_powerUpdateTime >= _powerTickTime)
            {
                _powerUpdateTime = 0;
                CurrentPower -= _powerSub;
            }
        }
        else
        {
            _powerUpdateTime += Time.deltaTime;
            if (_powerUpdateTime >= _powerTickTime)
            {
                _powerUpdateTime = 0;
                CurrentPower += _powerAdd;
            }
        }
        CheckPower();

        movement.x = Input.GetAxisRaw("Horizontal");
        Vector3 playerScale = transform.localScale;
        if(movement.x < 0)
        {
            playerScale.x = -2;
        }
        if (movement.x > 0)
        {
            playerScale.x = 2;
        }
        transform.localScale = playerScale;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
                _checkJump = true;
                _tickTime = 0;
                animator.SetBool("isJumping", true);
            }
        }
        //transform.Translate(movement.x * speed * Time.deltaTime, 0, 0);
        playerRb.velocity = new Vector2(movement.x * speed, playerRb.velocity.y);
        playerRb.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < _allEnemyItems.Count; i++)
            {
                var item = _allEnemyItems[i];
                if (item.IsDead == false && Mathf.Abs(Vector2.Distance(item.transform.localPosition, transform.localPosition)) <= 3)
                {
                    item.SetVacuum(false);
                    item.IsDead = true;
                    item.MoveToPlayer(transform);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Pressed");
            Crouch();
        }

        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Debug.Log("Up");
            Stand();
        }




        // Animation
        animator.SetFloat("speed", Mathf.Abs(movement.x));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check for ground collision.
        isGrounded = true;
        _checkJump = false;

        if (collision.gameObject.tag == "ghost")
        {
            SetDead();
        }
        else if (collision.gameObject.tag == "door")
        {
            NextLevel(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    void CheckResult()
    {
        if (_currentHp > MaxHp)
        {
            _currentHp = MaxHp;
        }
        else if (_currentHp < 0)
        {
            _currentHp = 0;
            SetDead();
        }
        _slider.value = _currentHp / 1.0f / MaxHp;
        _sliderText.text = _currentHp.ToString();
    }

    void SetDead()
    {
        _canController = false;
        SfxManager.sfxInstance.Audio.PlayOneShot(SfxManager.sfxInstance.Die);
        _loseRoot.SetActive(true);
    }

    void NextLevel(bool isWin)
    {
        _canController = false;
        if (isWin)
        {
            _winRoot.SetActive(true);
            var idx = SceneManager.GetActiveScene().buildIndex;
            if (idx == 0)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                //SceneManager.LoadScene(0);
            }
        }
        else
        {
            _loseRoot.SetActive(true);
        }
    }
    void CheckPower()
    {
        if (CurrentPower > MaxPower)
        {
            CurrentPower = MaxPower;
        }
        else if (CurrentPower < 0)
        {
            CurrentPower = 0;

            if (_isFlashLight)
            {
                _isFlashLight = !_isFlashLight;
                _lightRoot.SetActive(_isFlashLight);
            }
        }
        _powerSlider.value = CurrentPower / 1.0f / MaxPower;
    }

    private void Crouch()
    {
        playerBox.size = new Vector2 (playerBox.size.x, 0.48f);
        //playerBox.offset = new Vector2(playerBox.offset.x, -0.12f);
        animator.SetBool("isCrouching", true);
        speed = speed * 0.50f;
    }

    private void Stand()
    {
        playerBox.size = new Vector2(playerBox.size.x, 0.74f);
        //playerBox.offset = new Vector2(playerBox.offset.x, -0.1f);
        animator.SetBool("isCrouching", false);
        speed = speed * 2;
    }
}
