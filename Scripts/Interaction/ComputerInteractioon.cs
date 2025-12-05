using UnityEngine;

/// <summary>
/// コンピューター操作を管理するクラス
/// USBを持っている場合のみモニターを開くことができる
/// </summary>
public class ComputerInteractioon : Interactable
{
    /// <summary>
    /// リアルカメラコントローラーへの参照
    /// モニターの表示を制御する
    /// </summary>
    public RealCameraController realCamController;

    /// <summary>
    /// プレイヤーがコンピューターと相互作用したときに呼び出される
    /// USBを所持している場合、モニターを開いてタスクシーケンスを開始する
    /// </summary>
    public override void OnInteract()
    {
        // USBを持っているか確認
        if(CollectionManager.instance.hasUSB)
        {
            // カメラコントローラーが存在する場合、モニターを開く
            if (realCamController!= null)
              realCamController.OpenMonitor();
              this.enabled = false;
            
            // タスクマネージャーが存在する場合、シーケンスを開始
            if (TaskManager_B.instance != null)
            {
                TaskManager_B.instance.ActivateSequence();
            }
            // このスクリプトを無効化
            this.enabled = false;
        }
        else
        {
            // USBが必要であることをログに出力
            Debug.Log("Need Usb");
        }
    }
}
