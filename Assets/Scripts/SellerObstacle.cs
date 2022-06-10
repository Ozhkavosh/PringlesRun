using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class SellerObstacle: Obstacle
    {
        private void OnTriggerExit(Collider other)
        {
            Interact(other);
        }
    }
}
