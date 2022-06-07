using UnityEngine;

namespace Assets.Scripts
{
    public class MoveToCursor : MonoBehaviour
    {
        [SerializeField] private float _minLim, _maxLim;
        private Camera _cam;
        private Vector3 _mousePos;
        private bool _leftHold;
        private void Start()
        {
            if (_cam is null) _cam = Camera.main;
        }
        void Update()
        {
            _mousePos = Input.mousePosition;


            if (Input.GetMouseButtonDown(0)) _leftHold = true;
            else if (Input.GetMouseButtonUp(0)) _leftHold = false;

            if (_leftHold) MoveTo();
        }
        void MoveTo()
        {
            Vector3 pos = new Vector3(_mousePos.x,_mousePos.y,4);
            pos = _cam.ScreenToWorldPoint(pos);
            float xPos = Mathf.Clamp(pos.x, _minLim, _maxLim);
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }

    }
}
