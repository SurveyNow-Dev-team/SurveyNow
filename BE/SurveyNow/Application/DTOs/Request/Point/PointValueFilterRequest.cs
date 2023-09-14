namespace Application.DTOs.Request.Point
{
    public class PointValueFilterRequest
    {
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }

        public int IsValid()
        {
            return (MinPoint > 0 && MaxPoint > 0) ? 1 : 0;
        }

        public void ChangeValues()
        {
            if (MinPoint > MaxPoint)
            {
                (MaxPoint, MinPoint) = (MinPoint, MaxPoint);
            }
        }
    }
}
