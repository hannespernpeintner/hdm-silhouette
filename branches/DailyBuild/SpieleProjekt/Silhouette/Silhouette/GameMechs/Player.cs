using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Physik-Engine Klassen
using FarseerPhysics.Dynamics;

using Silhouette.Engine.Manager;
using Silhouette.Engine;
using FarseerPhysics.Factories;

namespace Silhouette.GameMechs
{
    class Player : DrawableLevelObject
    {
        public AnimationWithFixture animation;

        public KeyboardState oldState;

        private bool _isRunning;    // Hannes: Statusveränderungen des Chars. Weitere folgen.
        private bool _isJumping;
        private bool _isFalling;
        private bool _isStanding;
        private bool _animationChanged;     // das brauchen wir nur für das Polygonnetz...

        public bool isRunning { get { return _isRunning; } set { _isRunning = value; } }
        public bool isJumping { get { return _isJumping; } set { _isJumping = value; } }
        public bool isFalling { get { return _isFalling; } set { _isFalling = value; } }
        public bool isStanding { get { return _isStanding; } set { _isStanding = value; } }
        public bool animationChanged { get { return _animationChanged; } set { _animationChanged = value; } }

        private int _facing;                    // Wo der Chara hinschaut. 0 bedeutet links, 1 bedeutet rechts.

        private AnimationWithFixture _activeAnimation;     // Die aktive Sprite, die in der UpdateMethode auch geupdatet wird.

        private AnimationWithFixture _running;             // In den Methoden dieser Klasse werden sie passend hintereinander geschaltet.
        private AnimationWithFixture _jumping_left;
        private AnimationWithFixture _jumping_right;
        private AnimationWithFixture _falling;
        private AnimationWithFixture _standing_left;
        private AnimationWithFixture _standing_right;

        public override void Initialise()
        {
            // Aus dem Level geht die initiale StartPos des Chars hervor. Aktuell Testposition eingetragen.
            //position = Level.LevelSetting.CharacterStartPosition;

            _standing_left = new AnimationWithFixture();
            _standing_right = new AnimationWithFixture();
            _jumping_left = new AnimationWithFixture();
            _jumping_right = new AnimationWithFixture();
            _running = new AnimationWithFixture();
            _falling = new AnimationWithFixture();

            animationChanged = false;

            position = new Vector2(1, 1);

            isRunning = false;
            isJumping = false;
            isFalling = false;
            isStanding = true;
            _facing = 0;
        }

        public override void LoadContent()
        {
            _standing_left.Load(2, "Sprites/PlayerAnimations/Standing_left/", 0.5f);
            _standing_right.Load(2, "Sprites/PlayerAnimations/Standing_right/", 0.5f);
            _jumping_left.Load(8, "Sprites/PlayerAnimations/Jumping_left/", 0.35f);
            _jumping_right.Load(8, "Sprites/PlayerAnimations/Jumping_right/", 0.35f);

            // Hier müssen alle Sprites geladen werden.

            _activeAnimation = _standing_left;
            _activeAnimation.activePolygon[0].Body.BodyType = BodyType.Dynamic;
            _activeAnimation.activePolygon[0].Body.Mass = 100;
            _activeAnimation.activePolygon[0].Body.FixedRotation = true;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateControls(gameTime);
            UpdateTexture(gameTime);
        }

        private void UpdateControls(GameTime gameTime)
        {
            //////////////////////////////////////////////////////////////////////////////////
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (_facing == 1)
                {
                    _facing = 0;

                }

                else if (!isJumping)
                {
                    if (_facing == 1)
                    {
                        _facing = 0;
                    }

                    if (_facing == 0)
                    {
                        _activeAnimation.activePolygon[0].Body.ApplyForce(new Vector2(-15, 0));
                        isStanding = !isStanding;
                        _isRunning = !isRunning;
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (_facing == 0)
                {
                    _facing = 1;
                }

                else if (!isJumping)
                {
                    if (_facing == 0)
                    {
                        _facing = 1;
                    }

                    if (_facing == 1)
                    {
                        _activeAnimation.activePolygon[0].Body.ApplyForce(new Vector2(15, 0));
                        isStanding = !isStanding;
                        _isRunning = !isRunning;
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////
            if (Keyboard.GetState().IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W))
            {
                isStanding = false;
                isRunning = false;
                isFalling = false;
                isJumping = true;

                if (_facing == 0)
                {
                    _activeAnimation = _jumping_left;
                    _activeAnimation.activePolygon[0].Body.ApplyForce(new Vector2(-50, -250));
                }
                if (_facing == 1)
                {
                    _activeAnimation = _jumping_right;
                    _activeAnimation.activePolygon[0].Body.ApplyForce(new Vector2(50, -250));
                }
            }

            if (_activeAnimation.activePolygon[0].Body.GetLinearVelocityFromLocalPoint(Vector2.Zero).X.Equals(0) || _activeAnimation.activePolygon[0].Body.GetLinearVelocityFromLocalPoint(Vector2.Zero).Y.Equals(0))
            {
                isRunning = false;
                isJumping = false;
                isFalling = false;
                isStanding = true;
                if (_facing == 0)
                {
                    _activeAnimation = _standing_left;
                }
                if (_facing == 1)
                {
                    _activeAnimation = _standing_right;
                }
            }

            oldState = Keyboard.GetState();
        }

        private void UpdateTexture(GameTime gameTime)
        {
            _activeAnimation.Update(gameTime, position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _activeAnimation.Draw(spriteBatch);
            spriteBatch.DrawString(FontManager.Arial, "Standing: " + isStanding.ToString(), new Vector2(300, 20), Color.Azure);
            spriteBatch.DrawString(FontManager.Arial, "Running: " + isRunning.ToString(), new Vector2(300, 45), Color.Azure);
            spriteBatch.DrawString(FontManager.Arial, "Jumping: " + isJumping.ToString(), new Vector2(300, 70), Color.Azure);
            spriteBatch.DrawString(FontManager.Arial, "Falling: " + isFalling.ToString(), new Vector2(300, 95), Color.Azure);
        }

        //---> Editor-Stuff, für Player unwichtig <---//

        public override LevelObject clone()
        {
            throw new NotImplementedException();
        }
        public override bool contains(Vector2 worldPosition)
        {
            throw new NotImplementedException();
        }
        public override string getPrefix()
        {
            throw new NotImplementedException();
        }
        public override void transformed()
        {
            throw new NotImplementedException();
        }
        public override void drawSelectionFrame(SpriteBatch spriteBatch, Matrix matrix)
        {
            throw new NotImplementedException();
        }
        public override bool canScale() { return false; }
        public override Vector2 getScale() { return Vector2.One; }
        public override void setScale(Vector2 scale) { }
        public override bool canRotate() { return false; }
        public override float getRotation() { return 0; }
        public override void setRotation(float rotate) { }

        public override void loadContentInEditor(GraphicsDevice graphics)
        {
            throw new NotImplementedException();
        }

        public override void drawInEditor(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
