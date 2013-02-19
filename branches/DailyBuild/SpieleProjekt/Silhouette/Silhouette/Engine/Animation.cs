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
using FarseerPhysics.Dynamics;
using Silhouette.Engine.Manager;
using System.IO;
using Silhouette.GameMechs;

namespace Silhouette.Engine
{
    [Serializable]
    public class Animation:DrawableLevelObject
    {
        /* Hannes: 
        Da wir den Plan verworfen haben mehrere Einzelbilder in ein Spritebild zu packen, weil wir dann das zugehörige Polygon
        nicht mehr berechnet bekommen, hier eine neue Version für eine AnimatedSprite, der Kürze Halber Animation genannt. Es
        werden Einzelbilder verwendet, die zu ner Liste hinzugefügt werden. Eine Animation wird geladen, indem man beim
        Aufrufen von Load den Ordnerpfad angibt, sowie die Bilderzahl (!). Es werden nach und nach die durchnummerierten (!)
        Bilder reingeladen. Abgespielt wird automatisch und endlos. Die Animation ist beweglich.
        */

        enum AnimationState { 
            Play,
            Pause,
            Stop
        }

        private AnimationState state;
        private AnimationState State
        {
            get { return state; }
            set { state = value; }
        }

        public List<Texture2D> pictures;                    // Liste mit Einzelbildern, Nummer des gerade aktiven Bildes,
        public int activeFrameNumber;                       // zur Sicherheit auch das aktive Bild selber, können wir später
        public Texture2D activeTexture;                     // rauslöschen, wenn keine weitere Verwendung, auÃ erdem framespersecond
        private float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        //public Vector2 position;
        public float rotation;
        public Vector2 scale;

        public override Vector2 getScale() { return scale; }
        public override float getRotation() { return rotation; }

        private bool looped;
        public bool Looped
        {
            get { return looped; }
            set { looped = value; }
        }

        private bool backwards;
        public bool Backwards
        {
            get { return backwards; }
            set { backwards = value; }
        }

        private bool pingpong;
        public bool Pingpong
        {
            get { return pingpong; }
            set { pingpong = value; }
        }

        private bool playedOnce;
        public bool PlayedOnce
        {
            get { return playedOnce; }
            set { playedOnce = value; }
        }

        private String fullpath;
        public String Fullpath
        {
            get { return fullpath; }
            set { fullpath = value; }
        }

        private float totalElapsed;
        public float TotalElapsed
        {
            get { return totalElapsed; }
            set { totalElapsed = value; }
        }

        private int amount;
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Animation()
        {
            pictures = new List<Texture2D>();
            totalElapsed = 0;
            playedOnce = false;
            pingpong = false;
            totalElapsed = 0;
            amount = 0;
            looped = false;
            scale = new Vector2(1, 1);
            rotation = 0;
            State = AnimationState.Play;
        }

        public Animation(String fullpath, int amount, float speed)
        {
            // Der fullpath muss bis zum Unterstrich vor der Zahl angegeben werden!!
            pictures = new List<Texture2D>();
            totalElapsed = 0;
            this.playedOnce = false;
            this.fullpath = fullpath;
            this.amount = amount;
            this.Speed = speed;
            this.looped = true;
            this.position = Vector2.Zero;
        }

        public override void Initialise()
        {
            
        }

        // Wird in der Load des zugehörigen Trägers gerufen
        // speed sind Bilder pro Sekunde. Also irgendeine Integerahl
        public void Load(int amount, String path, float speed, bool looped)
        {
            this.Speed = speed;
            this.amount = amount;
            this.position = Vector2.Zero;
            this.looped = looped;

            //Achtung, Zählung beginnt bei den AMlern mit 01, nicht 00!!!!!
            for (int i = 1; i <= amount; i++)
            {
                if (i < 10)
                {
                    String temp = (path + "0" + i).ToString();
                    pictures.Add(GameLoop.gameInstance.Content.Load<Texture2D>(temp));
                }
                else
                {
                    String temp = (path + i).ToString();
                    pictures.Add(GameLoop.gameInstance.Content.Load<Texture2D>(temp));
                }
            }
            activeFrameNumber = 0;
            activeTexture = pictures[activeFrameNumber];
        }

        public override void loadContentInEditor(GraphicsDevice graphics)
        {
            Load();
        }

        public override void LoadContent()
        {
            Load();
        }

        public void Load()
        {
            String pathCut = Fullpath;

            if (pathCut.EndsWith(".png"))
            {
                pathCut = pathCut.Replace(".png", "");
            }
            if (pathCut.EndsWith("1"))
            {
                pathCut = pathCut.Remove(pathCut.Length - 1);
            }
            if (pathCut.EndsWith("0"))
            {
                pathCut = pathCut.Remove(pathCut.Length - 1);
            }

            int max = 100;

            for (int i = 1; i < max; i++)
            {
                if (i < 10)
                {
                    String temp = (pathCut + "0" + i + ".png").ToString();
                    try 
                    {
                        Texture2D t = GameLoop.gameInstance.Content.Load<Texture2D>(temp);
                        if (t != null)
                        {
                            pictures.Add(t);
                        }
                    } catch (Exception e)
                    {
                        Texture2D t = TextureManager.Instance.LoadFromFile(temp);
                        if (t != null)
                        {
                            pictures.Add(t);
                        }
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    String temp = (pathCut + i + ".png").ToString();
                    try 
                    {
                        Texture2D t = GameLoop.gameInstance.Content.Load<Texture2D>(temp);
                        if (t != null)
                        {
                            pictures.Add(t);
                        }
                    }
                    catch (Exception e)
                    {
                        Texture2D t = TextureManager.Instance.LoadFromFile(temp);
                        if (t != null)
                        {
                            pictures.Add(t);
                        }
                        Console.WriteLine(e.Message);
                    }
                }
            }

            if(pictures.Count == 0)
            {
                try
                {
                    pictures.Add(GameLoop.gameInstance.Content.Load<Texture2D>(fullpath));
                }
                catch (Exception e)
                {
                    pictures.Add(TextureManager.Instance.LoadFromFile(fullpath));
                }
            }

            activeFrameNumber = 0;
            //looped = true;
            activeTexture = pictures[activeFrameNumber];
            //State = AnimationState.Play;
        }

        //Braucht die position des Trägers!
        public override void Update(GameTime gameTime)
        {

            float fps = 1f / Speed;
            float mspf = 1000f / Speed;

            switch(State)
            {
                case AnimationState.Play:
                {
                    float elapsed = gameTime.ElapsedGameTime.Milliseconds;
                    totalElapsed += elapsed;

                    if (totalElapsed > mspf)
                    {
                        totalElapsed -= mspf;
                        switchToNextFrame();
                    }
                    break;
                }
                case AnimationState.Stop:
                {

                    break;
                }
                case AnimationState.Pause:
                {

                    break;
                }
            }
                
            activeTexture = pictures[activeFrameNumber];
        }

        public void UpdateTransformation(Vector2 position, float rotation, Vector2 scale)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }

        public void UpdatePosition(Vector2 position)
        {
            this.position = position;
        }
        
        private void switchToNextFrame()
        {
            switch (backwards)
            {
                case false:
                    switch (looped)
                    {
                        case false:
                            switch (pingpong)
                            {
                                case false:
                                    // vorwärts, nonlooped, nonpingpong
                                    if (activeFrameNumber == pictures.Count - 1)
                                    {
                                        playedOnce = true;
                                        State = AnimationState.Pause;
                                    }
                                    else
                                    {
                                        activeFrameNumber += 1;
                                    }
                                    break;
                                case true:
                                    // vorwärts, nonlooped, pingpong
                                    if (activeFrameNumber == pictures.Count - 1)
                                    {
                                        backwards = true;
                                    }
                                    else
                                    {
                                        activeFrameNumber += 1;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (pingpong)
                            {
                                case false:
                                    // vorwärts, looped, nonpingpong
                                    if (activeFrameNumber == pictures.Count - 1)
                                    {
                                        playedOnce = true;
                                        activeFrameNumber = 0;
                                    }
                                    else
                                    {
                                        activeFrameNumber += 1;
                                    }
                                    break;
                                case true:
                                    // vorwärts, looped, pingpong
                                    if (activeFrameNumber == pictures.Count - 1)
                                    {
                                        backwards = true;
                                    }
                                    else
                                    {
                                        activeFrameNumber += 1;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case true:
                    switch (looped)
                    {
                        case false:
                            switch (pingpong)
                            {
                                case false:
                                    // rückwärts, nonlooped, nonpingpong
                                    if (activeFrameNumber == 0)
                                    {
                                        playedOnce = true;
                                        State = AnimationState.Pause;
                                    }
                                    else
                                    {
                                        activeFrameNumber -= 1;
                                    }
                                    break;
                                case true:
                                    // rückwärts, nonlooped, pingpong
                                    if (activeFrameNumber == 0)
                                    {
                                        backwards = false;
                                    }
                                    else
                                    {
                                        activeFrameNumber -= 1;
                                    }
                                    break;
                            }
                            break;
                        case true:
                            switch (pingpong)
                            {
                                case false:
                                    // rückwärts, looped, nonpingpong
                                    if (activeFrameNumber == 0)
                                    {
                                        playedOnce = true;
                                        activeFrameNumber = pictures.Count - 1;
                                    }
                                    else
                                    {
                                        activeFrameNumber -= 1;
                                    }
                                    break;
                                case true:
                                    // rückwärts, looped, pingpong
                                    if (activeFrameNumber == 0)
                                    {
                                        backwards = false;
                                    }
                                    else
                                    {
                                        activeFrameNumber -= 1;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }

            activeTexture = pictures[activeFrameNumber];
        }

        //Nevermind!
        public void Update2(GameTime gameTime, Vector2 position)
        {
            this.position = position;

            switch (State)
            {
                case AnimationState.Play:
                    {
                        float elapsed = gameTime.ElapsedGameTime.Milliseconds;
                        totalElapsed += elapsed;

                        if (totalElapsed > Speed)
                        {
                            totalElapsed -= Speed;
                            switchToNextFrame();
                        }
                        break;
                    }
                case AnimationState.Stop:
                    {

                        break;
                    }
                case AnimationState.Pause:
                    {

                        break;
                    }
            }

            activeTexture = pictures[activeFrameNumber];
        }

        // Wird in der Draw des Trägers gerufen
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2((float)(pictures[0].Width / 2), (float)(pictures[0].Height / 2));
            //spriteBatch.Draw(activeTexture, position, new Rectangle((int)position.X, (int)position.Y, activeTexture.Width, activeTexture.Height), new Rectangle(0, 0, activeTexture.Width, activeTexture.Height), Color.White, getRotation, origin, getScale(), SpriteEffects.None, 1);
            spriteBatch.Draw(activeTexture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 1);
        }

        public void start()
        {
            State = AnimationState.Play;
        }

        public void stop()
        {
            resetAnimationProgress();
            State = AnimationState.Stop;
        }

        private void resetAnimationProgress()
        {
            totalElapsed = 0;
            if (backwards)
            {
                activeFrameNumber = pictures.Capacity - 1;
            }
            else
            {
                activeFrameNumber = 0;
            }
        }

        public void pause()
        {
            State = AnimationState.Pause;
        }
    }
}