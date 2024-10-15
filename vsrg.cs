using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Murlaff's Vsrg
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // List of notes
        private List<Note> notes;
        private Texture2D noteTexture;
        private Vector2 hitArea;
        private float noteSpeed = 400f; // Speed of notes falling

        // Input
        private KeyboardState currentKey;
        private KeyboardState previousKey;

        // Scoring
        private int score;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set up window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            // Initialize hit area (bottom of the screen)
            hitArea = new Vector2(100, 500);

            // Initialize notes list
            notes = new List<Note>();

            // Add notes with their lanes and spawn times
            notes.Add(new Note() { Position = new Vector2(100, -100), Lane = Keys.A, SpawnTime = 1f });
            notes.Add(new Note() { Position = new Vector2(200, -100), Lane = Keys.S, SpawnTime = 2f });
            notes.Add(new Note() { Position = new Vector2(300, -100), Lane = Keys.D, SpawnTime = 3f });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load note texture
            noteTexture = Content.Load<Texture2D>("note");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentKey = Keyboard.GetState();

            // Update notes' positions and check for input
            foreach (var note in notes)
            {
                // Update note position based on speed
                note.Position.Y += noteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if note reached hit area
                if (note.Position.Y >= hitArea.Y && note.Position.Y <= hitArea.Y + 50)
                {
                    if (IsKeyPressed(note.Lane))
                    {
                        score += 100; // Perfect hit!
                        note.IsHit = true; // Mark the note as hit
                    }
                }

                // Remove missed notes
                if (note.Position.Y > 600 && !note.IsHit)
                {
                    score -= 50; // Miss penalty
                    note.IsHit = true; // Mark the note as hit to avoid further checks
                }
            }

            previousKey = currentKey;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw the notes
            foreach
