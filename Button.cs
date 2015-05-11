using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.GUI
{
    class Button : Sprite
    {
        enum ButtonStatus
        {
            Normal, Pressed, Hovered, Disabled
        }

        private bool enabled = true;
        private bool hoverState = false;
        private MouseState oldMouseState;
        private Texture2D HoverTexture = null;
        private Texture2D PressedTexture = null;
        private Rectangle bounds = Rectangle.Empty;
        private ButtonStatus status = ButtonStatus.Normal;
        public event EventHandler OnMouseDown = null;
        public event EventHandler OnMouseClick = null;
        public event EventHandler OnMouseHover = null;
        public event EventHandler OnMouseLeave = null;


        public Button(Texture2D texture, Texture2D pressedTexture, Texture2D hoverTexture, Vector2 position)
            : base(texture, position)
        {
            this.PressedTexture = pressedTexture;
            this.HoverTexture = hoverTexture;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public void SetScale(Vector2 position, float Scale)
        {
            this.pos = position;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
        }

        public Rectangle Bounds
        {
            get { return this.bounds; }
            set
            {
                this.pos = new Vector2(value.X, value.Y);
                this.bounds = value;
            }
        }

        public override void Update(GameTime gametime)
        {
            if (!this.enabled)
            {
                this.status = ButtonStatus.Disabled;
                return;
            }

            MouseState state = Mouse.GetState();
            bool hovered = bounds.Contains(state.X, state.Y);

            if (this.hoverState && !hovered)
            {
                if (this.OnMouseLeave != null)
                {
                    this.OnMouseLeave(this, EventArgs.Empty);
                }
            }

            if (hovered && this.status != ButtonStatus.Pressed)
            {
                this.status = ButtonStatus.Hovered;
                if (this.OnMouseHover != null)
                {
                    this.OnMouseHover(this, EventArgs.Empty);
                }
            }
            else if (!hovered && this.status != ButtonStatus.Pressed)
            {
                this.status = ButtonStatus.Normal;
            }

            if (state.LeftButton == ButtonState.Pressed && this.oldMouseState.LeftButton == ButtonState.Released)
            {
                if (hovered)
                {
                    this.status = ButtonStatus.Pressed;
                    if (this.OnMouseDown != null)
                    {
                        this.OnMouseDown(this, EventArgs.Empty);
                    }
                }
            }

            if (state.LeftButton == ButtonState.Released && this.oldMouseState.LeftButton == ButtonState.Pressed)
            {
                if (hovered)
                {
                    this.status = ButtonStatus.Hovered;
                    if (this.OnMouseClick != null)
                    {
                        this.OnMouseClick(this, EventArgs.Empty);
                    }
                }
            }
            else if (this.status == ButtonStatus.Pressed && !hovered)
            {
                this.status = ButtonStatus.Normal;
            }
            else if (this.status == ButtonStatus.Pressed)
            {
                this.status = ButtonStatus.Pressed;
            }

            this.oldMouseState = state;
            this.hoverState = hovered;
        }

        public override void Draw(SpriteBatch batch)
        {
            this.Draw(batch, Color.White);
        }

        public void Draw(SpriteBatch batch, Color tint, Color disabledColor)
        {
            switch (this.status)
            {
                case ButtonStatus.Disabled:
                    batch.Draw(this.texture, this.bounds, disabledColor);
                    break;

                case ButtonStatus.Normal:
                    batch.Draw(this.texture, this.bounds, tint);
                    break;

                case ButtonStatus.Pressed:
                    batch.Draw(this.PressedTexture == null ? this.texture : this.PressedTexture, this.bounds, tint);
                    break;

                case ButtonStatus.Hovered:
                    batch.Draw(this.HoverTexture == null ? this.texture : this.HoverTexture, this.bounds, tint);
                    break;

                default:
                    batch.Draw(this.texture, this.bounds, tint);
                    break;
            }
        }

        public override void Draw(SpriteBatch batch, Color tint)
        {
            this.Draw(batch, tint, Color.Gray);
        }
    }
}

