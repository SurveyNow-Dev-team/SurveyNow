using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs.Request.User
{
    public class UserRequest
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? GoogleId { get; set; }

        [RegularExpression(@"^(84|0[3|5|7|8|9])[0-9]{8}$", ErrorMessage = "We accept Vietnamese phone number only")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-ZăâêôơưĂÂÊÔƠƯàáảãạÀÁẢÃẠèéẻẽẹÈÉẺẼẸìíỉĩịÌÍỈĨỊòóỏõọÒÓỎÕỌùúủũụÙÚỦŨỤỳýỷỹỵỲÝỶỸỴăắằẳẵặĂẮẰẲẴẶâấầẩẫậÂẤẦẨẪẬêếềểễệÊẾỀỂỄỆôốồổỗộÔỐỒỔỖỘơớờởỡợƠỚỜỞỠỢưứừửữựƯỨỪỬỮỰđĐ\s]*$", ErrorMessage = "Full name contains a-z, A-Z, space & other Vietnamese alphabet characters")]
        public string? FullName { get; set; } = null!;

        public Gender? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }

        public decimal Point { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;

        public Role Role { get; set; } = Role.User;

        public RelationshipStatus? RelationshipStatus { get; set; }

        [RegularExpression(@"^vn$", ErrorMessage = "We currently support Vietnamese")]
        public string? LangKey { get; set; } = "vn";

        [RegularExpression(@"^VND$", ErrorMessage = "We currently support VND")]
        public string? Currency { get; set; } = "VND";

        public bool IsDelete { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
