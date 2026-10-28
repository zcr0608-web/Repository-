# Mobile Strike: Tap Shooter — Unity 初學者匯入指南

> 本指南會協助你把這個資料夾中的 C# 腳本匯入已建立的 Unity 專案。腳本尚未在此環境中以 Unity Editor 編譯或實機測試，因此請先在電腦上驗證。

## 1. 下載程式碼

1. 開啟 https://github.com/zcr0608-web/Repository-/tree/main/MobileStrikeUnity
2. 回到儲存庫首頁，按綠色 **Code** → **Download ZIP**。
3. 解壓縮 ZIP。

## 2. 複製核心腳本

在解壓縮後的資料夾中，開啟 `MobileStrikeUnity/Assets/MobileStrike/Scripts/`。先複製下列 6 個檔案：

- `MobileStrikePrototypeBootstrap.cs`
- `MobileStrikePlayerController.cs`
- `MobileStrikeTouchPad.cs`
- `MobileStrikePlayerHealth.cs`
- `MobileStrikeWeapon.cs`
- `MobileStrikeCrosshair.cs`

在自己的 Unity 專案中，將它們放到：

`Assets/MobileStrike/Scripts/`

若資料夾不存在，請在 Unity 的 Project 視窗中依序建立 `MobileStrike` 和 `Scripts` 資料夾。也可以先在檔案總管複製，但不要把檔案放在 `Packages` 或 `Library` 資料夾。

## 3. 等待 Unity 編譯

切回 Unity Editor，等待右下角的匯入/編譯工作完成。開啟 **Window → General → Console**。如果看到紅色錯誤，先記下第一條錯誤；不要一次修改很多檔案。

## 4. 建立原型啟動物件

1. 在 Hierarchy 空白處按右鍵，選 **Create Empty**。
2. 將物件命名為 `PrototypeBootstrap`。
3. 選取物件，在 Inspector 按 **Add Component**。
4. 搜尋並加入 `MobileStrikePrototypeBootstrap`。

## 5. 試玩

按 Unity 上方的 **Play**。如果主選單出現，按 **START TRAINING** 開始訓練。電腦版目前以 WASD 移動，按住滑鼠右鍵並移動滑鼠來轉動視角；觸控操作需在 Android 裝置上另行測試。

## 常見問題

- **搜尋不到腳本：** 檢查檔名與類別名稱是否一致，並查看 Console 第一條紅色錯誤。
- **Unity UI 類別找不到：** 確認使用包含 Unity UI 的 Unity 安裝；檢查 Package Manager 中是否有 **Unity UI (com.unity.ugui)**。
- **畫面空白或按 Play 報錯：** 打開 Console，複製第一條紅色錯誤及堆疊訊息。
- **不要先匯入多人連線腳本：** `DuelNetworkHealth.cs` 和 `DuelRoundManager.cs` 尚需 Netcode for GameObjects 與完整網路場景設定，不能代表已完成線上 PvP。

## 目前狀態

此版本是 3D 射擊原型的起始程式，不是已完成的商業遊戲。它尚未在 Unity 中完成編譯驗證，線上 PvP、完整動畫/音效、Android APK/AAB 打包與 Google Play 上架仍需後續開發及測試。
