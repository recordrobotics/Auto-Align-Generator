﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.ui.graphics
{
    public class Mesh : IDisposable
    {
        public Mesh(float[] vertices, uint[] indices, List<Texture> textures, int material)
        {
            Material = material;
            Vertices = vertices;
            Indices = indices;
            Textures = textures;

            EBO = new BufferObject<uint>(Indices, BufferTargetARB.ElementArrayBuffer, false);
            VBO = new BufferObject<float>(Vertices, BufferTargetARB.ArrayBuffer, false);
            VAO = new VertexArrayObject<float, uint>(VBO, EBO);
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 14, false, 0 * sizeof(float));
            VAO.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 14, false, 3 * sizeof(float));
            VAO.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, 14, false, 6 * sizeof(float));
            VAO.VertexAttributePointer(3, 2, VertexAttribPointerType.Float, 14, false, 9 * sizeof(float));
            VAO.VertexAttributePointer(4, 3, VertexAttribPointerType.Float, 14, false, 11 * sizeof(float));
        }

        public int Material { get; }
        public float[] Vertices { get; private set; }
        public uint[] Indices { get; private set; }
        public IReadOnlyList<Texture>? Textures { get; private set; }
        public VertexArrayObject<float, uint> VAO { get; set; }
        public BufferObject<float> VBO { get; set; }
        public BufferObject<uint> EBO { get; set; }

        public void Bind()
        {
            VAO.Bind();
            EBO.Bind();
        }

        public void Dispose()
        {
            Textures = null;
            VAO.Dispose();
            VBO.Dispose();
            EBO.Dispose();
        }
    }
}
