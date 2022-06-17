using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class ScoreManager : MonoBehaviour
    {
        [SerializeField] private PriceIndicator _priceIndicator;
        [SerializeField] private TMPro.TMP_Text _playerScoreText;
        private const float MaxDistance = 2f;
        private static ScoreManager _instance;
        
        private PlayerBalance _playerBalance;
        private List<PriceIndicator> _indicators;
        private int _score  = 0;

        private void Awake()
        {
            if (_priceIndicator == null) Debug.LogWarning("Score manager's price indicator object is not set, displaying disabled", _instance);
            if (_playerScoreText == null) Debug.LogWarning("Score manager's score text field is not set, displaying disabled", _instance);
            _indicators = new List<PriceIndicator>(10);
            _playerBalance = PlayerBalance.GetInstance();
        }
        public void IndicateScoreChange(Vector3 position, int value, bool makeIndividual = false)
        {
            if (_playerScoreText)
            {
                _score += value;
                _playerScoreText.text = _score + "$";
            }
            if (_priceIndicator == null) return;

            var indicator = makeIndividual ? _priceIndicator.CreateCopyAt(position).GetComponent<PriceIndicator>() : GetClosest(position,Math.Sign(value));
            if (!_indicators.Contains(indicator))
            {
                _indicators.Add(indicator);
                indicator.LifetimeEnded += PriceIndicatorExpired;
                indicator.SetValue(value);
            }
            else
            {
                indicator.AddValue(value);
            }
        }

        public bool ChangePlayerBalance(int amount)
        {
            if (_playerBalance.Get() + amount < 0) return false;
            _playerBalance.Add(amount);
            return true;
        }

        private void OnDestroy()
        {
            _playerBalance?.Save();
        }
        private PriceIndicator GetClosest(Vector3 position,int sign = 1)
        {
            PriceIndicator closest = null;

            float minDistance = MaxDistance;
            foreach (var indicator in _indicators)
            {
                
                if (Math.Sign(indicator.GetValue()) != sign || !indicator.Alive()) continue;

                float dist = Vector3.Distance(position, indicator.transform.position);
                if (!(dist < minDistance)) continue;

                minDistance = dist;
                closest = indicator;
            }

            if (closest == null) closest = _priceIndicator.CreateCopyAt(position).GetComponent<PriceIndicator>();
            return closest;

        }
        private void PriceIndicatorExpired(PriceIndicator indicator)
        {
            _indicators.Remove(indicator);
            indicator.LifetimeEnded -= PriceIndicatorExpired;
            Destroy(indicator.gameObject);
        }
        public int GetScore() => _score;
    }
}
