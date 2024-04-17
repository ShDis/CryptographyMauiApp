namespace CryptographyMauiApp;

/// <summary>
/// Индийский код в рамках лимитированного времени
/// </summary>
public partial class PolybiusSquare : ContentPage
{
	public PolybiusSquare()
	{
		InitializeComponent();

        this.BindingContext = this;

        GeneratePolybius();
    }

    // Define the English and Russian alphabets
    private string englishAlphabet = "abcdefghijklmnopqrstuvwxyz";
    private string russianAlphabet = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
    private string keyPhrase = "polybius";
    private string phrase = "сейчасчетыречасаночи";
    private int cols = 5;
    private int rows = 0;
    private Dictionary<char, (int, int)> polybiusLayout = new Dictionary<char, (int, int)>();
    public string Table { get; set; }
    public string Coded { get; set; }
    public string Decoded { get; set; }


    private void GeneratePolybius()
    {
        polybiusLayout = new Dictionary<char, (int, int)>();

        // Create a combined alphabet by appending the English and Russian alphabets
        string combinedAlphabet = englishAlphabet + russianAlphabet;

        // Remove duplicates from the combined alphabet
        combinedAlphabet = new string(new HashSet<char>(combinedAlphabet).ToArray());

        // Create the key phrase alphabet by combining the key phrase with the combined alphabet
        string keyPhraseAlphabet = keyPhrase + combinedAlphabet;

        // Remove duplicates from the key phrase alphabet
        keyPhraseAlphabet = new string(new HashSet<char>(keyPhraseAlphabet).ToArray());

        // Assign positions to each character in the key phrase alphabet
        int row = 0;
        int col = 0;
        string labelRow = "";
        string table = "";
        foreach (char c in keyPhraseAlphabet)
        {
            polybiusLayout[c] = (row, col);
            labelRow += " " + c;
            col++;
            if (col >= cols)
            {
                table = row == 0 ? labelRow : table + "\n" + labelRow; // обыкновенный индийский код
                labelRow = "";
                col = 0;
                row++;
            }
        }
        table += "\n" + labelRow;
        rows = col == 0 ? row : row + 1;

        string text = Entry_Phrase.Text;
        string encr = "";
        string decr = "";
        foreach (char c in text)
        {
            encr += GetEncryptedChar(polybiusLayout.GetValueOrDefault(c).Item1, polybiusLayout.GetValueOrDefault(c).Item2);
            decr += GetDecryptedChar(polybiusLayout.GetValueOrDefault(c).Item1, polybiusLayout.GetValueOrDefault(c).Item2);
        }
        Coded = encr;
        Decoded = decr;
        Table = table;
        // костыль
        Label_Table.Text = Table;
        Entry_Decoded.Text = Decoded;
        Entry_Coded.Text = Coded;
    }

    private char GetChar(int row, int col)
    {
        return polybiusLayout.FirstOrDefault(x => x.Value == (row, col)).Key;
    }

    private char GetEncryptedChar(int row, int col)
    {
        return polybiusLayout.FirstOrDefault(x => x.Value == (row - 1 < 0 ? rows - 1 : row - 1, col)).Key;
    }

    private char GetDecryptedChar(int row, int col)
    {
        return polybiusLayout.FirstOrDefault(x => x.Value == (row + 1 > rows  ? 0 : row + 1, col)).Key;
    }

    private void Entry_Phrase_Completed(object sender, EventArgs e)
    {
        phrase = Entry_Phrase.Text;
        GeneratePolybius();
    }

    private void Entry_KeyPhrase_Completed(object sender, EventArgs e)
    {
        keyPhrase = Entry_KeyPhrase.Text;
        GeneratePolybius();
    }

    private void Entry_Columns_Completed(object sender, EventArgs e)
    {
        int check = 0;
        int def = 5;
        try
        {
            check = Int32.Parse(Entry_Columns.Text);
        }
        catch
        {
            check = def;
        }
        if (check < 2 || check > 20) { check = def; }
        cols = check;
        Entry_Columns.Text = check.ToString();

        GeneratePolybius();
    }
}