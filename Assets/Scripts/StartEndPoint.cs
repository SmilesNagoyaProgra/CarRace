using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndPoint : MonoBehaviour {
    [SerializeField] private Renderer m_renderer;
    [SerializeField] private Collider m_collider;

    private void Start() {
        m_renderer.enabled = false;
        m_collider.enabled = false;
    }

    public void SetActive(bool isActive) {
        m_renderer.enabled = isActive;
        m_collider.enabled = isActive;
    }
}
