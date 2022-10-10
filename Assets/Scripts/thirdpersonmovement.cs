using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class thirdpersonmovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public bool isGrounded;
    public float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Vector3 startScale, shrunkScale, bigScale, scale1, scale2;

    public Material transparentMat, transparentMat2;

    public CinemachineFreeLook playerCam, smallCam, bigCam;

    public CinemachineFreeLook smallCamAiming, bigCamAiming;
    public CinemachineVirtualCamera playerCamAiming;
   // CinemachineBrain myBrain;
   // CinemachineBlendDefinition customBlend;
  //  public CinemachineFreeLook.Orbit[] m_Orbits;
    public bool isSmall, isBig;

    bool shouldChangeSize = false, growing, shrinking;
    float sizeLerpPos = 0;
    GameObject lerpObject;
    public float changeSpeed;
    public GameObject topChecker;

    float Timer;
    float xRotation = 0f, yRotation = 0f;
    public Sprite empty, quarter, half, threeQuarters, full;
    public Image chargeBar;

    public float sizeCharge = 100;
    float percent = 0;
    bool shouldReshrink = false;
    bool dontChangeSize = false;
    public Animator playerAnim;
    bool rightClickDown = false;
    bool isAiming = false;

    public Animator reticleAnimator;

    public GameObject shrinkShot;
    public GameObject shootingPosition;
    public GameObject spine;
    public float turnSpeed = 10f;
    Quaternion currentRotation;

    string currentPlayer, currentMouseX, currentMouseY, currentHorizontal, currentVertical, currentJump, currentFire1, currentFire2, currentShift, currentGrow, currentShrink, currentOpenHand;

    public bool isParented = false;

    public GameObject handPlatform;
    bool disableGrav = false;
    public GameObject UILeftPos, UIRightPos, currentReticle;
    // Start is called before the first frame update

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        startScale = transform.localScale;
        shrunkScale = new Vector3(startScale.x * 0.1f, startScale.y * 0.1f, startScale.z * 0.1f);
        bigScale = new Vector3(startScale.x * 10f, startScale.y * 10f, startScale.z * 10f);
        //  myBrain.m_DefaultBlend = customBlend;
        isSmall = false;
        isBig = false;
        if(this.name == "Player")
        {
            currentPlayer = "Player";
            currentMouseX = "Mouse X";
            currentMouseY = "Mouse Y";
            currentHorizontal = "Horizontal";
            currentVertical = "Vertical";
            currentJump = "Jump";
            currentFire1 = "Fire1";
            currentFire2 = "Fire2";
            currentShift = "Shift";
            currentShrink = "Shrink";
            currentGrow = "Grow";
            currentOpenHand = "Open Hand";
        }
        if(this.name == "Player2")
        {
            currentPlayer = "Player2";
            currentMouseX = "Controller X";
            currentMouseY = "Controller Y";
            currentHorizontal = "Horizontal Controller";
            currentVertical = "Vertical Controller";
            currentJump = "Jump Controller";
            currentFire1 = "Fire1 Controller";
            currentFire2 = "Fire2 Controller";
            currentShift = "Shift Controller";
            currentShrink = "Shrink Controller";
            currentGrow = "Grow Controller";
            currentOpenHand = "Open Hand Controller";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer == "Player2")
        {
            Debug.Log(Input.GetAxisRaw(currentHorizontal) + " " + Input.GetAxisRaw(currentVertical));
        }

        currentReticle.transform.position = new Vector3(UILeftPos.transform.position.x + ((UIRightPos.transform.position.x - UILeftPos.transform.position.x) / 2), currentReticle.transform.position.y, currentReticle.transform.position.z);

    //    currentReticle.transform.position.x = UILeftPos.transform.position.x + ((UIRightPos.transform.position.x - UILeftPos.transform.position.x)/2);
       // playerCam.Orbit;
        float horizontal = Input.GetAxisRaw(currentHorizontal);
        float vertical = Input.GetAxisRaw(currentVertical);
        if(horizontal != 0 || vertical != 0)
        {
            playerAnim.SetBool("isWalking", true);
        }
        else
        {
            playerAnim.SetBool("isWalking", false);
        }
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            //-2 instead of 0 is to make transition to ground smoother and more consistent
            velocity.y = -2f;
        }
        if (Input.GetButtonDown(currentJump) && isGrounded)
        {
            //equation for needed jump height is the square root of (the jump height * -2 * gravity)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if(Input.GetButtonDown(currentJump) && disableGrav == true)
        {
            disableGrav = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            GameObject.Find("Main Camera").GetComponent<cameraSplit>().moveSplitCam(0);
            this.transform.parent = null;
        }

        if (Input.GetButtonDown(currentShift))
        {
            rightClickDown = true;
            playerAnim.SetTrigger("holdButton");
        }
        if (Input.GetButtonUp(currentShift))
        {
            rightClickDown = false;
            playerAnim.SetTrigger("letGoButton");
        }

        if (Input.GetButtonDown(currentFire2))
        {
            if(isAiming == false && isSmall == false && isBig == false)
            {
                currentRotation = GameObject.Find(currentPlayer).transform.localRotation;
                reticleAnimator.SetTrigger("open");
                playerAnim.SetTrigger("startAim");
                playerCam.gameObject.SetActive(false);
                playerCamAiming.gameObject.SetActive(true);
                isAiming = true;
            }

        }
        if (isAiming)
        {
            var rotInput = new Vector2(Input.GetAxis(currentMouseX), Input.GetAxis(currentMouseY));
            var rot = transform.eulerAngles;
            rot.y += rotInput.x * turnSpeed;
            transform.rotation = Quaternion.Euler(rot);

            rot = GameObject.Find(currentPlayer).transform.localRotation.eulerAngles;
            rot.x -= rotInput.y * turnSpeed;
            if (rot.x > 180)
                rot.x -= 360;
            rot.x = Mathf.Clamp(rot.x, -90, 90);
            GameObject.Find(currentPlayer).transform.localRotation = Quaternion.Euler(rot);
           // Debug.DrawRay(shootingPosition.transform.position, shootingPosition.transform.TransformDirection(Vector3.forward) * 5.6f, Color.yellow, 10f, false);

            //   Debug.Log("local: " + this.transform.localRotation.y * Mathf.Rad2Deg);

        }
        if (Input.GetButtonDown(currentFire1) && isAiming == true)
        {
            GameObject shot = Instantiate(shrinkShot, shootingPosition.transform.position, Quaternion.identity);
            shot.name = "shot" + currentPlayer;
        }

        if (Input.GetButtonUp(currentFire2) && isSmall == false && isBig == false)
        {
            if (isAiming == true)
            {
                reticleAnimator.SetTrigger("close");
                playerAnim.SetTrigger("endAim");
                playerCamAiming.gameObject.SetActive(false);
                playerCam.gameObject.SetActive(true);
                isAiming = false;
            }

        }

        if(Input.GetButtonDown(currentOpenHand) && isBig == true)
        {
            playerAnim.SetTrigger("openHand");
            handPlatform.GetComponent<BoxCollider>().enabled = true;
        }
        if(Input.GetButtonUp(currentOpenHand) && isBig == true)
        {
            playerAnim.SetTrigger("closeHand");
            handPlatform.GetComponent<BoxCollider>().enabled = false;
        }

        if (Input.GetButtonDown(currentShrink) && rightClickDown && sizeCharge == 100 && isBig == false)
        {
            if (isSmall == true)
            {
                smallCam.gameObject.SetActive(false);
                playerCam.gameObject.SetActive(true);

                controller.skinWidth = 0.08f;
                speed = 10f;
                changeSize(this.gameObject, shrunkScale, startScale, 0);

            }

            else if (isSmall == false)
            {
                smallCam.gameObject.SetActive(true);
                playerCam.gameObject.SetActive(false);

                controller.skinWidth = 0.01f;
                speed = 2f;
                changeSize(this.gameObject, startScale, shrunkScale, 0);

            }

        }

        if (Input.GetButtonDown(currentGrow) && rightClickDown && sizeCharge == 100 && isSmall == false)
        {
            if(isBig == true)
            {
                bigCam.gameObject.SetActive(false);
                playerCam.gameObject.SetActive(true);
                changeSize(this.gameObject, bigScale, startScale, 1);
                controller.skinWidth = 0.08f;
            }
            else if (isBig == false)
            {
                bigCam.gameObject.SetActive(true);
                playerCam.gameObject.SetActive(false);
                changeSize(this.gameObject, startScale, bigScale, 1);
                controller.skinWidth = 0.9f;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        if(disableGrav == false)
        {
            controller.Move(velocity * Time.deltaTime);
        }
        

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized   * speed * Time.deltaTime);
        }

        if(sizeCharge < 100)
        {
            sizeCharge = sizeCharge + (50f * Time.deltaTime);
        }
        if(sizeCharge > 100)
        {
            sizeCharge = 100;
        }

        if (sizeCharge >= 0 && sizeCharge < 25)
        {
            chargeBar.sprite = empty;
        }
        else if(sizeCharge >= 25 && sizeCharge < 50)
        {
            chargeBar.sprite = quarter;
        }
        else if(sizeCharge >= 50 && sizeCharge < 75)
        {
            chargeBar.sprite = half;
        }
        else if(sizeCharge >= 75 && sizeCharge < 100)
        {
            chargeBar.sprite = threeQuarters;
        }
        else if(sizeCharge == 100)
        {
            chargeBar.sprite = full;
        }

        if(shouldChangeSize == true)
        {
            if(sizeLerpPos == 0)
            {
                sizeLerpPos = 0.01f;
                Timer = 0.15f;
            }
            if(sizeLerpPos < 1)
            {
                sizeCharge = 100-(sizeLerpPos * 100);
                lerpObject.transform.localScale = Vector3.Lerp(scale1, scale2, sizeLerpPos);

                if(isSmall == false && isGrounded && shrinking && dontChangeSize == false)
                {
                    lerpObject.transform.position = new Vector3(lerpObject.transform.position.x, lerpObject.transform.position.y - 0.15f, lerpObject.transform.position.z);
                }

                if(isSmall && shrinking && isGrounded)
                {
                    lerpObject.transform.position = new Vector3(lerpObject.transform.position.x, lerpObject.transform.position.y + 0.15f, lerpObject.transform.position.z);
                }

                if(isBig && growing)
                {
                   // Debug.Log("11");
                    lerpObject.transform.position = new Vector3(lerpObject.transform.position.x, lerpObject.transform.position.y - 1.6f, lerpObject.transform.position.z);
                }

                if (isBig == false && growing && isGrounded)
                {
                   // Debug.Log("22");
                    lerpObject.transform.position = new Vector3(lerpObject.transform.position.x, lerpObject.transform.position.y + 1.8f, lerpObject.transform.position.z);
                }

                if (sizeLerpPos > Timer)
                {
                    GameObject duplicate = Instantiate(this.gameObject);
                    duplicate.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                    duplicate.GetComponent<thirdpersonmovement>().enabled = false;
                    duplicate.GetComponent<CharacterController>().enabled = false;
                    duplicate.GetComponent<growShrinkEffect>().enabled = true;
                    duplicate.transform.parent = null;
                    if ((scale2 == startScale || scale2 == bigScale) && Timer < 0.75f)
                    {
                        duplicate.transform.localScale = new Vector3(duplicate.transform.localScale.x + .3f, duplicate.transform.localScale.y + .3f, duplicate.transform.localScale.z + .3f);
                    }
                    // duplicate.transform.GetChild(0).GetComponent<MeshRenderer>().material = transparentMat;
                    duplicate.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material = transparentMat;
                    Destroy(duplicate, 0.2f);

                    Timer = Timer + 0.15f;
                }

                Timer = Timer + (Time.deltaTime);
                sizeLerpPos = sizeLerpPos + (changeSpeed * Time.deltaTime);
            }
            
            if(shouldReshrink == true)
            {
                if(sizeLerpPos > percent)
                {
                    shouldReshrink = false;
                    sizeLerpPos = 0;

                    var tempScale = scale1;
                    scale1 = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
                    scale2 = tempScale;
                    isSmall = !isSmall;

                }
            }
            else if(shouldReshrink == false)
            {
                if (sizeLerpPos > 1)
                {
                    percent = 0;
                    if (growing)
                    {
                        isBig = !isBig;
                        growing = false;
                    }
                    else if (shrinking)
                    {
                        isSmall = !isSmall;
                        shrinking = false;
                    }

                    sizeLerpPos = 0;
                    shouldChangeSize = false;
                    dontChangeSize = false;
                    lerpObject.transform.localScale = scale2;
                }
            }
            
        }
    }


    void changeSize(GameObject changeObject, Vector3 startSize, Vector3 endSize, int changeDirection)
    {
        shouldChangeSize = true;
        lerpObject = changeObject;
        scale1 = startSize;
        scale2 = endSize;
        if(changeDirection == 1)
        {
            //bigman to regularman
            growing = true;
        //    changeSpeed = 9;
        }
        if(changeDirection == 0)
        {
            //antman to regularman
            shrinking = true;
           // changeSpeed = 9;
        }

        RaycastHit hit;

        if (Physics.Raycast(topChecker.transform.position, topChecker.transform.TransformDirection(Vector3.up), out hit, 1.6f))
        {
        //    Debug.Log(hit.transform.name);
         //   Debug.Log(Vector3.Distance(topChecker.transform.position, hit.point));
           // Debug.DrawRay(topChecker.transform.position, topChecker.transform.TransformDirection(Vector3.up) * 1.6f, Color.yellow, 10f, false);
            if(percent == 0)
            {
                percent = Vector3.Distance(topChecker.transform.position, hit.point) / 1.7f;
                shouldReshrink = true;
                playerAnim.SetTrigger("hithead");
                playerAnim.ResetTrigger("holdButton");
                playerAnim.ResetTrigger("letGoButton");

                //0.08 0.01
                if (shrinking)
                {
                    if (isSmall == true)
                    {
                        dontChangeSize = true;
                        smallCam.gameObject.SetActive(true);
                        playerCam.gameObject.SetActive(false);

                        controller.skinWidth = 0.01f;
                        speed = 2f;
                    }
                }
            }

        }

    }

    void OnControllerColliderHit(ControllerColliderHit col)
    {
        if(isParented == false && col.gameObject.name == "handCube" && this.gameObject.name == "Player")
        {
            GameObject.Find("Main Camera").GetComponent<cameraSplit>().moveSplitCam(-0.75f);
            this.transform.parent = col.transform.parent.gameObject.transform;
            disableGrav = true;
        }
    }

}
