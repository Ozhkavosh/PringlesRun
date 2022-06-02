
using UnityEngine;

public class PriceIndicator : MonoBehaviour
{
    [SerializeField]TMPro.TMP_Text textField;
    [SerializeField] float lifetime = 1f;
    [SerializeField] Animation anim;

    private float _timer;
    private int _price;
    private Transform cameraTransform;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        transform.LookAt(2*transform.position- cameraTransform.position);
        transform.position += new Vector3(0.5f, 0);

    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > lifetime) Destroy(gameObject);
        if(_timer > lifetime / 2)
        {
            textField.alpha = 1f- (_timer - lifetime/2f)/(lifetime / 2f);
        }
        if ((transform.position - cameraTransform.position).z < 0) Destroy(gameObject);
    }
    public void SetValue( int value)
    {
        _price = value;
        textField.text = value + "$";
        if (_price < 0)
        {
            textField.color = Color.red;
        }
        anim.Play();
    }
    public void AddValue( int value)
    {
        _price += value;
        lifetime += 1;
        textField.text = _price + "$";
        anim.Play();
    }

}
