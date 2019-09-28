using Utf8Json;

namespace Surveying3D.Core
{
    public readonly struct SurveyResults
    {
        public readonly float Height;
        public readonly float Width;
        public readonly float Volume;
        public readonly float Depth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height">y軸方向</param>
        /// <param name="width">x軸方向</param>
        /// <param name="depth">z軸方向</param>
        /// <param name="volume"></param>
        [SerializationConstructor]
        public SurveyResults(float height, float width,float depth, float volume)
        {
            Height = height;
            Width = width;
            Depth = depth;
            Volume = volume;
        }
    }
}