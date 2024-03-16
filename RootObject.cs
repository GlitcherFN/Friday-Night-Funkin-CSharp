// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

public class Note_
{
    public List<List<object>> sectionNotes { get; set; }
    public int lengthInSteps { get; set; }
    public bool mustHitSection { get; set; }
}

public class RootObject
{
    public Song song { get; set; }
}

public class Song
{
    public string player1 { get; set; }
    public string player2 { get; set; }
//    public List<List<object>> events { get; set; }
    public string gfVersion { get; set; }
    public List<Note_> notes { get; set; }
    public string player3 { get; set; }
    public string song { get; set; }
    public string stage { get; set; }
    public bool needsVoices { get; set; }
    public bool validScore { get; set; }
    public int bpm { get; set; }
    public int speed { get; set; }
}
