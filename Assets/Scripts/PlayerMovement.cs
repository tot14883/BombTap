using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	public Animator animator;
	public RagdollCharacter ragdollCharacter;

	[Header("Movement")]
	public float speed = 1f;
	public float gravity = -9.10f;
	public float jumpHeight = 1f;
	public float rotationSpeed = 25;

	[Header("Ground Check")]
	public Transform ground_check;
	public float ground_distance = 0.4f;
	public LayerMask ground_mask;

	[Header("Camera")]
	[SerializeField] CinemachineVirtualCamera thirdPersonCam;
	[SerializeField] CinemachineVirtualCamera firstPersonCam;

	[Header("Mouse Look")]
	public Vector2 turn;
	public float mouseSensitivity = 0.5f;
	public Vector3 deltaMove;
	public GameObject player;

	Vector3 targetDirection;
	Vector3 move;
	Vector3 velocity;
	bool isGrounded;

	void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
		isGrounded = Physics.CheckSphere(ground_check.position, ground_distance, ground_mask);

		if(isGrounded && velocity.y < 0)
        {
			velocity.y = -2f;
        }

		if (!ragdollCharacter.isActiveRagdoll())
		{
			float x = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");
			turn.x += Input.GetAxis("Mouse X") * mouseSensitivity;
			turn.y += Input.GetAxis("Mouse Y") * mouseSensitivity;

			turn.y = Mathf.Clamp(turn.y, -45f, 70f);
			firstPersonCam.transform.localRotation = Quaternion.Euler(-turn.y, 0, 0);
			player.transform.localRotation = Quaternion.Euler(0, turn.x, 0);

			animator.SetFloat("forward", z);
			animator.SetFloat("strafe", x);

			move = controller.transform.forward * z + controller.transform.right * x;
			controller.transform.Rotate(Vector3.up * x * (100f * Time.deltaTime));
			controller.Move(move * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.LeftShift))
        {
			speed = 3f;
			animator.SetBool("run", true);
        }
		else
        {
			animator.SetBool("run", false);
        }


		if (Input.GetButtonDown("Jump") && isGrounded)
        {
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
			animator.SetBool("jump", true);
			StartCoroutine(WaitForJumping(2f));
        }
        else
        {
			animator.SetBool("jump", false);
		}

		if (ragdollCharacter.isActiveRagdoll())
		{
			CameraSwitcher.SwitchCamera(thirdPersonCam);
		}
		else
		{
			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);
		}
	}

	IEnumerator WaitForJumping(float waitTime)
    {
		yield return new WaitForSeconds(waitTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
		if (hit.gameObject.tag == "Brick")
		{
			Debug.Log("aaa");
			Brick brick = hit.gameObject.GetComponent<Brick>();
			if (brick != null)
			{
				brick.ShowSecret();
			}
		}
	}

    private void DetectMine()
	{
		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
		if (Physics.SphereCast(ray, 0.2f, out hit))
		{
			GameObject hitObject = hit.transform.gameObject;
			Brick brick = hitObject.GetComponent<Brick>();
			if (brick != null)
			{
				brick.ShowSecret();
			}
		}
	}

    private void OnEnable()
    {
		CameraSwitcher.Register(firstPersonCam);
		CameraSwitcher.Register(thirdPersonCam);
		CameraSwitcher.SwitchCamera(firstPersonCam);
    }

    private void OnDisable()
    {
		CameraSwitcher.Unregister(thirdPersonCam);
		CameraSwitcher.Unregister(firstPersonCam);
    }
}
