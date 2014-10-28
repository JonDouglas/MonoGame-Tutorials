using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wizardry
{
    public class Sprite
    {
        public Vector2 Position = new Vector2(0, 0);

        private Texture2D mSpriteTexture;
        public string AssetName;
        public Rectangle Size;
        private float mScale = 1.0f;

        public void LoadContent(ContentManager contentManager, string assetName)
        {
            mSpriteTexture = contentManager.Load<Texture2D>(assetName);
            AssetName = assetName;
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mSpriteTexture, Position, new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            Position += direction*speed*(float) gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
