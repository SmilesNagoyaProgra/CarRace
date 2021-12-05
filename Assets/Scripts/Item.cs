using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public enum ItemModes {
    NONE = 0,
    SPEED_UP,  // 通常一回だけ速度が上がるItem
    SPEED_3UP, // 通常三回だけ速度が上がるItem
    COIN,      // Coinを設置
    INVISIBLE, // 障害物を一定時間避ける
    MAX,
};

// 親
public class Item : MonoBehaviour {
    private ItemModes m_itemModes;

    // プロパティ(アイテムのgetter)
    public ItemModes ItemMode {
        get {
            return m_itemModes;
        }
        protected set {
            m_itemModes = value;
        }
    }

    // Protected
    // 継承先 に 公開 

    // Get(取得する) 値を返す
    // Set(指定する) 値を更新

    // 開始処理
    public void Start() {
        m_itemModes = (ItemModes)Random.Range((int)ItemModes.SPEED_UP, (int)ItemModes.MAX);

        transform.DORotate(new Vector3(0, 10, 0), 0.1f)
                 .SetRelative()
                 .SetLoops(-1, LoopType.Incremental);

        // メソッドチェーン (関数をつなげる)
        // 回転(Y軸 + 10 ,間隔 0.1秒ごと)
        // 今あるとこの値を参考に
        // ループ(-1 無限ループ : 無限にやってね ++)       

        //transform.DOJump(new Vector3(0, 10, 0), 10.0f, 1, 1.0f)
        //         .SetLoops(10, LoopType.Incremental);


        // transform.DOMove 移動
        // transfrm.
        // transform.DOScale // 拡大縮小

        // 無限に → 今あることろの値を参考に → Y軸に10度づつ0.1間隔で回転してください。
        // 10回だけ → ジャンプしてください
    }
}

// < クラス >
// Unity はクラスベースで作られている
// [C C# C++] [Java] [Python]
// クラス

// 使用するキーは同じ
// ?ブロックに触れたらアイテム取得
// アイテム → アニメーション

// 敵 プレイヤー
// HP 攻撃力 防御力
// プレイヤー同士