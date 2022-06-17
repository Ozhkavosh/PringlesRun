using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private GameObject _scoreObject;
    [SerializeField] private Transform _barTransform;
    [SerializeField] private float _placingOffset;
    [SerializeField] private float _scorePerMark;
    [SerializeField] private float _showDuration = 1f;
    [SerializeField] private AnimationCurve _motionCurve;
    private Transform _playerTransform;
    private float _showTimeElapsed;
    private bool _showEnabled;
    private Vector3 _destinationPosition;
    private Vector3 _currentPosition;
    private Vector3 _lastPlacedPosition;
    private Vector3 _startPosition;
    private Vector3 _velocity = Vector3.zero;
    private void Update()
    {
        if (!_showEnabled) return;
        _showTimeElapsed += Time.deltaTime;
        if (Vector3.Distance(_currentPosition, _lastPlacedPosition) > _placingOffset)
        {
            Instantiate(_scoreObject, _barTransform).transform.position = _currentPosition;
            _lastPlacedPosition = _currentPosition;
        }
        float t = _showTimeElapsed / _showDuration;
        Vector3 newPos = Vector3.Lerp(_startPosition, _destinationPosition, _motionCurve.Evaluate(t));
        _playerTransform.position += newPos - _currentPosition;

        _currentPosition = newPos;
        if (_showTimeElapsed >= _showDuration)
        {
            Debug.Log("Score bar show end.", this);
            _showEnabled = false;
        }
    }
    public void Show(int score)
    {
        _showEnabled = true;
        _startPosition = _scoreObject.transform.position;
        _currentPosition = _startPosition;
        _showTimeElapsed = 0;
        _destinationPosition = _currentPosition;
        _destinationPosition.y = (score / _scorePerMark) * _placingOffset;
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null) return;
        player.SetMove(false);
        Debug.Log("Score: "+player.GetScore());
        _playerTransform = player.transform;
        Show(player.GetScore());
    }

}
