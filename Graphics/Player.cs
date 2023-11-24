using Silk.NET.OpenGL;
using System.Diagnostics.CodeAnalysis;

namespace GameEngine.Graphics
{
    internal class Player : Object
    {
        private readonly Texture[] _textures;
        public uint SpritePosition;
        public TextureTransformation TextTransformation;
        public Player(GL _pGl, uint _pProgram, uint _stride, int _spriteAmount, float[]? _vertices = null) : base(_pGl, _pProgram, _stride, _vertices)
        {
            _textures = new Texture[_spriteAmount];
            NewTransformation();
        }
        public override void NewTexture(uint _textureLoc, string path)
        {
            _textures[_textureLoc] = new Texture(_gl, _program, _stride, path);
            _textures[_textureLoc].Use(_textureLoc + 1);
            _textures[_textureLoc].CreateTexture();
            if (_textures[_textureLoc] != null)
            {
                Transformation.Scale.X = _textures[_textureLoc].Width / 800f;
                Transformation.Scale.Y = _textures[_textureLoc].Heigth / 800f;
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
                Transformation.Scale.X = _textures[SpritePosition].Width / 800f / 10f;
                Transformation.Scale.Y = _textures[SpritePosition].Heigth / 800f / 8f;
            }
            Transformation.Use();
            TextTransformation.Use();
            _textures[SpritePosition].Bind();
            _vao.Bind();
        }
    }
}
