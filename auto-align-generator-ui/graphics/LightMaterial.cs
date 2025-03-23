using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.ui.graphics
{
    public class LightMaterial : Material
    {
        public LightMaterial() : base(new Shader("assets/shaders/vertex.glsl", "assets/shaders/light.glsl"))
        {
        }
    }
}
