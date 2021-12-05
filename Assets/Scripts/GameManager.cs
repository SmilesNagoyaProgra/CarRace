using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] private int             m_maxLapsCount  = 3;
    [SerializeField] private StartEndPoint   m_startEndPoint = default;
    [SerializeField] private TextMeshProUGUI m_timeText      = default;
    [SerializeField] private Ranking         m_ranking       = default;

    private List<CheckPoint> m_checkPoints;
    private int              m_currentIndexCount;
    private int              m_currentRapCount;
    private bool             m_isGamePlay;

    void Start() {
        m_isGamePlay        = true;
        m_currentRapCount   = 0;
        m_currentIndexCount = 0;
        m_checkPoints = new List<CheckPoint>();
        m_timeText.text     = "TimeRecord 00.00";
        var checkPointObjects = GameObject.FindGameObjectsWithTag("CheckPoint");
        foreach(var checkPointObject in checkPointObjects) {
            m_checkPoints.Add(checkPointObject.GetComponent<CheckPoint>());
        }
        StartCoroutine("StartTimeRecord");
    }

    IEnumerator StartTimeRecord() {
        float time = 0;
        while (m_isGamePlay) {
            time += Time.deltaTime;
            m_timeText.text = "TimeRecord " + time.ToString("F2");
            yield return null;
        }
        m_ranking.gameObject.SetActive(true);
        m_ranking.SetRanking(time);
    }

    public void AddCheckPoint(CheckPoint checkPoint) {
        if (m_checkPoints[checkPoint.Index].IsActive) {
            return;
        }
        m_checkPoints[checkPoint.Index].IsActive = true;
        m_currentIndexCount++;
        if (m_checkPoints.Count == m_currentIndexCount) {
            Debug.Log("周回クリア");
            m_startEndPoint.SetActive(true);
        }
    }

    public void Goal() {
        m_currentRapCount++;
        if (m_currentRapCount >= m_maxLapsCount) {
            Debug.Log("ゲームクリア");
            m_isGamePlay = false;
            StartCoroutine("FadeOut");
        } else {
            Debug.Log("現在ラップ数 : " + m_currentRapCount.ToString());
            m_currentIndexCount = 0;
            clearCheckPoint();
        }
    }

    public void clearCheckPoint() {
        foreach (var checkPoint in m_checkPoints) {
            checkPoint.IsActive = false;
        }
    }

    private IEnumerator FadeOut() {
        float count = 0.0f;
        while (true) {
            count += Time.deltaTime;
            if (5.0f < count) {
                break;
            }
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}
