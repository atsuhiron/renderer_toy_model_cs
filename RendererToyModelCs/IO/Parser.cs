using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;
using RendererToyModelCs.Geom;
using RendererToyModelCs.WorldObject;

namespace RendererToyModelCs.IO
{
    public static class Parser
    {
        public static World Parse(Dictionary<string, dynamic?> dict)
        {
            Dictionary<string, dynamic?> cameraDict = dict.GetValueOrDefault("camera", null) ?? throw new ArgumentException("camera must not be null.");
            List<object> surfaceDictList = dict.GetValueOrDefault("surfaces", null) ?? throw new ArgumentException("surfaces must not be null.");

            Camera camera = ParseCamera(cameraDict);
            List<ISurface> surfaceList = surfaceDictList.Select(suf => (Dictionary<string, dynamic?>) suf).Select(ParseSurface).ToList();

            return new World(surfaceList, camera);
        }

        private static Camera ParseCamera(Dictionary<string, dynamic?> dict)
        {
            ArgumentNullException.ThrowIfNull(dict, "camera");
            float focal = ParseNumeric<float>(dict, "focal_length");
            if (focal <= 0f) throw new ArgumentException("focal_length must be positive");
            float fovV = ParseNumeric<float>(dict, "fov_v");
            if (fovV <= 0f) throw new ArgumentException("fov_v must be positive");
            float fovH = ParseNumeric<float>(dict, "fov_h");
            if (fovH <= 0f) throw new ArgumentException("fov_h must be positive");
            int pixelV = ParseNumeric<int>(dict, "pixel_v");
            if (pixelV <= 0) throw new ArgumentException("pixel_v must be positive");
            int pixelH = ParseNumeric<int>(dict, "pixel_h");
            if (pixelH <= 0) throw new ArgumentException("pixel_h must be positive");
            Vector<float> pos = ParseVec(dict.GetValueOrDefault("pos", null), "pos");
            Vector<float> vec = ParseVec(dict.GetValueOrDefault("vec", null), "vec");
            CameraMode mode = ParseCameraMode(dict.GetValueOrDefault("camera_mode", "default"));

            return new Camera(pos, vec, focal, fovV, fovH, pixelV, pixelH, mode);
        }

        private static T ParseNumeric<T>(Dictionary<string, dynamic?> parent, string name) where T : struct, IComparable<T>, IEquatable<T>
        {
            dynamic? num = parent.GetValueOrDefault(name, null);
            ArgumentNullException.ThrowIfNull(num, name);
            return (T)num;
        }

        private static CameraMode ParseCameraMode(string name)
        {
            var lower = name.ToLower();
            foreach (CameraMode mode in Enum.GetValues(typeof(CameraMode)))
            {
                if (lower == name) return mode;
            }
            throw new ArgumentException($"Cannot parse camera mode: {name}");
        }

        private static ISurface ParseSurface(Dictionary<string, dynamic?> dict)
        {
            ArgumentNullException.ThrowIfNull(dict, "surfaces");
            if (!dict.ContainsKey("surface_type")) throw new ArgumentException("surface_type must not be null.");
            string sufType = dict.GetValueOrDefault("surface_type", "default") ?? "default";

            var points = new List<Vector<float>>
            {
                ParseVec(dict.GetValueOrDefault("point1")),
                ParseVec(dict.GetValueOrDefault("point2")),
                ParseVec(dict.GetValueOrDefault("point3"))
            };

            string? name = dict.GetValueOrDefault("name", null);

            return sufType switch
            {
                "rough" => new RoughSurface(points, name, CColor.CreateFromColorCode(dict.GetValueOrDefault("color", null))),
                "light" => new LightSurface(points, name, CLight.CreateFromColorCode(dict.GetValueOrDefault("light", null))),
                "smooth" => new SmoothSurface(points, name),
                _ => throw new ArgumentException($"unknown surface type: {sufType}"),
            };
        }

        private static Vector<float> ParseVec(dynamic? point, string nameForErrorMsg="point")
        {
            ArgumentNullException.ThrowIfNull((object?)point, nameForErrorMsg);

            float[] array = ((List<object>)(point ?? new List<float>())).Select(ToFloat).ToArray();
            if (array.Length != 3) throw new ArgumentException("Length of point must be 3");

            return Vector<float>.Build.DenseOfArray(array);
        }

        private static float ToFloat(object obj)
        {
            return obj switch
            {
                float floatVal => floatVal,
                double doubleVal => (float)doubleVal,
                decimal decimalVal => (float)decimalVal,
                _ => throw new ArgumentException($"Not supported type: {obj.GetType()}")
            };
        }
    }
}
