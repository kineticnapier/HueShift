# HueShift

HueShiftは、入力された色コードから補色・反転色・グレースケールを計算して表示するWPFアプリです。HEXまたはRGB表記で色を指定すると、元の色と計算結果を色パネルと数値で確認できます。

## 主な機能
- HEX（`#RRGGBB` / `RRGGBB`）またはRGB（`255,0,0` や `rgb(255,0,0)`）表記の色を解析
- 入力色の補色、反転色、グレースケールを計算
- 計算結果を色パネルとHEX/RGB値で表示
- 起動時はデフォルト色 `#1E90FF` で自動計算

## 使い方
1. テキストボックスに色コードを入力します（例: `#FF0000` または `34,139,34`）。
2. **計算** ボタンを押すと、補色・反転色・グレースケールが更新されます。
3. 入力形式が不正な場合はエラーメッセージが表示されます。入力を修正して再度計算してください。

## 動作環境
- .NET 10.0 (Windows向けWPF)
- Windows環境（WinExeとしてビルド）

## ビルド手順
1. .NET SDK 10 をインストールします。
2. リポジトリ直下で以下を実行します。
   ```bash
   dotnet build
   ```
3. ビルドに成功すると `HueShift/bin/Debug/net10.0-windows/` に実行ファイルが生成されます。

## プロジェクト構成
- `HueShift/MainWindow.xaml`: UIレイアウト
- `HueShift/MainWindow.xaml.cs`: 入力のパースと結果表示のロジック
- `HueShift/ColorConverter.cs`: 色変換（補色・反転色・グレースケール、RGB/HSL変換）
- `HueShift/HueShift.csproj`: WPFアプリのプロジェクト設定

## ライセンス
このプロジェクトは [LICENSE.txt](LICENSE.txt) の条件に従います。
