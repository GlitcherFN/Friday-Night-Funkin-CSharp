using Friday_Night_Funkin__C_;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace Friday_Night_Funkin__C_ 
{
public class Note
{
    public int Timestamp { get; set; }
    public int NoteType { get; set; }
    public Rectangle Bounds { get; set; } // Rectangle representing the note's position and size
    public Texture2D Texture { get; set; } // Texture representing the note's sprite
    public bool ShouldRemove { get; set; }
    public Vector2 Position { get; set; }
    public int X { get; set; } = 100;
    public int Y { get; set; } = 100;

    public const int V = 130 * 2;
    private static readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f / 20f);
    public static AnimationManager notes;
    public static SpriteSheet spriteSheet;
    public static void LoadContent()
    {
        spriteSheet = Game1.spriteSheetLoader.Load("Notes.png");
        LoadContent();
            notes = new AnimationManager(
                Game1.spriteSt, new Vector2(520, 100), Game1.strumNoteRed);
        }
        public void Update(GameTime gameTime)
    {
        notes = new AnimationManager(
        spriteSheet, new Vector2(100, +100), Game1.strumNoteRed);
    }

    // Additional properties and methods as needed

    public void Draw(SpriteBatch spriteBatch)
    {
        if (X != null)
        {
                System.Diagnostics.Debug.WriteLine(Game1.spawnNote.CurrentSprite);
                Game1.spriteRender.Draw(
                Game1.spawnNote.CurrentSprite,
                Game1.spawnNote.CurrentPosition, Color.White, 0, 1, Game1.spawnNote.CurrentSpriteEffects);
            }
        // Or draw a rectangle if you're not using sprites
        else
        {
            System.Diagnostics.Debug.WriteLine("TEXTURES IS NULL NOOOOOOOO!!!!!!!!!!! :sob:");
            spriteBatch.DrawRectangle(Bounds, Color.White);
        }
    }
}
public static class GameArea
{
    private static List<Note> notes = new List<Note>();

/*    public static void AddNote(Note note)
    {
        //NoteGroup.Add(note);
    }*/

    public static void ScheduleRemoveNote(Note note, int timestamp)
    {
        // Schedule removal of the note after a certain time
        // For simplicity, we'll just mark the note for removal
        // and it will be removed during the next update
        note.ShouldRemove = true;
    }

    public static void Update(GameTime gameTime)
    {
        // Update notes
        foreach (var note in notes)
        {
            note.Update(gameTime);
        }

        // Remove notes that should be removed
        notes.RemoveAll(note => note.ShouldRemove);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        // Draw notes
        foreach (var note in notes)
        {
            note.Draw(spriteBatch);
        }
    }
}
public class NoteManager
{
    private Texture2D noteTexture; // Texture representing the note
    private SpriteBatch spriteBatch;
    private NoteGroup noteGroupi;

    public NoteManager(Texture2D noteTexture, SpriteBatch spriteBatch)
    {
        this.noteTexture = noteTexture;
        this.spriteBatch = spriteBatch;
    }

    //public void CreateNoteFromChart(int timestamp, int noteType)
    //{
    //    int[] validNoteTypes = { 0, 1, 2, 3, 4, 5, 6, 7, -1 };
    //    if (!Array.Exists(validNoteTypes, element => element == noteType))
    //    {
    //        System.Diagnostics.Debug.WriteLine("Invalid noteType: " + noteType);
    //        return; // Skip invalid note types
    //    }

    //    // Create a new note instance
    //    Note note = new();
    //    note.Timestamp = timestamp;
    //    note.NoteType = noteType;

    //    // Adjust the position and size of the note
    //    note.Bounds = new Rectangle(170 * noteType, 0, noteTexture.Width, noteTexture.Height);

    //    // Adjust the scale of the note (optional

    //    // Set texture for note or handle text rendering
    //    if (noteType == -1)
    //    {
    //        // Render text for "Show Song"
    //        // This part assumes you have access to a SpriteFont for rendering text
    //        // You might need to adjust the position and other properties based on your requirements
    //        // spriteBatch.DrawString(font, "Show Song", new Vector2(note.Bounds.X, note.Bounds.Y), Color.White);
    //    }
    //    else
    //    {
    //        // Assign texture for other note types
    //        note.Texture = noteTexture;
    //    }

    //    // Add the note to the game area
    //    NoteGroup.Add(this.note);

    //    // Schedule removal of the note after a certain time
    //    // You can use your own timing logic here
    //    // For simplicity, I'm using a fixed duration of 1400 milliseconds
    //    // noteGroup.ScheduleRemoveNote(note, timestamp + 1400);
    //    }
    }
}