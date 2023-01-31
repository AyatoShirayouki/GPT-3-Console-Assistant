using Newtonsoft.Json;
using System.Text;

Console.Write("Input: ");
string? str = Console.ReadLine();

if (str.Length > 0)
{
    

    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", "Bearer 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");

    var content = new StringContent("{\"model\": \"text-davinci-003\",\r\n  \"prompt\": \"" + str + "\",\r\n  \"max_tokens\": 1000,\r\n  \"temperature\": 0}", Encoding.UTF8, "application/json");

    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

    string responseString = await response.Content.ReadAsStringAsync();

    try
    {
        var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

        string guess = GuessCommand(dyData!.choices[0].text);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"--> Guess: {guess}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Deserialisation of json failed, Error: {ex}");
    }

    Console.WriteLine(responseString);
}
else
{
    Console.WriteLine("--> You need to provide input in orrder to get a result!");
}

static string GuessCommand(string raw)
{
    Console.WriteLine("--> GPT-3 API Returned Text: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(raw);

    var lastIndex = raw.LastIndexOf('\n');

    string guess = raw.Substring(lastIndex + 1);

    Console.ResetColor();

    TextCopy.ClipboardService.SetText(guess);

    return guess;
}
