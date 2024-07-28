using Common.Scripts;
using UnityEngine;

namespace Evolution.Scripts
{
    public class Evolution01Cell : Cell
    {
        protected new void OnEnable()
        {
            SetRigidbody2D(GetComponent<Rigidbody2D>());
            base.OnEnable();
        }
    }
}