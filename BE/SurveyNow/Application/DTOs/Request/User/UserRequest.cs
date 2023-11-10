using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Request.User
{
    public class UserRequest
    {
        [RegularExpression(@"^[a-zA-ZăâêôơưĂÂÊÔƠƯàáảãạÀÁẢÃẠèéẻẽẹÈÉẺẼẸìíỉĩịÌÍỈĨỊòóỏõọÒÓỎÕỌùúủũụÙÚỦŨỤỳýỷỹỵỲÝỶỸỴăắằẳẵặĂẮẰẲẴẶâấầẩẫậÂẤẦẨẪẬêếềểễệÊẾỀỂỄỆôốồổỗộÔỐỒỔỖỘơớờởỡợƠỚỜỞỠỢưứừửữựƯỨỪỬỮỰđĐ\s]*$", ErrorMessage = "Full name contains a-z, A-Z, space & other Vietnamese alphabet characters")]
        public string? FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? RelationshipStatus { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression(@"^vn$", ErrorMessage = "We currently support Vietnamese")]
        public string? LangKey { get; set; } = "vn";

        [RegularExpression(@"^VND$", ErrorMessage = "We currently support VND")]
        public string? Currency { get; set; } = "VND";

        public AddressRequest? Address { get; set; }

        public OccupationRequest? Occupation { get; set;}
    }
}
