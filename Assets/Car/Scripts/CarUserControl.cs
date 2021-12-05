using System;
using UnityEngine;
using Cinemachine;
using TMPro;
using DG.Tweening;

namespace UnityStandardAssets.Vehicles.Car {
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera m_camera        = default; // 1人称視点
        [SerializeField] private TextMeshProUGUI          m_itemText      = default; // アイテムindex
        [SerializeField] private ParticleSystem           m_sppedUpEffect = default; // スピードup
        [SerializeField] private GameObject[]             m_collisions    = default; // 当たり判定オブジェクト
        [SerializeField] private Renderer                 m_carRenderer   = default; // 車Renderer
        [SerializeField] private GameObject               m_coinObject    = default; // Coin
        [SerializeField] private GameManager              m_gameManager   = default; // GameManager

        //=================== カメラ
        private CarController m_car;          // CarController
        private bool          m_IsCameraMode; // カメラ

        //=================== アイテム
        private Item          m_currentItem;     // 現在のアイテム
        private int           m_itemCount;       // アイテム数
        private bool          m_isUseItem;       // アイテムを利用しているかどうか

        //=================== Invisible
        private Material      m_carBaseMaterial;
        private Color         m_carBaseColor;   

        private void Start() {
            m_car = GetComponent<CarController>();

            // カメラ
            m_IsCameraMode = false;

            // アイテム
            m_itemText.text = "null";
            m_currentItem   = null;
            m_isUseItem     = false;

            m_carBaseMaterial = new Material(m_carRenderer.material);
            m_carBaseColor = m_carBaseMaterial.GetColor("_BaseColor");
        }

        private void FixedUpdate() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float handbrake = Input.GetAxis("Jump");
            m_car.Move(h, v, v, handbrake);

            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                changedCamera(!m_IsCameraMode);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                usedItem();
            }

        }

        private void changedCamera(bool isCameraMode) {
            // 三項演算子
            //if (m_IsCameraMode == true) { // true
            //    m_camera.Priority = 9;
            //} else {                      // false
            //    m_camera.Priority = 11;
            //}
            m_camera.Priority = (m_IsCameraMode == true) ? 9 : 11;
            m_IsCameraMode    = isCameraMode;
        }

        private void usedItem() {
            if (m_isUseItem || m_currentItem == null) {
                return;
            }
            m_itemCount--;
            switch (m_currentItem.ItemMode) {
                case ItemModes.SPEED_3UP:
                case ItemModes.SPEED_UP:
                    sppedUp();
                    break;
                case ItemModes.COIN:
                    addCoin();
                    break;
                case ItemModes.INVISIBLE:
                    invisible();
                    break;
            }
            if(m_itemCount <= 0) {
                m_currentItem = null;
                m_itemText.text = "null";
            }
        }

        private void sppedUp() {
            m_isUseItem = true;
            m_car.AddSpeedUp();
            m_sppedUpEffect.Play();
            DOVirtual.DelayedCall(3, () => {
                m_sppedUpEffect.Stop();
                m_isUseItem = false;
            });
        }

        private void invisible() {
            m_isUseItem = true;
            m_carBaseMaterial.SetColor("_BaseColor", new Color(1, 1, 1, 0.8f));
            m_carRenderer.material = m_carBaseMaterial;
            foreach (var colObject in m_collisions) {
                colObject.gameObject.layer = 10;
            }
            DOVirtual.DelayedCall(3, () => {
                m_carBaseMaterial.SetColor("_BaseColor", new Color(m_carBaseColor.r, m_carBaseColor.g, m_carBaseColor.b, 1.0f));
                m_carRenderer.material = m_carBaseMaterial;
                foreach (var colObject in m_collisions) {
                    colObject.gameObject.layer = 0;
                }
                m_isUseItem = false;
            });
        }

        private void addCoin() {
            Instantiate(m_coinObject, transform.position - transform.forward * 3.0f + transform.up, Quaternion.identity);
        }

        public void Collision(Collider target,PlayerCollisionCallBack playerCollisionCallBack) {
            //Debug.Log(target.gameObject.name + ":" + playerCollisionCallBack.gameObject.name);
            if (checkPoint(target))    return;
            if (startEndPoint(target)) return;
            if (checkCoin(target))     return;
            if (checkItem(target))     return;
        }

        private bool checkPoint(Collider target) {
            CheckPoint checkPoint = target.GetComponent<CheckPoint>();
            if (checkPoint != null) {
                //Debug.Log("CheckPoint");
                m_gameManager.AddCheckPoint(checkPoint);
                return true;
            }
            return false;
        }

        private bool startEndPoint(Collider target) {
            StartEndPoint startEndPoint = target.GetComponent<StartEndPoint>();
            if (startEndPoint != null) {
                //Debug.Log("CheckPoint");
                m_gameManager.Goal();
                startEndPoint.SetActive(false);
                return true;
            }
            return false;
        }

        private bool checkCoin(Collider target) {
            var coin = target.GetComponent<Coin>();
            if (coin != null) {
                //Debug.Log("Get Coin");
                m_car.AddSpeedUp();
                return true;
            }
            return false;
        }

        private bool checkItem(Collider target) {
            if (m_currentItem != null) {
                return false;
            }
            var item = target.GetComponent<Item>();
            if (item != null) {
                switch (item.ItemMode) {
                    case ItemModes.COIN:      setItem("Coin", item); break;
                    case ItemModes.INVISIBLE: setItem("Inv", item); break;
                    case ItemModes.SPEED_UP:  setItem("SpUp", item); break;
                    case ItemModes.SPEED_3UP: setItem("Sp3Up", item); break;
                    default: break;
                }
                return true;
            }
            return false;
        }

        private void setItem(string name, Item item) {
            m_itemText.text = name;
            m_currentItem   = item;
            if (item.ItemMode == ItemModes.SPEED_3UP) {
                m_itemCount = 3;
            } else {
                m_itemCount = 1;
            }
        }
    }
}