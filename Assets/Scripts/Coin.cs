using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 子供 : 親(100％ - private + public + protected))
// private   : 親の財産                         親〇 子供 × 他のクラス ×
// public    : 子供にもしくは誰かに1000円上げる 親〇 子供 〇 他のクラス 〇
// protected : 100円募金                        親〇 子供 〇 他のクラス ×

public class Coin : Item {
    // 開始処理
    private void Start() {
        base.Start();
        base.ItemMode = ItemModes.COIN;
        Debug.Log(base.ItemMode);
    }
}