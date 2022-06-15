using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Управляет счетом игрока и осуществляет сохранение данных
    /// </summary>
    class PlayerBalance
    {
        public Action<int> BalanceChanged = (a)=> { };
        private static string _dataKey = "playerBalance";
        private static PlayerBalance _instance;
        private int _balance;

        private PlayerBalance()
        {
            object data = DataManager.GetData(_dataKey);
            if (data == null) _balance = 0;
            else _balance = (int)data;
        }

        public static PlayerBalance GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PlayerBalance();
            }

            return _instance;
        }

        public void Add(int value)
        {
            _balance += value;
            BalanceChanged.Invoke(_balance);
        }

        public int Get() => _balance;

        public void Save()
        {
            DataManager.SaveData(_dataKey,_balance);
        }
    }
}
