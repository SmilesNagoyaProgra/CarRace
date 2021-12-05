using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour {
    [SerializeField] private GameObject m_scoreObject   = default;
    [SerializeField] private Transform  m_rootTransform = default;

    private List<float>       m_rankingRecords;
    private List<ScoreObject> m_scoreObjects;
    private int               m_rankingCount;

    private const string RANKING_TITLE_TEXT = "ranking";
    private const string RANKING_COUNT      = "rankingCount";

    // 開始処理
    void Start() {
        m_rankingRecords = new List<float>();
        m_scoreObjects   = new List<ScoreObject>();
        m_rankingCount   = PlayerPrefs.GetInt(RANKING_COUNT,0);
        getRanking();
        gameObject.SetActive(false);
    }

    // ランキング名称
    private string rankingName(int i) {
        return RANKING_TITLE_TEXT + "_" + (i + 1).ToString();
    }

    // ランキングレコード表示処理
    private void showRecord(int rankNo,float record) {
        var scoreObject = Instantiate(m_scoreObject, m_rootTransform);
        var score = scoreObject.GetComponent<ScoreObject>();
        score.SetText((rankNo+1).ToString() + "Rank" + record.ToString("F2"));
        m_scoreObjects.Add(score);
    }


    // ランキング読み込み
    void getRanking() {
        for (int i = 0; i < m_rankingCount; i++) {
            float data = PlayerPrefs.GetFloat(rankingName(i));
            m_rankingRecords.Add(data);
            showRecord(i, data);
        }
    }

    // ランキング書き込み
    public void SetRanking(float value) {
        m_rankingRecords.Add(value);
        m_rankingRecords.Sort();
        foreach (var scoreObject in m_scoreObjects) {
            Destroy(scoreObject.gameObject);
        }
        for (int i = 0; i < m_rankingRecords.Count; i++) {
            PlayerPrefs.SetFloat(rankingName(i), m_rankingRecords[i]);
            showRecord(i, m_rankingRecords[i]);
        }
        m_rankingCount++;
        PlayerPrefs.SetInt(RANKING_COUNT, m_rankingCount);
    }
}
