﻿using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.ui.graphics
{
    public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        private uint _handle;

        public VertexArrayObject(BufferObject<TVertexType> vbo, BufferObject<TIndexType> ebo)
        {
            //Setting out handle and binding the VBO and EBO to this VAO.
            _handle = Window.GL.GenVertexArray();
            Bind();
            vbo.Bind();
            ebo.Bind();
        }

        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, bool normalized, int offSet)
        {
            //Setting up a vertex attribute pointer
            Window.GL.VertexAttribPointer(index, count, type, normalized, vertexSize * (uint)sizeof(TVertexType), (void*)(offSet));
            Window.GL.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            //Binding the vertex array.
            Window.GL.BindVertexArray(_handle);
        }

        public void Unbind()
        {
            //Binding the vertex array.
            Window.GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            //Remember to dispose this object so the data GPU side is cleared.
            //We dont delete the VBO and EBO here, as you can have one VBO stored under multiple VAO's.
            Window.GL.DeleteVertexArray(_handle);
        }
    }
}
