using System;

namespace EnglishHelper
{
    public class ResultCompare
    {
        //public string CompareText { get; set; }
        //public string InputText { get; set; }

        //public string AfterCompareInHtml { get; set; }
        //public string AfterCompareInPlain { get; set; }

        //public int Percent { get; set; }
        //public string Level { get; set; }

        /// <summary>
        /// begin bold,color html
        /// </summary>
        private const string BEGIN_SPAN_HTML = "<span style=\"font-weight:bold;color:FF0000;\">";

        /// <summary>
        /// end html
        /// </summary>
        private const string END_SPAN_HTML = "</span>";

        public static string MakeResult(string InputText, string CompareText, bool UseLevenshteinDistance)
        {
           string InputTextOK = InputText.Trim().ToUpper();
           string CompareTextOK = CompareText.Trim().ToUpper();
            int Percent = 0;
            if (UseLevenshteinDistance)
            {
                Percent = LevenshteinDistance(InputTextOK, CompareTextOK);
            }
            else
            {
                Percent = StringCompare(InputTextOK, CompareTextOK);
            }

            string AfterCompareInPlain = "Our sentence:\r\n" + InputText
                + "\r\nYour sentence:\r\n" + CompareText +
                "\r\nMatch: " + Percent + " .\r\nResult: " + GetLevel(Percent)+".";
            return AfterCompareInPlain;
        }


        public static string MakeResultString(string InputText, string CompareText, int Percent)
        {
            string AfterCompareInPlain = "Our sentence:\r\n" + InputText
                + "\r\nYour sentence:\r\n" + CompareText +
                "\r\nMatch: " + Percent + " .\r\nResult: " + GetLevel(Percent) + ".";
            return AfterCompareInPlain;
        }
        /// <summary>
        /// return color and bold text in html
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private String MakeColorBoldText(string input)
        {
            return BEGIN_SPAN_HTML + input + END_SPAN_HTML;
        }

        /// <summary>
        /// compare char by char
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>percent same</returns>
        private static int StringCompare(string a, string b)
        {
            if (a == b) //Same string, no iteration needed.
                return 100;
            if ((a.Length == 0) || (b.Length == 0)) //One is empty, second is not
            {
                return 0;
            }
            double maxLen = a.Length > b.Length ? a.Length : b.Length;
            int minLen = a.Length < b.Length ? a.Length : b.Length;
            int sameCharAtIndex = 0;
            for (int i = 0; i < minLen; i++) //Compare char by char
            {
                if (a[i] == b[i])
                {
                    sameCharAtIndex++;
                }
            }
            return (int)(sameCharAtIndex / maxLen * 100);
        }

        private static string GetLevel(int percent)
        {
            if (percent < 50)
            {
                return "Bad";
            }
            else if (percent >= 50 && percent < 70)
            {
                return "Average";
            }
            else if (percent >= 70 && percent < 100)
            {
                return "Good";
            }
            else
            {
                return "Excellent";
            }
        }

        /// <summary>
        /// Compute LevenshteinDistance.
        /// </summary>
        public static int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 2, m + 2];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            int i = 0;
            int j = 0;

            for (i = 0; i <= n; i++)
            {
                d[i, 0] = i;
            }

            for (j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }

            for (i = 1; i <= n; i++)
            {
                for (j = 1; j <= m; j++)
                {
                    int cost = 0;
                    if (t[j - 1] == s[i - 1])
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}