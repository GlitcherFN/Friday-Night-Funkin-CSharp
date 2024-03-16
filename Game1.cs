using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleFpsCounter;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;
using EmptyKeys.UserInterface.Controls;
using static System.Collections.Specialized.BitVector32;
using System.Threading;
using System.Timers;
using MonoGame.Extended.Timers;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
// using JsonSerializer = Newtonsoft.Json.JsonSerializer;
namespace Friday_Night_Funkin__C_
{
    class ReadJsonFile
    {
        static void Main(string[] args)
        {
            StreamReader r = new StreamReader("its-a-me.json");
            string json = r.ReadToEnd();
            var chartData = System.Text.Json.JsonSerializer.Deserialize<RootObject>(json);
        }
    }
    public class Game1 : Game
    {
        static Note_ NoteData;
        static RootObject RootObject;
        private static Song Song;
        private static NoteGroup noteGroup = new NoteGroup();
        private static Texture2D noteTexture; // Texture representing the note
        private static readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f / 20f);
        public SpriteSheet spriteSheet;
        public static SpriteSheetLoader spriteSheetLoader;
        public static SpriteSheet spriteSt;
        public static GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        public static SpriteRender spriteRender;
        public AnimationManager characterAnimationManager;
        SpriteFont font;
        static int noteType;
        AnimationManager leftNote;
        AnimationManager DownNote;
        AnimationManager UpNote;
        AnimationManager RightNote;
        static SpriteFrame spriteFrames;
        KeyboardState previousState;
        private readonly Tweener _tweener = new();
        private static readonly Tweener _tween = new();
        public static AnimationManager spawnNote;
        Rectangle rectangle;
        Texture2D pixel;
        static AnimationManager noteClassNotes = Note.notes;
        public static Animation arrRed;
        public static Animation[] strumNoteRed;

        // Add the class
        SimpleFps fps = new();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            this.IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadChartFromJsonFile();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("File");
            spriteSheetLoader = new SpriteSheetLoader(Content, GraphicsDevice);
            spriteSheet = spriteSheetLoader.Load("Notes.png");
            spriteSt = spriteSheetLoader.Load("Notes.png");
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteRender = new SpriteRender(_spriteBatch);
            var characterStartPosition = new Vector2(200, 100);
            var arrowLEFT = new[] {
            Notes.ArrowLEFT0000,
            };
            var arrowDOWN = new[] {
            Notes.ArrowDOWN0000,
            };
            var arrowUP = new[] {
            Notes.ArrowUP0000,
            };
            var arrowRIGHT = new[] {
            Notes.ArrowRIGHT0000,
            };
            var arrLeft = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowLEFT);
            var arrDown = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowDOWN);
            var arrUp = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowUP);
            var arrRight = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowRIGHT);
            var animations = new[] { arrLeft };
            var animations1 = new[] { arrDown };
            var animations2 = new[] { arrUp };
            var animations3 = new[] { arrRight };
            leftNote = new AnimationManager(
            this.spriteSheet, new Vector2(100, 100), animations);
            DownNote = new AnimationManager(
            this.spriteSheet, new Vector2(260, 100), animations1);
            UpNote = new AnimationManager(
            this.spriteSheet, new Vector2(390, 100), animations2);
            RightNote = new AnimationManager(
            this.spriteSheet, new Vector2(520, 100), animations3);
            base.LoadContent();
        }

        Task LoadChartFromJsonFile()
        {
            noteTexture = Content.Load<Texture2D>("Notes");
            try
            {
                StreamReader r = new StreamReader("its-a-me.json");
                string json = r.ReadToEnd();
                var chartData = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);


                // Ensure Song object is initialized

                Song = chartData.song;

                if (chartData != null && chartData.song != null && chartData.song.notes != null && chartData.song.notes.Count > 0)
                {
                    foreach (var section in chartData.song.notes)
                    {
                        if (section.sectionNotes != null && section.sectionNotes.Count > 0)
                        {
                            foreach (List<object> noteData in section.sectionNotes)
                            {
                                if (noteData != null && noteData.Count >= 2 && !section.mustHitSection)
                                {
                                    CreateNoteFromChart(Convert.ToInt32(noteData[0]), Convert.ToInt32(noteData[1]));
                                }
                                else
                                {
                                    Console.WriteLine("Invalid noteData: " + noteData);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                /*                var st = new StackTrace(e, true);
                                // Get the top stack frame
                                var frame = st.GetFrame(0);
                                // Get the line number from the stack frame
                                var line = frame.GetFileLineNumber();*/
                var penis = e.ToString();
                System.Diagnostics.Debug.WriteLine("Error loading chart: " + penis);

                
            }

            return Task.CompletedTask;
        }



        protected override void Update(GameTime gameTime)
        {
            var arrowLEFT = new[] {
            Notes.ArrowLEFT0000,
            };
            var arrowDOWN = new[] {
            Notes.ArrowDOWN0000,
            };
            var arrowUP = new[] {
            Notes.ArrowUP0000,
            };
            var arrowRIGHT = new[] {
            Notes.ArrowRIGHT0000,
            };
            var LeftConfirm = new[] {
             Notes.Left_confirm0000,
             Notes.Left_confirm0001,
             Notes.Left_confirm0002,
             Notes.Left_confirm0003,
             };
            var UpConfirm = new[] {
             Notes.Up_confirm0000,
             Notes.Up_confirm0001,
             Notes.Up_confirm0002,
             Notes.Up_confirm0003,
             };
            var DownConfirm = new[] {
             Notes.Down_confirm0000,
             Notes.Down_confirm0001,
             Notes.Down_confirm0002,
             Notes.Down_confirm0003,
             };
            var UpPress = new[] {
             Notes.Up_press0000,
             Notes.Up_press0001,
             Notes.Up_press0002,
             Notes.Up_press0003,
             };
            var LeftPress = new[] {
             Notes.Left_press0000,
             Notes.Left_press0001,
             Notes.Left_press0002,
             Notes.Left_press0003,
             };
            var DownPress = new[] {
             Notes.Down_press0000,
             Notes.Down_press0001,
             Notes.Down_press0002,
             Notes.Down_press0003,
             };
            var RightPress = new[] {
             Notes.Right_press0000,
             Notes.Right_press0001,
             Notes.Right_press0002,
             Notes.Right_press0003,
             };
            var RightConfirm = new[] {
             Notes.Right_confirm0000,
             Notes.Right_confirm0001,
             Notes.Right_confirm0002,
             Notes.Right_confirm0003,
             };
            var purple = new[] {
             Notes.Purple0000,
             };
            var green = new[] {
             Notes.Green0000,
             };
            var blue = new[] {
             Notes.Blue0000,
             };
            var red = new[] {
             Notes.Red0000,
             };
            var leftCnfrm = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, LeftConfirm);
            var UpCnfrm = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, UpConfirm);
            var DownCnfrm = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, DownConfirm);
            var RightCnfrm = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, RightConfirm);
            var leftPress = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, LeftPress);
            var upPress = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, UpPress);
            var downPress = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, DownPress);
            var rightPress = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, RightPress);
            var arrLeft = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowLEFT);
            var arrDown = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowDOWN);
            var arrUp = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowUP);
            var arrRight = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, arrowRIGHT);
            var animations = new[] { leftCnfrm };
            var animationsStat = new[] { arrLeft };
            var animationsPress = new[] { leftPress };
            var animations1 = new[] { DownCnfrm };
            var animations1Stat = new[] { arrDown };
            var animations1Press = new[] { downPress };
            var animations2 = new[] { UpCnfrm };
            var animations2Stat = new[] { arrUp };
            var animations2Press = new[] { upPress };
            var animations3 = new[] { RightCnfrm };
            var animations3Press = new[] { rightPress };
            var animations3Stat = new[] { arrRight };
            var arrPurple = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, purple);
            var arrGreen = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, green);
            var arrBlue = new Animation(
            new Vector2(0, -100), timePerFrame, SpriteEffects.None, blue);
            arrRed = new Animation(
            new Vector2(0, 0), timePerFrame, SpriteEffects.None, red);
            var strumNotePurple = new[] { arrPurple };
            var strumNoteGreen = new[] { arrGreen };
            var strumNoteBlue = new[] { arrBlue };
            strumNoteRed = new[] { arrRed };
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up))
            {
                UpNote = new AnimationManager(
                this.spriteSheet, new Vector2(420, 100), animations2Press);
            }
            if (kstate.IsKeyUp(Keys.Up))
            {
                UpNote = new AnimationManager(
                this.spriteSheet, new Vector2(420, 100), animations2Stat);
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                DownNote = new AnimationManager(
                this.spriteSheet, new Vector2(260, 100), animations1Press);
            }

            if (kstate.IsKeyUp(Keys.Down))
            {
                DownNote = new AnimationManager(
                this.spriteSheet, new Vector2(260, 100), animations1Stat);
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                leftNote = new AnimationManager(
                this.spriteSheet, new Vector2(100, 100), animationsPress);
            }
            if (kstate.IsKeyUp(Keys.A))
            {
                leftNote = new AnimationManager(
                this.spriteSheet, new Vector2(100, 100), animationsStat);
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                RightNote = new AnimationManager(
                this.spriteSheet, new Vector2(580, 100), animations3Press);
            }
            if (kstate.IsKeyUp(Keys.Right))
            {
                RightNote = new AnimationManager(
                this.spriteSheet, new Vector2(580, 100), animations3Stat);
            }
            int penis = 10;
            var not = Note.notes;
            spawnNote = new AnimationManager(
            this.spriteSheet, new Vector2(260, 400), strumNoteBlue);
            _tweener.TweenTo(target: spawnNote, expression: spawnNote => spawnNote.CurrentPosition, toValue: new Vector2(0, 100), duration: 1, delay: 1)
            .Easing(EasingFunctions.Linear);
            System.Diagnostics.Debug.WriteLine(spawnNote.CurrentPosition);
            if (kstate.IsKeyDown(Keys.F))
            {
                {
                    DownNote = new AnimationManager(
                    this.spriteSheet, new Vector2(260, 100), animations1);
                }
            }
            // TODO: Add your update logic here
            _tweener.Update(gameTime.GetElapsedSeconds());
            leftNote.Update(gameTime);
            DownNote.Update(gameTime);
            UpNote.Update(gameTime);
            RightNote.Update(gameTime);
            spawnNote.Update(gameTime);
            fps.Update(gameTime);
            base.Update(gameTime);
        }
        public void CreateNoteFromChart(int timestamp, int noteType)
        {
            List<int> validNoteTypes = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, -1 };
            if (!validNoteTypes.Contains(noteType))
            {
                Console.WriteLine("Invalid noteType: " + noteType);
                return;
            }
            var xAxis = 130 * noteType;

            var purple = new[] {
             Notes.Purple0000,
             };
            var green = new[] {
             Notes.Green0000,
             };
            var blue = new[] {
             Notes.Blue0000,
             };
            var red = new[] {
             Notes.Red0000,
             };
            var arrPurple = new Animation(
            new Vector2(100, -100), timePerFrame, SpriteEffects.None, purple);
            var arrGreen = new Animation(
            new Vector2(420, -100), timePerFrame, SpriteEffects.None, green);
            var arrBlue = new Animation(
            new Vector2(260, -100), timePerFrame, SpriteEffects.None, blue);
            var arrRed = new Animation(
            new Vector2(580, -100), timePerFrame, SpriteEffects.None, red);
            var strumNotePurple = new[] { arrPurple };
            var strumNoteGreen = new[] { arrGreen };
            var strumNoteBlue = new[] { arrBlue };
            var strumNoteRed = new[] { arrRed };

            Note note = new();
            note.Y = 100;
            note.X = 100;
            // note.SetValue(System.Windows.Controls.Canvas.LeftProperty, 130 * noteType);
            // note.SetValue(System.Windows.Controls.Canvas.TopProperty, 0);
            // note.RenderTransform = new System.Windows.Media.ScaleTransform(1.5, 1.5);

            if (noteType == -1)
            {
                System.Diagnostics.Debug.WriteLine("event fired (not really bc they arent implemented");
            }
            else
            {
               // List<string> arrowKeys = new List<string> { "a", "arrowDown", "arrowUp", "arrowRight", "a", "s", "d", "f" };
               //  string arrowKey = arrowKeys[noteType];
                // note.SetValue(System.Windows.Controls.Canvas.TagProperty, arrowKey);
                // note.Children.Add(new System.Windows.Controls.TextBlock { Text = arrowKey.ToUpper() });
            }
            GameTime gameTime = new();
            var gameTimer = TimeSpan.FromMilliseconds(timestamp);
            System.Timers.Timer timer = new();
            timer.Start();
            int penis = 10;
            gameTimer += gameTime.ElapsedGameTime;
            {
                penis += 10;
                // int gameTimeElapsed = (int)Convert.ChangeType(gameTime.ElapsedGameTime, TypeCode.Int32);
                //_tweener.TweenTo(target: spawnNote, expression: spawnNote => spawnNote.CurrentPosition, toValue: new Vector2(0, 100), duration: 1, delay: 1)
                //.Easing(EasingFunctions.Linear);
            }

            // System.Timers.Timer removeTimer = new();
            // removeTimer.Interval = gameTime.ElapsedGameTime + 1400;
            //removeTimer.Ticks += (sender, e) =>
            //{
            //      gameArea.Children.Remove(note);
            // removeTimer.Stop();
            //};
            //   removeTimer.Start();
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Draw sprites using the spriteRectangles
            _spriteBatch.Begin();
            fps.DrawFps(_spriteBatch, font, new Vector2(10f, 00f), Color.Black);
            // Draw character on screen
            spriteRender.Draw(
                leftNote.CurrentSprite,
                leftNote.CurrentPosition, Color.White, 0, 1, leftNote.CurrentSpriteEffects);
            spriteRender.Draw(
                DownNote.CurrentSprite,
                DownNote.CurrentPosition, Color.White, 0, 1, DownNote.CurrentSpriteEffects);
            spriteRender.Draw(
                UpNote.CurrentSprite,
                UpNote.CurrentPosition, Color.White, 0, 1, UpNote.CurrentSpriteEffects);
            spriteRender.Draw(
                RightNote.CurrentSprite,
                RightNote.CurrentPosition, Color.White, 0, 1, RightNote.CurrentSpriteEffects);
            spriteRender.Draw(
                spawnNote.CurrentSprite,
                spawnNote.CurrentPosition, Color.White, 0, 1, spawnNote.CurrentSpriteEffects);
            if (Note.notes != null)
            {
            spriteRender.Draw(
            Note.notes.CurrentSprite,
            Note.notes.CurrentPosition, Color.White, 0, 1, Note.notes.CurrentSpriteEffects);
            }
            NoteGroup.drawSpr(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}


namespace SimpleFpsCounter
{
    public class SimpleFps
    {
        private int frames = 0;
        private double updates = 0;
        private double elapsed = 0;
        private double last = 0;
        private double now = 0;
        public float msgFrequency = 1.0f;
        public string msg = "";

        /// <summary>
        /// The msgFrequency here is the reporting time to update the message.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            now = gameTime.TotalGameTime.TotalSeconds;
            elapsed = (now - last);
            if (elapsed > msgFrequency)
            {
                int fps = (int)(frames / elapsed);
                int elapsedInt = (int)(elapsed);
                msg = " Fps: " +fps.ToString() + "   Elapsed time: " + elapsed.ToString() +  "    Updates: " + updates.ToString();
                //Console.WriteLine(msg);
                elapsed = 0;
                frames = 0;
                updates = 0;
                last = now;
            }
            updates++;
        }

        public void DrawFps(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition, Color fpsTextColor)
        {
            spriteBatch.DrawString(font, msg, fpsDisplayPosition, fpsTextColor);
            frames++;
        }
    }
}