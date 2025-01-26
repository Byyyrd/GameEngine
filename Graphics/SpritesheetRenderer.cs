﻿using Silk.NET.OpenGL;
using System.Diagnostics.CodeAnalysis;

namespace GameEngine.Graphics
{
    internal class SpritesheetRenderer : Object
    {
        public TextureTransformation TextTransformation;
        private float _spriteTilingX, _spriteTilingY;
        public SpritesheetRenderer(GL _pGl, uint _pProgram, uint _pStride, int spritesTilingX, int spriteTilingY, float[]? _pVertices = null) : base(_pGl, _pProgram, _pStride, _pVertices)
        {
            _spriteTilingX = spritesTilingX;
            _spriteTilingY = spriteTilingY;
            _program = _pProgram;
            _vertices = new float[]{
                 // positions           // texture coords
                 0.5f,  0.5f, 0.0f,     1f/spritesTilingX , 1f                     , // top right
                 0.5f, -0.5f, 0.0f,     1f/spritesTilingX , 1.0f - 1f/spriteTilingY, // bottom right
                -0.5f, -0.5f, 0.0f,     0.0f             , 1.0f - 1f/spriteTilingY, // bottom left
                -0.5f,  0.5f, 0.0f,     0.0f             , 1f                       // top left 
            };
            NewVAO();
            NewTransformation();
        }
        public unsafe override void Render()
        {
			this.Use();
			_gl?.DrawElements(GLEnum.Triangles, 6, GLEnum.UnsignedInt, (void*)(0 * sizeof(uint)));
		}
        [MemberNotNull(nameof(TextTransformation))]
        public override void NewTransformation()
        {
            base.NewTransformation();
            TextTransformation = new TextureTransformation(_gl, _program);
            TextTransformation.Use();
        }
        public override void Use()
        {
			_gl.UseProgram(_program);
			if (_textures[SpritePosition] != null)
            {
                Transformation.Scale.X = _textures[SpritePosition].Width / OpenGl.WINDOW_WIDTH / _spriteTilingX;
                Transformation.Scale.Y = _textures[SpritePosition].Heigth / OpenGl.WINDOW_HEIGTH / _spriteTilingY;
            }
            Transformation.Use();
            TextTransformation.Use();
            _textures[SpritePosition]?.Bind();
            _vao.Bind();
			
		}
        public void NextSpriteX()
        {
            TextTransformation.Position.X += 1f / _spriteTilingX;
        }
        public void SetSpritePositionY(int index)
        {
            TextTransformation.Position.Y = -(1f / _spriteTilingY) * index;
        }
    }
}
