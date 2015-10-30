namespace WordsMatching
{
    /// <summary>
    /// Summary description for StringMatcher.
    /// </summary>
    ///
    public delegate float Similarity(string s1, string s2);

    public class MatchsMaker
    {

        public string _lString { get; set; }
        public string _rString { get; set; }
        private string[] _leftTokens, _rightTokens;
        private int leftLen, rightLen;
        private float[,] cost;
        private Similarity getSimilarity;

        private bool _accentInsensitive;
        private static MatchsMaker matchsMaker = null;

        public MatchsMaker(string left, string right)
            : this(left, right, false)
        {
        }

        public MatchsMaker(string left, string right, bool accentInsensitive)
        {
            _accentInsensitive = accentInsensitive;

            _lString = left;
            _rString = right;

            if (_accentInsensitive)
            {
                _lString = StripAccents(_lString);
                _rString = StripAccents(_rString);
            }

            MyInit();
        }

        public static MatchsMaker getInstance(string ourString, string yourString)
        {
            if(matchsMaker==null)
            {
                matchsMaker = new MatchsMaker(ourString, yourString);
            }else
            {
                matchsMaker._lString = ourString;
                matchsMaker._rString = yourString;
                matchsMaker.MyInit();
            }
            return matchsMaker;
        }
        private string StripAccents(string input)
        {
            string beforeConversion = "�����������������������������������ǒ�";
            string afterConversion = "aAaAaAaAeEeEeEeEiIiIiIoOoOoOuUuUuUcC'n";

            System.Text.StringBuilder sb = new System.Text.StringBuilder(input);

            for (int i = 0; i < beforeConversion.Length; i++)
            {
                char beforeChar = beforeConversion[i];
                char afterChar = afterConversion[i];

                sb.Replace(beforeChar, afterChar);
            }

            sb.Replace("�", "oe");
            sb.Replace("�", "ae");

            return sb.ToString();
        }

        private void MyInit()
        {
            ISimilarity editdistance = new Leven();
            getSimilarity = new Similarity(editdistance.GetSimilarity);

            //ISimilarity lexical=new LexicalSimilarity() ;
            //getSimilarity=new Similarity(lexical.GetSimilarity) ;

            Tokeniser tokeniser = new Tokeniser();
            _leftTokens = tokeniser.Partition(_lString);
            _rightTokens = tokeniser.Partition(_rString);
            if (_leftTokens.Length > _rightTokens.Length)
            {
                string[] tmp = _leftTokens;
                _leftTokens = _rightTokens;
                _rightTokens = tmp;
                string s = _lString; _lString = _rString; _rString = s;
            }

            leftLen = _leftTokens.Length - 1;
            rightLen = _rightTokens.Length - 1;
            Initialize();
        }

        private void Initialize()
        {
            cost = new float[leftLen + 1, rightLen + 1];
            for (int i = 0; i <= leftLen; i++)
                for (int j = 0; j <= rightLen; j++)
                    cost[i, j] = getSimilarity(_leftTokens[i], _rightTokens[j]);
        }

        public float GetScore()
        {
            BipartiteMatcher match = new BipartiteMatcher(_leftTokens, _rightTokens, cost);
            return match.Score;
        }

        public float Score
        {
            get
            {
                return GetScore();
            }
        }
    }
}