using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;



namespace test1
{
    static class Constants
    {
        public const string path = "C:/Users/Jimmy/Downloads/Open/Open/test1/shader/";
        public const string obj = "C:/Users/Jimmy/Downloads/Open/Open/OBJModel/";
    }
    internal class Window : GameWindow
    {
        private readonly List<Vector3> _pointLightPositions = new List<Vector3>()
        {
            new Vector3(-3.24f, 1.80f, 3.24f),
            new Vector3(5.23f, 1.80f, 3.24f),
            new Vector3(-3.24f,1.80f, -5.23f),
            new Vector3(5.23f, 1.80f, -5.23f)
        };
        private readonly List<Vector3> point_light_color_difuse = new List<Vector3>()
    {
           
            new Vector3(0.3f, 0.3f, 0.3f),
            new Vector3(0.3f, 0.3f, 0.3f),
            new Vector3(0.3f, 0.3f, 0.3f),
            
            new Vector3(1f, 1f, 1f)
            
        };
        int Count=0;
        Asset3d[] _object3d = new Asset3d[9];
        double _time;
        float degr = 0;
        Camera _camera;
        bool _firstMove = true;
        Vector2 _lastPos;
        Vector3 _objecPost = new Vector3(0.0f, 0.0f, 0.0f);
        float _rotationSpeed = 1f;
        float _intime;
        Asset3d LightObject = new Asset3d();
        Asset3d[] cahaya = new Asset3d[8];
        bool cameraFree = false;
        bool drop = false;
        bool stPerson = false;
        
        bool rott = false;
        int counroot;
        int coundrop;
        int countSt;
        int countRd = 1;
        int countpl;
       
        int num;
        
        bool play;


        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        public Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            var secretFormula = new float[4, 4] {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatix = new Matrix4
            (
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );

            return secretFormulaMatix;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            //ganti background
            GL.ClearColor(0.69f, 0.84f, 0.85f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            for (int i = 0; i < 4; i++)
            {
                cahaya[i] = new Asset3d();
                cahaya[i].createBoxVertices2(_pointLightPositions[i].X, _pointLightPositions[i].Y, _pointLightPositions[i].Z, 0.2f);
                cahaya[i].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);
            }
            cahaya[4] = new Asset3d();
            cahaya[4].createBoxVertices2(0,1.78f,0f, 0.2f);
            cahaya[4].load(Constants.path + "shader.vert", Constants.path + "shader1.frag", Size.X, Size.Y);
            cahaya[5] = new Asset3d();
            _object3d[0] = new Asset3d();
            _pointLightPositions.Add(cahaya[0]._centerPosition);
            point_light_color_difuse.Add(new Vector3(0.0f, 1, 1));
            //_object3d[0].createEllipsoid2(0.2f, 0.2f, 0.2f, 0.0f, 0.0f, 0.0f, 72, 24);
            //_object3d[0].createEllipsoid(0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 0.0f);
            _object3d[0].addChild(0.5f, 0.5f, 0.0f, 0.001f, 0.001f, 0.001f);
            _object3d[0].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[1] = new Asset3d();
            _object3d[2] = new Asset3d();
            _object3d[3] = new Asset3d();
            _object3d[4] = new Asset3d();
            _object3d[5] = new Asset3d();
            _object3d[6] = new Asset3d();
            _object3d[7] = new Asset3d();
            _object3d[8] = new Asset3d();
            _object3d[1].readFileOBJ(Constants.obj + "bocil.obj");
            _object3d[1].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[2].readFileOBJ(Constants.obj + "donat.obj");
            _object3d[2].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[3].readFileOBJ(Constants.obj + "backpack-backpack.obj");
            _object3d[3].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[4].createBoxVertices2(1, -5.16f, -1,10f);
            _object3d[4].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[7].readFileOBJ(Constants.obj + "bocil1.obj");
            _object3d[7].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[8].readFileOBJ(Constants.obj + "donat1.obj");
            _object3d[8].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            //_object3d[4].readFileOBJ(Constants.obj + "KK.obj");
            //_object3d[4].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[1].translate(-1.1f, -0.155f, -0.5f);
            _object3d[1].scale(5f, _object3d[1]._centerPosition);
            _object3d[7].scale(10f, _object3d[1]._centerPosition);
            _object3d[3].scale(5f, _object3d[3]._centerPosition);
            _object3d[3].translate(-1f, -0.045f, 0);
            _object3d[2].translate(-1f, 0, 0);
            _object3d[5].createBoxVertices3(1f, 2.25f, -1f, 10f, 0.5f, 10f);
            _object3d[5].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[6].createBoxVertices3(-3.74f, 0.59f, 3.74f, 0.5f, 3f, 0.5f);
            _object3d[6].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[6].addChild(5.73f, 0.59f, 3.74f, 0.5f, 3f, 0.5f);
            _object3d[6].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[6].addChild(-3.74f, 0.59f, -5.73f, 0.5f, 3f, 0.5f);
            _object3d[6].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[6].addChild(5.73f, 0.59f, -5.73f, 0.5f, 3f, 0.5f);
            _object3d[6].load(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[2]._euler[2], 90);
            _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[2], 90);

            
            _camera = new Camera(new Vector3(0, 1, 4f), Size.X / Size.Y);
            _camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
            CursorGrabbed = true;
            
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            Count += 1;
            base.OnRenderFrame(args);
            _time = (float)args.Time;
            _intime += (float)args.Time * 10;
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //_time += 9.0 * args.Time;
            Matrix4 temp = Matrix4.Identity;
            //temp = temp * Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f);
            //degr += MathHelper.DegreesToRadians(20f);
            //temp = temp * Matrix4.CreateRotationX(degr);
            //_object3d[0].rotatede(_object3d[0]._centerPosition, _object3d[0]._euler[1], 0);
            //_object3d[0].render(0,_time,temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            
            cahaya[1].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            cahaya[0].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            cahaya[2].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            cahaya[3].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            cahaya[4].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            cahaya[0].rotatede(cahaya[0]._centerPosition, cahaya[0]._euler[2], -3);
            cahaya[1].rotatede(cahaya[1]._centerPosition, cahaya[0]._euler[2], -3);
            //cahaya[3].rotatede(cahaya[3]._centerPosition, cahaya[3]._euler[2], -3);
            cahaya[2].rotatede(cahaya[2]._centerPosition, cahaya[2]._euler[2], -3);
            cahaya[4].rotatede(_object3d[5]._centerPosition, _object3d[5]._euler[1], -0.5f);
            _pointLightPositions[4] = cahaya[4]._centerPosition;
            //_pointLightPositions[3] = cahaya[3]._centerPosition;
            _pointLightPositions[2] = cahaya[2]._centerPosition;
            _pointLightPositions[1] = cahaya[1]._centerPosition;
            _pointLightPositions[0] = cahaya[0]._centerPosition;
            

            //_object3d[0].setFragVariable(new Vector3(0.1f, 0.5f, 0.5f), new Vector3(0,1,.8f), cahaya[0]._centerPosition, _camera.Position);

            for (int i = 0; i < 9; i++)
            {
                _object3d[i].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                _object3d[i].setDirectionalLight(new Vector3(1f, -1, 0.0f), new Vector3(0.1f), new Vector3(1), new Vector3(0.5f));
                //_object3d[i].setPointLight(cahaya[0]._centerPosition, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
                _object3d[i].setSpotLight(_camera.Position, _camera.Front, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
                1.0f, 0.09f, 0.032f, MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(17.5f)));
                _object3d[i].setPointLights(_pointLightPositions, new Vector3(0.1f, 0.1f, 0.1f), point_light_color_difuse, new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }
            _object3d[1].setFragVariable(new Vector3(0.5f, 0f, 0f), _camera.Position); //bocil
            _object3d[2].setFragVariable(new Vector3(1f, 0f, 0.67f), _camera.Position); //donat
            _object3d[3].setFragVariable(new Vector3(1f, 1f, 0f), _camera.Position); //backpack
            _object3d[4].setFragVariable(new Vector3(0f, 0.1f, 0f), _camera.Position); //latar
            _object3d[5].setFragVariable(new Vector3(0.2f, 0.9f, 0.9f), _camera.Position); //latar
            _object3d[6].setFragVariable(new Vector3(0.2f, 0.9f, 0.9f), _camera.Position);
            _object3d[7].setFragVariable(new Vector3(0.5f, 0f, 0f), _camera.Position); //bocil
            _object3d[8].setFragVariable(new Vector3(1f, 0f, 0.67f), _camera.Position); //donat
            //_object3d[1].cameraCoord(_camera, _object3d[1]._centerPosition, 0.02f);
            //_object3d[2].cameraCoord(_camera, _object3d[2]._centerPosition, 0.02f);
            //_object3d[3].cameraCoord(_camera, _object3d[3]._centerPosition, 0.02f);
            _object3d[1].cameraCoord(_camera, _object3d[1]._centerPosition, 0.12f);
            _object3d[2].cameraCoord(_camera, _object3d[2]._centerPosition, 0.12f);
            _object3d[3].cameraCoord(_camera, _object3d[3]._centerPosition, 0.12f);
            _object3d[4].cameraCoord(_camera, _object3d[4]._centerPosition, 5);
            _object3d[5].cameraCoord(_camera, _object3d[5]._centerPosition, 5f, 0.25f, 5f);
            _object3d[6].cameraCoord(_camera, _object3d[6]._centerPosition, 0.5f);
            cahaya[0].cameraCoord(_camera, cahaya[0]._centerPosition, 0.2f);
            cahaya[1].cameraCoord(_camera, cahaya[1]._centerPosition, 0.2f);
            cahaya[2].cameraCoord(_camera, cahaya[2]._centerPosition, 0.2f);
            cahaya[3].cameraCoord(_camera, cahaya[3]._centerPosition, 0.2f);

            
            //for (float i = 0; i < 2; i += 0.01f)
            //{
            //    for (int j = 0; j < 7; j++)
            //    {
            //        if (Count % 25 == 0)
            //        {
            //            _object3d[j].setDirectionalLight(new Vector3(1, -1, 1), new Vector3(0.5f), new Vector3(1), new Vector3(0.5f));
            //        }
            //        if (Count % 60 == 0)
            //        {
            //            _object3d[j].setDirectionalLight(new Vector3(-1, 1, -11), new Vector3(0.5f), new Vector3(1), new Vector3(0.5f));
            //        }
            //    }
            //}

            if (!drop)
            {
                coundrop = 1;
            }
            if (!rott)
            {
                counroot = 1;
            }
            if (!stPerson)
            {
                countSt = 1;
                _object3d[7].translate(50, 0, 0);
            }
            
            if (!play)
            {
                _object3d[8].translate(50, 0, 0);
            }
            if (rott)
            {
                if (counroot == 1)
                {

                   
                    _object3d[1].readFileOBJ(Constants.obj + "bocil.obj");
                    _object3d[2].readFileOBJ(Constants.obj + "donat.obj");
                    _object3d[3].readFileOBJ(Constants.obj + "backpack-backpack.obj");
                    _object3d[1].translate(-1.1f, -0.155f, -0.5f);
                    _object3d[1].scale(5f, _object3d[1]._centerPosition);
                    _object3d[3].scale(5f, _object3d[3]._centerPosition);
                    _object3d[3].translate(-1f, -0.045f, 0);
                    _object3d[2].translate(-1f, 0, 0);
                    _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[2]._euler[2], 90);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[2], 90);

                    Count = 0;
                    counroot = 0;
                }
                
                if (Count <= 75)
                {
                    //_object3d[1].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 0.05f);
                    //_object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 0.05f);
                    //_object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 0.05f);
                    _object3d[1].translate(0, 0, 0.0025f);
                }
                if (Count > 75 && cameraFree == false && Count < 314)
                {
                    _camera.Position = _object3d[3]._centerPosition - new Vector3(0.509f, -0.153f, 0.326f);
                    _camera.Yaw = 34.2f;
                    _camera.Pitch = -13.2f;

                }
                if (Count >= 76 && Count <= 300)
                {
                    _object3d[2].translate(0.01f, 0, 0.01f);
                    _object3d[3].translate(0.01f, 0, 0.01f);
                    _object3d[2].rotatede(_object3d[2]._centerPosition, _object3d[2]._euler[1], 5);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[1], 5);
                    _object3d[1].translate(0.01f, 0, 0.01f);
                    cahaya[4].cameraCoord(_camera, cahaya[4]._centerPosition, 0.2f * 1.01f);
                    cahaya[4].scale(1.009f, cahaya[4]._centerPosition);
                    cahaya[4].translate(0, -0.0027f, 0);

                }
                if (Count >= 301 && Count <= 315)
                {
                    _object3d[1].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 12);
                    _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[2]._euler[1], 18);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[1], 18);
                    _object3d[1].translate(0, 0, -0.015f);
                }
                if (Count >= 314 && cameraFree == false && Count < 539)
                {
                    _camera.Position = _object3d[3]._centerPosition - new Vector3(-0.42f, -0.525f, -0.21f);
                    _camera.Yaw = -123.2f;
                    _camera.Pitch = -33.6f;
                }
                if (Count >= 316 && Count <= 540)
                {
                    _object3d[2].translate(0.01f, 0, -0.01f);
                    _object3d[3].translate(0.01f, 0, -.01f);
                    _object3d[2].rotatede(_object3d[2]._centerPosition, _object3d[2]._euler[1], 5);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[1], 5);
                    _object3d[1].translate(0.01f, 0, -0.01f);
                    cahaya[4].cameraCoord(_camera, cahaya[4]._centerPosition, 0.2f * 0.992f);
                    cahaya[4].scale(0.991f, cahaya[4]._centerPosition);
                    cahaya[4].translate(0, +0.0027f, 0);

                }
                if (Count >= 541 && Count <= 555)
                {
                    _object3d[1].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 4);
                    _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 4);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 4);
                    _object3d[1].translate(0.002f, 0, 0.01f);
                }
                //if (Count >= 0)
                //{
                //    Console.WriteLine("position" + _camera.Position);
                //    Console.WriteLine("yaw" + _camera.Yaw);
                //    Console.WriteLine("pitch" + _camera.Pitch);
                //}
                if (Count >= 545 && cameraFree == false && Count < 780)
                {
                    _camera.Position = _object3d[3]._centerPosition - new Vector3(-0.5f, -0.637f, 0.05f);
                    _camera.Yaw = -184.5f;
                    _camera.Pitch = -44.4f;

                }
                if (Count >= 556 && Count <= 780)
                {
                    _object3d[2].translate(-0.01f, 0, -0.01f);
                    _object3d[3].translate(-0.01f, 0, -0.01f);
                    _object3d[2].rotatede(_object3d[2]._centerPosition, _object3d[2]._euler[1], 5);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[1], 5);
                    _object3d[1].translate(-0.01f, 0, -0.01f);
                    cahaya[4].cameraCoord(_camera, cahaya[4]._centerPosition, 0.2f * 1.01f);
                    cahaya[4].scale(1.009f, cahaya[4]._centerPosition);
                    cahaya[4].translate(0, -0.0027f, 0);

                }
                if (Count >= 781 && Count <= 795)
                {
                    _object3d[1].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 6f);
                    _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 6f);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 6f);
                    _object3d[1].translate(0, 0, 0);
                }
                if (Count >= 795 && cameraFree == false && Count < 1100)
                {
                    _camera.Position = _object3d[3]._centerPosition - new Vector3(-0.21f, -0.075f, 0.37f);
                    _camera.Yaw = -611.85f;
                    _camera.Pitch = -7.798f;
                }
                if (Count >= 796 && Count <= 1020)
                {
                    _object3d[2].translate(-0.0075f, 0, 0.007f);
                    _object3d[3].translate(-0.0075f, 0, 0.007f);
                    _object3d[2].rotatede(_object3d[2]._centerPosition, _object3d[2]._euler[1], 5);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[1], 5);
                    _object3d[1].translate(-0.0075f, 0, 0.007f);
                    cahaya[4].cameraCoord(_camera, cahaya[4]._centerPosition, 0.2f * 0.992f);
                    cahaya[4].scale(0.991f, cahaya[4]._centerPosition);
                    cahaya[4].translate(0, 0.0027f, 0);

                }
                if (Count >= 1021 && Count <= 1100)
                {
                    _object3d[2].translate(-0.0075f, 0, 0.007f);
                    _object3d[3].translate(-0.0075f, 0, 0.007f);
                    _object3d[2].rotatede(_object3d[2]._centerPosition, _object3d[2]._euler[1], 5);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[1], 5);
                    _object3d[1].translate(-0.0075f, 0, 0.007f);

                }
                if (Count >= 1101 && Count <= 1115)
                {
                    _object3d[1].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 4);
                    _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 4f);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[1]._euler[1], 4f);

                }
                if (Count >= 1115)
                {
                    Count = 0;
                    counroot = 1;
                    cameraFree = true;
                }
            }
       
            
            if (drop)
            {
                if (coundrop == 1)
                {
                    
                    _object3d[1].readFileOBJ(Constants.obj + "bocil.obj");
                    _object3d[2].readFileOBJ(Constants.obj + "donat.obj");
                    _object3d[3].readFileOBJ(Constants.obj + "backpack-backpack.obj");
                    _object3d[1].translate(-1.1f, -0.155f, -0.5f);
                    _object3d[1].scale(5f, _object3d[1]._centerPosition);
                    _object3d[3].scale(5f, _object3d[3]._centerPosition);
                    _object3d[3].translate(-1f, -0.045f, 0);
                    _object3d[2].translate(-1f, 0, 0);
                    _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[2]._euler[2], 90);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[2], 90);
                    Count = 0;
                    coundrop = 0;
                }
                if (Count <= 75)
                {
                    _object3d[1].translate(0, 0, 0.0025f);
                }
                if (Count > 75 && cameraFree == false && Count < 485)
                {
                    _camera.Position = _object3d[3]._centerPosition - new Vector3(0.509f, -0.153f, 0.326f);
                    _camera.Yaw = 10f;
                    _camera.Pitch = -13.2f;
                }
                if (Count >= 76 && Count <= 490)
                {
                    _object3d[2].translate(0.01f, 0, 0.01f);
                    _object3d[3].translate(0.01f, 0, 0.01f);
                    _object3d[2].rotatede(_object3d[3]._centerPosition, _object3d[2]._euler[1], 5);
                    _object3d[3].rotatede(_object3d[3]._centerPosition, _object3d[3]._euler[1], 5);
                    _object3d[1].translate(0.01f, 0, 0.01f);
                }
                if (Count >= 491 && Count <= 700)
                {
                    _object3d[2].translate(0f, -0.01f, 0f);
                    _object3d[3].translate(0f, -0.01f, 0f);
                }
                if (Count > 491 && cameraFree == false && Count < 695)
                {
                    _camera.Position = _object3d[3]._centerPosition + new Vector3(0f, 0f, 1f);
                    _camera.Yaw = 440f;
                    _camera.Pitch = 180f;
                }
                if (Count >= 695)
                {
                    Count = 0;
                    coundrop = 1;
                    cameraFree = true;
                }
            }

            bool checkObj = _object3d[7].CheckCollision(_object3d[7]._centerPosition, _object3d[1]._centerPosition, 0.1f, 0.1f, 0.1f);
            bool checkObj1 = _object3d[7].CheckCollision(_object3d[7]._centerPosition, _object3d[2]._centerPosition, 0.2f, 0.2f, 0.2f);
            bool checkObj2 = _object3d[7].CheckCollision(_object3d[7]._centerPosition, _object3d[3]._centerPosition, 0.2f, 0.2f, 0.2f);
           
            bool checkObj3 = _object3d[7].CheckCollision(_object3d[7]._centerPosition, _object3d[8]._centerPosition, 0.2f, 0.2f, 0.2f);
            if (stPerson)
            {
                Console.WriteLine(_camera.Yaw);
                if (countSt == 1)
                {
                    _object3d[7].readFileOBJ(Constants.obj + "bocil1.obj");
                    
                    _object3d[7].translate(0f, -0.155f, -5f);
                    _object3d[7].scale(7f, _object3d[7]._centerPosition);
                    countSt = 0;
                    _camera.Position = _object3d[7]._centerPosition + new Vector3 (0, 0.35f, -0.3f);
                    _camera.Yaw = 90.4f;
                    _camera.Pitch = 0;
                  
                }
                if (checkObj || checkObj1 || checkObj2 )
                {
                    countSt = 1;
                }
                if (play)
                {
                    Random rnd = new Random();
                    float rand = rnd.Next(100, 400) / 100;
                    
                    
                    if (countpl == 1)
                    {
                         num = rnd.Next(1, 3);
                        _object3d[8].readFileOBJ(Constants.obj + "donat1.obj");
                        Count = 0;
                        
                        countpl = 0;
                        if (num == 1)
                        {
                            _object3d[8].translate(0, 0, -rand);

                        }
                        else
                        {
                            _object3d[8].translate(0, 0, rand);

                        }
                        cahaya[3]._centerPosition = _object3d[8]._centerPosition - new Vector3(0,1f,0);
                        _pointLightPositions[3] = _object3d[8]._centerPosition - new Vector3(0, 1f, 0);
                    }
                    if (Count >= 0 && Count <= 180)
                    {
                        _object3d[8].rotatede(_object3d[8]._centerPosition, _object3d[8]._euler[1], 1);
                        _object3d[8].scale(1.001f,_object3d[8]._centerPosition);
                    }
                    if (Count >= 181 && Count <= 360)
                    {
                        _object3d[8].rotatede(_object3d[8]._centerPosition, _object3d[8]._euler[1], 1);
                        _object3d[8].scale(0.999f, _object3d[8]._centerPosition);
                    }
                    if (Count >= 360)
                    {
                        Count = 0;
                    }

                    if (checkObj3)
                    {
                        _object3d[8].translate(200, 0, 10);
                        cahaya[3]._centerPosition = _object3d[8]._centerPosition + new Vector3(0, 0.4f, 0);

                    }
                }
                if (_object3d[7]._centerPosition.Z <= -5.75||_object3d[7]._centerPosition.X <= -3.91|| _object3d[7]._centerPosition.Z >= 3.75|| _object3d[7]._centerPosition.X >= 5.91)
                {
                    countSt = 1;
                }
                if (_camera.Yaw >= 181 && _camera.Yaw <=  360 && countRd == 1 && countSt == 0)
                {
                    _object3d[7].rotatede(_object3d[7]._centerPosition, _object3d[7]._euler[1], 180);
                    countRd = 0;
                    _object3d[7].translate(0, 0, -0.6f);
                   
                }

                if (_camera.Yaw >= 0 && _camera.Yaw <= 180 && countRd == 0 && countSt == 0)
                {
                    _object3d[7].rotatede(_object3d[7]._centerPosition, _object3d[7]._euler[1], -180);
                    countRd = 1;
                    _object3d[7].translate(0, 0, 0.6f);
                    
                }
                
                if (_camera.Yaw >= 360)
                {
                    _camera.Yaw = 0;
                }
                if (_camera.Yaw <= 0)
                {
                    _camera.Yaw += 360;
                }

            }

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            float cameraSpeed = 1f;
            if (!stPerson) { 
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
            } 
            }
            else if (stPerson)
            {
                if (KeyboardState.IsKeyDown(Keys.W))
                {
                    _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
                    _object3d[7]._centerPosition += _camera.Front * cameraSpeed * (float)args.Time;
                    _object3d[7]._model *= Matrix4.CreateTranslation(_camera.Front * cameraSpeed * (float)args.Time);
                }
                if (KeyboardState.IsKeyDown(Keys.S))
                {
                    _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
                    _object3d[7]._centerPosition -= _camera.Front * cameraSpeed * (float)args.Time;
                    _object3d[7]._model *= Matrix4.CreateTranslation(-_camera.Front * cameraSpeed * (float)args.Time);
                
                }
                if (KeyboardState.IsKeyDown(Keys.A))
                {
                    _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
                    _object3d[7]._centerPosition -= _camera.Right * cameraSpeed * (float)args.Time;
                    _object3d[7]._model *= Matrix4.CreateTranslation(-_camera.Right * cameraSpeed * (float)args.Time);
                }
                if (KeyboardState.IsKeyDown(Keys.D))
                {
                    _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
                    _object3d[7]._centerPosition += _camera.Right * cameraSpeed * (float)args.Time;
                    _object3d[7]._model *= Matrix4.CreateTranslation(_camera.Right * cameraSpeed * (float)args.Time);
                }
            }
            var mouse = MouseState;
            var sensitivity = 0.2f;
            if (!stPerson)
            {
                if (_firstMove)
                {
                    _lastPos = new Vector2(mouse.X, mouse.Y);
                    _firstMove = false;
                }
                else
                {
                    var deltaX = mouse.X - _lastPos.X;
                    var deltaY = mouse.Y - _lastPos.Y;
                    _lastPos = new Vector2(mouse.X, mouse.Y);
                    _camera.Yaw += deltaX * sensitivity;
                    _camera.Pitch -= deltaY * sensitivity;
                }

                if (KeyboardState.IsKeyDown(Keys.N))
                {
                    var axis = new Vector3(0, 1, 0);
                    _camera.Position -= _objecPost;
                    _camera.Yaw += _rotationSpeed;
                    _camera.Position = Vector3.Transform(_camera.Position,
                        generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                    _camera.Position += _objecPost;

                    _camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
                }
                if (KeyboardState.IsKeyDown(Keys.Comma))
                {
                    var axis = new Vector3(0, 1, 0);
                    _camera.Position -= _objecPost;
                    _camera.Yaw -= _rotationSpeed;
                    _camera.Position = Vector3.Transform(_camera.Position,
                        generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                    _camera.Position += _objecPost;

                    _camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
                }
                if (KeyboardState.IsKeyDown(Keys.K))
                {
                    var axis = new Vector3(1, 0, 0);
                    _camera.Position -= _objecPost;
                    _camera.Pitch -= _rotationSpeed;
                    _camera.Position = Vector3.Transform(_camera.Position,
                        generateArbRotationMatrix(axis, _objecPost, _rotationSpeed).ExtractRotation());
                    _camera.Position += _objecPost;
                    _camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
                }
                if (KeyboardState.IsKeyDown(Keys.M))
                {
                    var axis = new Vector3(1, 0, 0);
                    _camera.Position -= _objecPost;
                    _camera.Pitch += _rotationSpeed;
                    _camera.Position = Vector3.Transform(_camera.Position,
                        generateArbRotationMatrix(axis, _objecPost, -_rotationSpeed).ExtractRotation());
                    _camera.Position += _objecPost;
                    _camera._front = -Vector3.Normalize(_camera.Position - _objecPost);
                }
             }
                if (stPerson)
                {
                    if (_firstMove)
                    {
                    _lastPos = new Vector2(mouse.X, mouse.Y);
                    _firstMove = false;
                    }
                     else
                    {
                        var deltaX = mouse.X - _lastPos.X;
                        
                        _lastPos = new Vector2(mouse.X, mouse.Y);
                        _camera.Yaw += deltaX* sensitivity;
                        //_object3d[7].rotatede(_object3d[7]._centerPosition, _object3d[7]._euler[1], deltaX * sensitivity);
                    }
            }
            if (KeyboardState.IsKeyDown(Keys.P))
                {
                    cameraFree = false;
                }
            if (KeyboardState.IsKeyDown(Keys.KeyPad0))
                {
                    cameraFree = true;
                }
            
            if (KeyboardState.IsKeyDown(Keys.KeyPad1))
            {
                rott = true;
                drop = false;
                stPerson = false;
                play = false;
            }
            if (KeyboardState.IsKeyDown(Keys.KeyPad2))
            {
                drop = true;
                rott = false;
                stPerson = false;
                play = false;
            }
            if (KeyboardState.IsKeyDown(Keys.KeyPad3))
            {
                stPerson = true;
                drop = false;
                rott = false;
                play = false;
            }
            if (KeyboardState.IsKeyDown(Keys.KeyPad4))
            {
                play = true;
                rott = false;
                stPerson = true;
                drop = false;
                countpl = 1;

            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X / 2) / (Size.X / 2);
                float _y = -(MousePosition.Y - Size.Y / 2) / (Size.Y / 2);

                Console.WriteLine("x = " + _x + "y = " + _y);
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Console.WriteLine("Offset Y: " + e.OffsetY);
            Console.WriteLine("Offset X: " + e.OffsetX);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }


    }
}
