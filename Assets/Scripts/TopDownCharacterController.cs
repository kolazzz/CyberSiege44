using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector2 moveInput;
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private LayerMask damagelayerMask;
    private Camera _mainCamera;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        Shoot();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector2 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion bulletrotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Instantiate(bulletprefab, firepoint.position, bulletrotation);
            Debug.Log("Shoot");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskUtil.ContainsLayer(damagelayerMask, collision.gameObject))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}

