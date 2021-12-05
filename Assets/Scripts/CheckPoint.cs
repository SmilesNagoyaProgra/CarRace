using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
    [SerializeField] private int m_index = default;
    public int Index {
        get => m_index;
    }
    public bool IsActive { get; set; }
}
