using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Application.DTOs.Request.Point
{
    public class PointDateFilterRequest
    {
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/([0-9]{4})$", ErrorMessage = "Date format must be dd/mm/yyyy")]
        public string? FromDate { get; set; }
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/([0-9]{4})$", ErrorMessage = "Date format must be dd/mm/yyyy")]
        public string? ToDate { get; set; }
        
        public int IsValid()
        {
            string format = "dd/MM/yyyy";
            bool isValidFromDate = DateTime.TryParseExact(FromDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate);
            bool isValidToDate = DateTime.TryParseExact(ToDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate);
            return (isValidFromDate && isValidToDate) ? 1 : 0;  
        }

        public void ChangeValues()
        {
            string format = "dd/MM/yyyy";

            DateTime fromDate = DateTime.ParseExact(FromDate, format, CultureInfo.InvariantCulture), 
                toDate = DateTime.ParseExact(ToDate, format, CultureInfo.InvariantCulture);
            var result = DateTime.Compare(fromDate, toDate);
            if (result > 0)
            {
                (FromDate, ToDate) = (ToDate, FromDate);
            }
        }

        public DateTime GetFromDate()
        {
            string format = "dd/MM/yyyy";
            return DateTime.ParseExact(FromDate, format, CultureInfo.InvariantCulture);
        }

        public DateTime GetToDate()
        {
            string format = "dd/MM/yyyy";
            return DateTime.ParseExact(ToDate, format, CultureInfo.InvariantCulture);
        }
    }
}
