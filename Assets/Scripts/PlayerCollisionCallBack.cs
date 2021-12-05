using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class PlayerCollisionCallBack : MonoBehaviour {
    private CarUserControl m_carUserControl = default;
    private void Start() {
        m_carUserControl = transform.parent.parent.GetComponent<CarUserControl>();
    }

    private void OnTriggerEnter(Collider target) {
        m_carUserControl.Collision(target, this);
    }
}