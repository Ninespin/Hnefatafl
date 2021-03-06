﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Hnefatafl {
    class BaseShaderProgram : CustomShaderProgram{

        public BaseShaderProgram(string vertpath, string fragpath): base(vertpath, fragpath, "BaseShader") {
            this.vbos = new int[3];

            this.Init();
            
        }

        

        public override void InitVariablePipe() {

            variablePipe.Add("in_position", 0);
            GL.BindAttribLocation( this.id, 0, "in_position" );
            variablePipe.Add("in_uv", 1);
            GL.BindAttribLocation(this.id, 1, "in_uv");
             variablePipe.Add("in_normal", 2);
             GL.BindAttribLocation(this.id, 2, "in_normal");
             

            variablePipe.Add( "pvm", GL.GetUniformLocation( this.id, "pvm") );
            variablePipe.Add( "sampler", GL.GetUniformLocation( this.id, "sampler" ) );

        }


        public override void Init() {
            ShaderManager.LinkProgram(this);
            GL.UseProgram(this.id);
            InitVariablePipe();

        }

        public override void SetPVMMatrix( ref Matrix4 pvm ) {
            GL.UniformMatrix4( variablePipe["pvm"], false, ref pvm );

        }

        public override void LoadUniforms( ref GameObject renderedObject) {
            GL.Uniform1( variablePipe["sampler"], 0f );

        }

        public override void Prepare(Mesh mesh, int VBO, int TBO, int NBO) {
            GL.UseProgram(this.id);

            GL.BindBuffer(BufferTarget.ArrayBuffer, TBO);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, mesh.TextureHandle);
            GL.TexCoordPointer(2,
                TexCoordPointerType.Float,
                Vector2.SizeInBytes,
                0
                );
            GL.VertexAttribPointer(this.VariablePipe["in_uv"], 2, VertexAttribPointerType.Float, true, 0, 0);
            GL.EnableVertexAttribArray(this.VariablePipe["in_uv"]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, NBO);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.NormalPointer(
                NormalPointerType.Float,
                Vector3.SizeInBytes,
                0
                );
            GL.VertexAttribPointer(this.VariablePipe["in_normal"], 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(this.VariablePipe["in_normal"]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3,
                VertexPointerType.Float,
                Vector3.SizeInBytes,
                0
                );
            GL.VertexAttribPointer(this.VariablePipe["in_position"], 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(this.VariablePipe["in_position"]);
           



        }

        public override void GenerateVBOs() {
            GL.GenBuffers(3, this.vbos);
            
        }

        public override void InitVBOs(Mesh mesh) {
 
            

        }


        public override void EndRender() {
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.Disable(EnableCap.Texture2D);

            GL.DisableVertexAttribArray(this.VariablePipe["in_position"]);
            GL.DisableVertexAttribArray(this.VariablePipe["in_normal"]);
            GL.DisableVertexAttribArray( this.VariablePipe["in_uv"] );

        }

        public override void Cleanup() {
            this.EndRender();

            GL.DeleteProgram(this.ID);

            GL.DeleteVertexArray(this.vao);
            GL.DeleteBuffers(this.vbos.Length, this.vbos);
        }

    }


}
