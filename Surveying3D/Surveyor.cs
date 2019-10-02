using System;
using System.IO;
using System.Linq;
using ObjLoader.Loader.Loaders;

namespace Surveying3D
{
    public static class Surveyor
    {
        public static SurveyResults Survey(string modelPath)
        {
            try
            {
                var objLoaderFactory = new ObjLoaderFactory();
                //objLoaderは前に読み込んだやつを保持するので毎回新しく作らないといけない。
                var objLoader = objLoaderFactory.Create(new MaterialNullStreamProvider());
                using (var fileStream = File.OpenRead(modelPath))
                {
                    LoadResult result = objLoader.Load(fileStream);
                    return Survey(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("========================================================");
                Console.WriteLine(modelPath + ". \n" + e.Message);
                Console.WriteLine("========================================================");

                throw new Exception(modelPath + "\n" + e.Message);
            }
        }

        public static SurveyResults Survey(LoadResult objContent)
        {
            if (objContent.Groups.SelectMany(x => x.Faces).Any(x => x.Count >= 4))
            {
                throw new Exception("The surface of this object is defined by more than 4 points.");
            }
            
            float totalVolume = 0f;

            foreach (var group in objContent.Groups)
            {
                foreach (var face in group.Faces)
                {
                    var vertex1 = objContent.Vertices[face[0].VertexIndex - 1];
                    var vertex2 = objContent.Vertices[face[1].VertexIndex - 1];
                    var vertex3 = objContent.Vertices[face[2].VertexIndex - 1];

                    var vec1 = vertex1.AsVector3();
                    var vec2 = vertex2.AsVector3();
                    var vec3 = vertex3.AsVector3();

                    totalVolume += MathUtil.SarrusRule(vec1, vec2, vec3) / 6f;
                }
            }
            
            var vertices = objContent.Vertices;
            var height = vertices.Max(x => x.Y) - vertices.Min(x => x.Y);
            var width = vertices.Max(x => x.X) - vertices.Min(x => x.X);
            var depth = vertices.Max(x => x.Z) - vertices.Min(x => x.Z);

            return new SurveyResults(height, width, depth, totalVolume);
        }
    }
}