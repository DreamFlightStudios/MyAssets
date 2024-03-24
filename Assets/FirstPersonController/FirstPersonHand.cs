using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _playerCollider;

    [SerializeField] private Transform _hand;
    [SerializeField] private LayerMask _pickUpLayer;
    [SerializeField] private int _dropForce;
    [SerializeField] private int _raycastDistance;
    
    private GameObject _pickUpObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) PickUp();
        if (Input.GetKeyDown(KeyCode.G)) Drop();
    }

    private void PickUp()
    {
        if (_pickUpObject != null) return;

        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward), out hit, _raycastDistance, _pickUpLayer))
        {
            _pickUpObject = hit.collider.gameObject;

            _pickUpObject.GetComponent<Rigidbody>().isKinematic = true;

            _pickUpObject.transform.position = _hand.position;
            _pickUpObject.transform.parent = _hand.transform;

            Physics.IgnoreCollision(_playerCollider, _pickUpObject.GetComponent<Collider>());
        }
    }

    private void Drop()
    {
        if (_pickUpObject != null)
        {
            _pickUpObject.GetComponent<Rigidbody>().isKinematic = false;
            _pickUpObject.GetComponent<Rigidbody>().AddForce(_hand.forward * _dropForce, ForceMode.Impulse);

            Physics.IgnoreCollision(_playerCollider, _pickUpObject.GetComponent<Collider>(), false);

            _pickUpObject.transform.parent = null;
            _pickUpObject = null;
        }
    }
}
