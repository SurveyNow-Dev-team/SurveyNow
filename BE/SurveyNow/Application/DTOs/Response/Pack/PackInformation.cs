using Domain.Enums;

namespace Application.DTOs.Response.Pack
{
    public class PackInformation
    {
        public required string Name { get; set; }
        public PackType PackType { get; set; }

        public required List<string> Benefits { get; set; }
    }
}
