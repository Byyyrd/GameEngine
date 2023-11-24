using Silk.NET.OpenGL;
using System.Diagnostics.CodeAnalysis;

namespace GameEngine.Graphics
{
    internal class SpritesheetRenderer : Object
    {
        private Texture[] _textures;
        public uint SpritePosition;
        public TextureTransformation TextTransformation;
        private float _spriteTilingX, _spriteTilingY;
        public SpritesheetRenderer(GL _pGl, uint _pProgram, uint _pStride, int _spriteAmount, int spritesTilingX, int spriteTilingY, float[]? _pVertices = null) : base(_pGl, _pProgram, _pStride, _pVertices)
        {
            _spriteTilingX = spritesTilingX;
            _spriteTilingY = spriteTilingY;
            _textures = new Texture[_spriteAmount];
            _vertices = new float[]{
            // positions          // texture coords
             0.5f,  0.5f, 0.0f,     1f/spritesTilingX , 1f                     , // top right
             0.5f, -0.5f, 0.0f,     1f/spritesTilingX , 1.0f - 1f/spriteTilingY, // bottom right
            -0.5f, -0.5f, 0.0f,     0.0f             , 1.0f - 1f/spriteTilingY, // bottom left
            -0.5f,  0.5f, 0.0f,     0.0f             , 1f                       // top left 
            };
            NewVAO();
            NewTransformation();
        }
        public override void NewTexture(uint _textureLoc, string path)
        {
            _textures[_textureLoc] = new Texture(_gl, _program, _stride, path);
            _textures[_textureLoc].Use(_textureLoc + 1);
            _textures[_textureLoc].CreateTexture();
            if (_textures[_textureLoc] != null)
            {
                Transformation.Scale.X = _textures[_textureLoc].Width / OpenGl.WINDOW_WIDTH;
                Transformation.Scale.Y = _textures[_textureLoc].Heigth / OpenGl.WINDOW_HEIGTH;
            }
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
            if (_textures[SpritePosition] != null)
            {
                Transformation.Scale.X = _textures[SpritePosition].Width / OpenGl.WINDOW_WIDTH / _spriteTilingX;
                Transformation.Scale.Y = _textures[SpritePosition].Heigth / OpenGl.WINDOW_HEIGTH / _spriteTilingY;
            }
            Transformation.Use();
            TextTransformation.Use();
            _textures[SpritePosition].Bind();
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
