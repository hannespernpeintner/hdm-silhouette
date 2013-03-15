using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using Microsoft.Xna.Framework.Media;

using Silhouette.GameMechs;
using Silhouette.Engine.PartikelEngine;
using Silhouette.Engine.Manager;
using Silhouette.Engine.Effects;

namespace Silhouette.Engine
{
    public enum ShaderType
    {
        None,
        Water,
        ColorChange,
        WeakBlur,
        Blur,
        StrongBlur,
        WeakBleach,
        Bleach,
        StrongBleach,
        BleachBlur
    }

    [Serializable]
    public partial class Layer
    {
        private string _name;
        [DisplayName("Name"), Category("General")]
        [Description("The name of the layer.")]
        public string name { get { return _name; } set { _name = value; } }

        private Vector2 scrollSpeed;
        [DisplayName("ScrollSpeed"), Category("General")]
        [Description("The layers scroll speed relative to the camera. A value of 1 represents the same scrolling speed as the camera" + 
        "(all collision stuff should move with the same speed as the camera!). Used for parallax scrolling.")]
        public Vector2 ScrollSpeed { get { return scrollSpeed; } set { scrollSpeed = value; } }

        private ShaderType _shaderType;
        [DisplayName("Shader"), Category("General")]
        [Description("Defines the shader which affect the whole layer.")]
        public ShaderType shaderType { get { return _shaderType; } set { _shaderType = value; } }

        private List<LevelObject> _loList;
        [DisplayName("Level Objects"), Category("Objects")]
        [Description("The objects of the Layer.")]
        public List<LevelObject> loList { get { return _loList; } }

        [NonSerialized]
        private RenderTarget2D _rt;
        public RenderTarget2D Rt { get { return _rt; } set { _rt = value; } }


        private List<EffectObject> _effects;
        public List<EffectObject> Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }
        public bool isVisible = true;

        [NonSerialized]
        [Browsable(false)]
        public ParticleRenderer particleRenderer;

        public Layer()
        {
            scrollSpeed = Vector2.One;
            _loList = new List<LevelObject>();
            shaderType = ShaderType.None;
        }

        public void initializeLayer() { }

        public void loadLayer()
        {
            particleRenderer = new ParticleRenderer();
            Rt = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);

            foreach (LevelObject lo in loList)
            {
                lo.LoadContent();

                if (lo is ParticleObject)
                {
                    ParticleObject p = (ParticleObject)lo;
                    particleRenderer.addParticleObjects(p);
                }
            }

            particleRenderer.initializeParticles();

            Effects = new List<EffectObject>();
            //Effects.Add(e0);

            foreach (EffectObject eo in Effects)
            {
                eo.Initialise();
                eo.LoadContent();
            }
        }
        public void loadLayerInEditor()
        {
            particleRenderer = new ParticleRenderer();
            Rt = new RenderTarget2D(GameLoop.gameInstance.GraphicsDevice, GameSettings.Default.resolutionWidth, GameSettings.Default.resolutionHeight);

            foreach (LevelObject lo in loList)
            {
                lo.LoadContent();

                if (lo is ParticleObject)
                {
                    ParticleObject p = (ParticleObject)lo;
                    particleRenderer.addParticleObjects(p);
                }
            }

            particleRenderer.initializeParticles();

            Effects = new List<EffectObject>();
            //Effects.Add(e0);

            foreach (EffectObject eo in Effects)
            {
                eo.Initialise();
                eo.LoadContent();
            }
        }

        public void updateLayer(GameTime gameTime)
        {
            foreach (LevelObject lo in loList)
            {
                lo.Update(gameTime);
            }

            particleRenderer.updateParticles(gameTime);


            foreach (EffectObject eo in Effects)
            {
                eo.Update(gameTime);
            }
        }

        public void drawLayer(SpriteBatch spriteBatch)
        {
            foreach (LevelObject lo in loList)
            {
                if (lo is DrawableLevelObject)
                {
                    DrawableLevelObject dlo = (DrawableLevelObject)lo;

                    if (dlo.isVisible)
                        dlo.Draw(spriteBatch);
                }
            }

            particleRenderer.drawParticles();
        }

        public void drawLayerInEditor(SpriteBatch spriteBatch)
        {
            foreach (LevelObject lo in loList)
            {
                if (lo is DrawableLevelObject)
                {
                    DrawableLevelObject dlo = (DrawableLevelObject)lo;

                    if (dlo.isVisible)
                        dlo.Draw(spriteBatch);
                }
            }

            particleRenderer.drawParticles();
        }

        public Effect getShaderByType(ShaderType shaderType)
        {
            switch (shaderType)
            {
                case ShaderType.None:
                    return null;
                case ShaderType.Water:
                    return EffectManager.Water();
                case ShaderType.ColorChange:
                    return EffectManager.ColorChange();
                case ShaderType.WeakBleach:
                    return EffectManager.WeakBleach();
                case ShaderType.Bleach:
                    return EffectManager.Bleach();
                case ShaderType.StrongBleach:
                    return EffectManager.StrongBleach();
                case ShaderType.WeakBlur:
                    return EffectManager.WeakBlurrer();
                case ShaderType.Blur:
                    return EffectManager.Blurrer();
                case ShaderType.StrongBlur:
                    return EffectManager.StrongBlurrer();
                case ShaderType.BleachBlur:
                    return EffectManager.BleachBlur();

                default:
                    return null;
            }
        }
        public Effect getShaderByTypeInEditor(ShaderType shaderType, GraphicsDevice graphics)
        {
            switch (shaderType)
            {
                case ShaderType.None:
                    return null;
                case ShaderType.Water:
                    return EffectManager.WaterInEditor(graphics);
                case ShaderType.ColorChange:
                    return EffectManager.ColorChange();
                case ShaderType.WeakBleach:
                    return EffectManager.WeakBleachInEditor(graphics);
                case ShaderType.Bleach:
                    return EffectManager.BleachInEditor(graphics);
                case ShaderType.StrongBleach:
                    return EffectManager.StrongBleachInEditor(graphics);
                case ShaderType.WeakBlur:
                    return EffectManager.WeakBlurrerInEditor(graphics);
                case ShaderType.Blur:
                    return EffectManager.BlurrerInEditor(graphics);
                case ShaderType.StrongBlur:
                    return EffectManager.StrongBlurrerInEditor(graphics);
                case ShaderType.BleachBlur:
                    return EffectManager.BleachBlurInEditor(graphics);

                default:
                    return null;
            }
        }

        public BlendState getBlendStateByEffect(ShaderType shaderType) 
        {
            switch (shaderType)
            {
                case ShaderType.WeakBleach:
                    return BlendState.NonPremultiplied;
                case ShaderType.Bleach:
                    return BlendState.NonPremultiplied;
                case ShaderType.StrongBleach:
                    return BlendState.NonPremultiplied;
                case ShaderType.BleachBlur:
                    return BlendState.NonPremultiplied;

                default:
                    return null;
            }
        }
    }
}
