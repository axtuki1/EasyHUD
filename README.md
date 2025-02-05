# EasyHUD

目の前に表示する各種情報を表示するHUDの枠組みです。  
VRとデスクトップを検出して自動で適切な表示方法を選択します。

## 導入方法

1. VCC等を利用してEasyHUDをインポートします。
2. `EasyHUD.prefab`をシーンに配置します。
3. `EasyHUD/PlayerHUD`の位置を調整しちょうど良い位置に配置します。
4. `EasyHUD/PlayerHUD/Offset/UIRoot`配下に表示したいUIを配置します。

## VR時に最前面に表示する場合

### TextMeshProの場合

Font Asset内のMaterialに適用されているシェーダーを  
TextMeshPro/Distance Field Overlayに変更します。

### RawImage, Imageの場合

サンプルとして付属している`AX/UI/Sample/HUD Image`を使用するか、  
任意のUI要素を前面に表示するシェーダーを使用してください。

## プレイヤーによるOffsetの調整

### uGUIで調整する場合

`EasyHUD/PlayerTrack`の(X | Y | Z)Sliderに調整用スライダーを登録し、  
適用するタイミングで`SendCustomEvent`で`ReadSlider`イベントを呼び出してください。

これによりスライダーから値を読み出しOffsetを調整することができます。

### UdonSharpから調整する場合

`EasyHUD/PlayerTrack`の`PlayerTracking.SetOffset(Vector3, Quaternion)`を  
呼ぶことで設定が可能です。

## サンプル

### SystemLog

システムログを表示するサンプルです。  
どっかで見たことあるかもです。

#### 導入方法

1. `EasyHUD/PlayerHUD/Offset/UIRoot`配下に`SystemLog.prefab`を配置します。
2. `SystemLog/TextTemplate`のAudioClipに1文字ずつ表示する際の効果音を設定します。(任意)
3. `SystemLog/TextTemplate`のTextMeshProUGUIに使用するフォントを設定します。(推奨)
4. シーン直下に`SystemLogHelper.prefab`を配置します。
5. `SystemLogHelper`のSystemLogに`SystemLog/TextTemplate`を設定します。
6. 必要に応じてSystemLogHelperの設定を変更します。

### UdonSharpからの利用

`SystemLogHelper`の`SystemLogHelper.Log(string)`を呼ぶことで表示できます。

### 注意

メッセージが長すぎる場合、表示に時間がかかる上に画面上の邪魔になります。  
表示内容は簡潔にするようにしてください。