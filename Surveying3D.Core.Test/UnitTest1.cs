using System;
using System.IO;
using System.Numerics;
using NUnit.Framework;
using ObjLoader.Loader.Loaders;

namespace Surveying3D.Core.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            var x1 = new Vector3(1, 0, 0);
            var y1 = new Vector3(0, 1, 0);
            var z1 = new Vector3(0, 0, 1);

            var x_1 = new Vector3(-1, 0, 0);
            var y_1 = new Vector3(0, -1, 0);
            var z_1 = new Vector3(0, 0, -1);
            
            //0
            Console.Write("0: ");
            Console.WriteLine(MathUtil.SarrusRule(x1, y1, z1));
            
            //1
            Console.Write("1: ");
            Console.WriteLine(MathUtil.SarrusRule(x_1, z1, y1));

            //2
            Console.Write("2: ");
            Console.WriteLine(MathUtil.SarrusRule(x_1, y1, z_1));

            //3
            Console.Write("3: ");
            Console.WriteLine(MathUtil.SarrusRule(x1, z_1, y1));

            //4
            Console.Write("4: ");
            Console.WriteLine(MathUtil.SarrusRule(z1, y_1, x1));

            //5
            Console.Write("5: ");
            Console.WriteLine(MathUtil.SarrusRule(x_1, y_1, z1));

            //6
            Console.Write("6: ");
            Console.WriteLine(MathUtil.SarrusRule(x_1, z_1, y_1));

            //7
            Console.Write("7: ");
            Console.WriteLine(MathUtil.SarrusRule(x1, y_1, z_1));
            
        }

        [Test]
        public void Test1()
        {
//            Gltf model = Interface.LoadModel("/Users/neno/WorkSpace/CSharp/Surveying3D/Model/cube.glb");
//            Console.WriteLine(model);
//            
//            ModelRoot model2 = SharpGLTF.Schema2.ModelRoot.Load("/Users/neno/WorkSpace/CSharp/Surveying3D/Model/cube.glb");
//            var mesh = model2.LogicalMeshes;
//
//            SharpGLTF.Schema2.Mesh m = mesh[0];

            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create(new MaterialNullStreamProvider());
            float totalVol = 0f;
            try
            {
                var model_path = "/Users/neno/WorkSpace/CSharp/Surveying3D/Model/test.obj";
//                var model_path = "/Users/neno/WorkSpace/DataSet/ForUnity/Beef/Beef0.5/Model.obj";
                var fileStream = File.OpenRead(model_path);
                LoadResult result = objLoader.Load(fileStream);
                Console.WriteLine(result);
                Console.WriteLine("=====================");
                foreach (var group in result.Groups)
                {
                    foreach (var face in group.Faces)
                    {
                        var vertex1 = result.Vertices[face[0].VertexIndex - 1];
                        var vertex2 = result.Vertices[face[1].VertexIndex - 1];
                        var vertex3 = result.Vertices[face[2].VertexIndex - 1];

                        var vec1 = vertex1.AsVector3();
                        var vec2 = vertex2.AsVector3();
                        var vec3 = vertex3.AsVector3();

                        totalVol += MathUtil.SarrusRule(vec1, vec2, vec3) / 6f;
                    }
                }

                Console.Write("Volume Result(m^3)");
                Console.WriteLine(totalVol);

                Console.Write("Volume Result(cm^3)");
                Console.WriteLine(totalVol * 1000000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Assert.Pass();
        }
    }
}