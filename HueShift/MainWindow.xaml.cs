using System;
using System.Windows;
using System.Windows.Media;

namespace HueShift
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 起動時に一度計算を実行
            CalculateColor("#1E90FF");
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputColorTextBox.Text.Trim();
            CalculateColor(input);
        }

        private void CalculateColor(string input)
        {
            Color originalColor;

            // 1. 色コードの解析 (HEXまたはRGB)
            try
            {
                originalColor = ParseColorCode(input);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"色コードの形式が正しくありません。\nエラー: {ex.Message}", "入力エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 2. 元の色情報を更新
            UpdateOriginalColor(originalColor);

            // 3. 補色を計算・表示
            Color complementaryColor = ColorConverter.GetComplementaryColor(originalColor);
            UpdateResultColor(
                complementaryColor,
                ComplementaryColorRect,
                ComplementaryColorHex,
                ComplementaryColorRgb
            );

            // 4. 反対色（反転色）を計算・表示
            Color invertColor = ColorConverter.GetInvertColor(originalColor);
            UpdateResultColor(
                invertColor,
                InvertColorRect,
                InvertColorHex,
                InvertColorRgb
            );
        }

        /// <summary>
        /// 色コード文字列を Color オブジェクトに変換します。
        /// </summary>
        private Color ParseColorCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("入力が空です。");

            if (code.StartsWith("#") || code.Length == 6 || code.Length == 7) // 例: #RRGGBB
            {
                string hex = code.StartsWith("#") ? code[1..] : code; // IDE0057: Substring の簡素化

                // 6桁のHEXコード (RRGGBB) のみを処理
                if (hex.Length == 6)
                {
                    try
                    {
                        byte rHex = Convert.ToByte(hex[0..2], 16);
                        byte gHex = Convert.ToByte(hex[2..4], 16);
                        byte bHex = Convert.ToByte(hex[4..6], 16);

                        return Color.FromRgb(rHex, gHex, bHex);
                    }
                    catch (FormatException)
                    {
                        throw new ArgumentException("HEXコードの形式が不正です。");
                    }
                }
            }

            // RGB形式 (例: 255,0,0)
            string[] parts = code.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 &&
                byte.TryParse(parts[0], out byte r) &&
                byte.TryParse(parts[1], out byte g) &&
                byte.TryParse(parts[2], out byte b))
            {
                return Color.FromRgb(r, g, b);
            }

            throw new ArgumentException("サポートされていない形式、または不正な値です。");
        }

        /// <summary>
        /// 元の色情報をUIに表示します。
        /// </summary>
        private void UpdateOriginalColor(Color color)
        {
            OriginalColorRect.Fill = new SolidColorBrush(color);
            OriginalColorHex.Text = $"HEX: #{color.R:X2}{color.G:X2}{color.B:X2}";
            OriginalColorRgb.Text = $"RGB: ({color.R}, {color.G}, {color.B})";
        }

        /// <summary>
        /// 計算結果の色をUIに表示します。
        /// </summary>
        private void UpdateResultColor(Color color, System.Windows.Shapes.Rectangle rect, System.Windows.Controls.TextBlock hexText, System.Windows.Controls.TextBlock rgbText)
        {
            rect.Fill = new SolidColorBrush(color);
            hexText.Text = $"HEX: #{color.R:X2}{color.G:X2}{color.B:X2}";
            rgbText.Text = $"RGB: ({color.R}, {color.G}, {color.B})";
        }
    }
}