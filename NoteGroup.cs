using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Friday_Night_Funkin__C_;

class NoteGroup
{
    public static List<Note> Notes { get; } = new List<Note>();
    public static void Add(Note note)
    {
        ///Notes.Add(note);
    }

    public void Update(GameTime gameTime)
    {
        // Update logic for all notes in the group
        foreach (var note in Notes)
        {
            // Update logic for individual note
        }
    }

    public static void drawSpr(SpriteBatch spriteBatch)
    {
        // Drawing logic for all notes in the group
        foreach (var note in Notes)
        {
            note.Draw(spriteBatch);
        }
    }

    // Additional methods for managing the group as needed
}
