using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts
{
    public class BalanceUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _textField;
        private void Awake()
        {
            PlayerBalance.GetInstance().BalanceChanged += SetValue;
            Refresh();
        }
        public void Refresh()
        {
            _textField.text = PlayerBalance.GetInstance().Get()+"$";
        }

        public void SetValue(int value)
        {
            _textField.text = value + "$";
        }
    }
}

