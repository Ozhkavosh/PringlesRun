using UnityEngine;
using System.Collections.Generic;
namespace Assets.Scripts
{
    public class FinishTrigger : MonoBehaviour
    {
        [SerializeField] private Market[] _markets;
        [SerializeField] private GameObject _activateOnTrigger;
        [SerializeField] private Animator cameraAnimator;
        private int _sellCapacity;
        private Player _player;
        float _sellDelay = 0.1f;
        private void Awake()
        {
            foreach (var market in _markets)
            {
                _sellCapacity += market.Capacity;
            }
        }
        private void OnTriggerEnter(Collider other)
        {

            var player = other.GetComponent<Player>();
            if (player is null) return;
            player.Stop();
            cameraAnimator.SetTrigger("GoUp");
            _activateOnTrigger.SetActive(true);
            _player = player;

        }
        public void SellItems()
        {
            if (_player is null) return;
            List<Stackable> listToSell = _player.GetMostValuable(_sellCapacity);
            
            StartCoroutine(SellItemsWithDelays(listToSell));
            StartCoroutine(OnSellingFinished(listToSell.Count * _sellDelay + 1f));
        }
        private System.Collections.IEnumerator SellItemsWithDelays(List<Stackable> list)
        {
            for (int i = 0; i < _markets.Length; i++)
            {
                for (int j = 0; j < _markets[i].Capacity; j++)
                {

                    if (list.Count == 0) yield break;
                    var item = list[0];

                    if (!_markets[i].TrySell(item)) break;

                    _player.Holder.RemoveFromStack(item, false);
                    item.DisableStacking();
                    list.Remove(item);
                    yield return new WaitForSeconds(_sellDelay);
                }

            }
        }
        private System.Collections.IEnumerator OnSellingFinished(float delay)
        {
            yield return new WaitForSeconds(delay);

            _activateOnTrigger.SetActive(false);
            cameraAnimator.SetTrigger("GoDown");
        }
    }
}
