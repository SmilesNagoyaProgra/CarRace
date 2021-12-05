using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreObject : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI m_text = default;
    public void SetText(string text) { 
        m_text.text = text;
    }
}
