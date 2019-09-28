namespace Surveying3D.Core
{
    public class SurveyResults
    {
        public float Height { get; }
        public float Width { get; }
        public float Volume { get; }

        public SurveyResults(float height, float width, float volume)
        {
            Height = height;
            Width = width;
            Volume = volume;
        }
    }
}