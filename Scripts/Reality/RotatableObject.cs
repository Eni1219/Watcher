using UnityEngine;
using System.Collections;

    /// <summary>
    /// オブジェクトを回転させるクラス
    /// 指定した軸と角度に基づいてスムーズに回転し、鍵を表示する
    /// </summary>
    public class RotatableObject : MonoBehaviour
    {
        public enum Axis { X, Y, Z } 

        public Axis rotationAxis = Axis.Y;
        public float targetAngle = -100.0f;
        public float rotationDuration = 1.0f;
        public string rotationSoundName;
        public GameObject Key;
        
        private Quaternion initialRotation;
        private bool isRotating = false;
        private Coroutine rotationCoroutine;

        void Awake()
        {
            initialRotation = transform.rotation;
        }
        
        private void Start()
        {
            Key.SetActive(false);
        }

        /// <summary>
        /// 回転をトリガー
        /// </summary>
        public void TriggerRotation()
        {
            if (isRotating)
               return;

            AudioManager.instance.Play("Unlock");
            rotationCoroutine = StartCoroutine(RotateSequence());
            Key.SetActive(true);
        }

        /// <summary>
        /// 回転シーケンス
        /// </summary>
        private IEnumerator RotateSequence()
        {
            isRotating = true; 

            Quaternion startRotation = transform.rotation; 
            Quaternion targetRotation;

            // 回転軸に基づいて目標回転を計算
            switch (rotationAxis)
            {
                case Axis.X:
                    targetRotation = initialRotation * Quaternion.Euler(targetAngle, 0, 0);
                    break;
                case Axis.Y:
                    targetRotation = initialRotation * Quaternion.Euler(0, targetAngle, 0);
                    break;
                case Axis.Z:
                    targetRotation = initialRotation * Quaternion.Euler(0, 0, targetAngle);
                    break;
                default:
                    targetRotation = startRotation;
                    break;
            }

            // スムーズな回転を実行
            float timer = 0f;
            while (timer < rotationDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / rotationDuration);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            transform.rotation = targetRotation;

            isRotating = false; 
            rotationCoroutine = null; 
            this.enabled = false;
        }
    }