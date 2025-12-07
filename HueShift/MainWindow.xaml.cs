using System;
using System.Windows;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace HueShift
{
    public partial class MainWindow : Window
    {
        private const string DefaultColorCode = "#1E90FF";

        public MainWindow()
        {
            InitializeComponent();
            // アプリ起動時に初期値で一度計算を実行する
            CalculateColor(DefaultColorCode);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            string colorInput = InputColorTextBox.Text.Trim();
            CalculateColor(colorInput);
        }

        /// <summary>
        /// 入力された色コードを解析し、各変換結果をUIへ反映します。
        /// </summary>
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

            // 5. グレースケールを計算・表示
            Color grayscaleColor = ColorConverter.GetGrayscaleColor(originalColor);
            UpdateResultColor(
                grayscaleColor,
                GrayscaleColorRect,
                GrayscaleColorHex,
                GrayscaleColorRgb
            );
        }

        /// <summary>
        /// 色コード文字列を Color オブジェクトに変換します。
        /// </summary>
        private Color ParseColorCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("入力が空です。");

            string trimmed = code.Trim();

            // RGB形式 (例: 255,0,0 または rgb(255,0,0))
            if (TryParseRgb(trimmed, out Color rgbColor))
            {
                return rgbColor;
            }

            // HEX形式の判定 (例: #RRGGBB または RRGGBB)
            if (TryParseHex(trimmed, out Color hexColor))
            {
                return hexColor;
            }

            throw new ArgumentException("サポートされていない形式、または不正な値です。");
        }

        /// <summary>
        /// RGB表記を Color オブジェクトに変換します。"rgb(…)"のラッパーも許容します。
        /// </summary>
        private static bool TryParseRgb(string input, out Color color)
        {
            color = default;

            string normalized = input;
            if (input.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
            {
                int start = input.IndexOf('(');
                int end = input.LastIndexOf(')');

                if (start >= 0 && end > start)
                {
                    normalized = input[(start + 1)..end];
                }
            }

            string[] rgbParts = normalized.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // HEX形式の判定 (例: #RRGGBB または RRGGBB)
            if (code.StartsWith("#") || code.Length == 6 || code.Length == 7)
            {
                string hex = code.StartsWith("#") ? code[1..] : code;

                if (hex.Length != 6)
                {
                    throw new ArgumentException("HEXコードは6桁で指定してください。");
                }

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

            // RGB形式 (例: 255,0,0)
            string[] rgbParts = code.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (rgbParts.Length == 3 &&
                byte.TryParse(rgbParts[0], out byte red) &&
                byte.TryParse(rgbParts[1], out byte green) &&
                byte.TryParse(rgbParts[2], out byte blue))
            {
                color = Color.FromRgb(red, green, blue);
                return Color.FromRgb(red, green, blue);
            }

            return false;
        }

        /// <summary>
        /// HEX表記を Color オブジェクトに変換します。#付き/なしの6桁のみを受け付けます。
        /// </summary>
        private static bool TryParseHex(string input, out Color color)
        {
            color = default;

            Match match = Regex.Match(input, "^#?[0-9A-Fa-f]{6}$");
            if (!match.Success)
            {
                return false;
            }

            string hex = input.StartsWith("#") ? input[1..] : input;

            if (!IsHexString(hex))
            {
                throw new ArgumentException("HEXコードは0-9とA-Fの組み合わせで指定してください。");
            }

            byte rHex = Convert.ToByte(hex[0..2], 16);
            byte gHex = Convert.ToByte(hex[2..4], 16);
            byte bHex = Convert.ToByte(hex[4..6], 16);

            color = Color.FromRgb(rHex, gHex, bHex);
            return true;
        }

        /// <summary>
        /// 文字列がHEX表記に使える文字だけで構成されているかを判定します。
        /// </summary>
        private static bool IsHexString(string value)
        {
            foreach (char c in value)
            {
                bool isHexDigit = (c >= '0' && c <= '9') ||
                                  (c >= 'A' && c <= 'F') ||
                                  (c >= 'a' && c <= 'f');

                if (!isHexDigit)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 元の色情報をUIに表示します。
        /// </summary>
        private void UpdateOriginalColor(Color color)
        {
            OriginalColorRect.Fill = new SolidColorBrush(color);
            OriginalColorHex.Text = FormatHex(color);
            OriginalColorRgb.Text = FormatRgb(color);
        }

        /// <summary>
        /// 計算結果の色をUIに表示します。
        /// </summary>
        private void UpdateResultColor(Color color, System.Windows.Shapes.Rectangle rect, System.Windows.Controls.TextBlock hexText, System.Windows.Controls.TextBlock rgbText)
        {
            rect.Fill = new SolidColorBrush(color);
            hexText.Text = FormatHex(color);
            rgbText.Text = FormatRgb(color);
        }

        /// <summary>
        /// 色をHEX表記の文字列に整形します。
        /// </summary>
        private static string FormatHex(Color color)
        {
            return $"HEX: #{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// 色をRGB表記の文字列に整形します。
        /// </summary>
        private static string FormatRgb(Color color)
        {
            return $"RGB: ({color.R}, {color.G}, {color.B})";
        }
    }
}