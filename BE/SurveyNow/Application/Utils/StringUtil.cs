using System.Globalization;
using System.Text.RegularExpressions;

namespace Application.Utils;

public static class StringUtil
{
    public static string? GetNameFromEmail(string email)
    {
        var name = email?.Split("@")[0];
        if (name != null && name.Trim() != "")
            return FormatVietnameseString(name);
        return null;
    }

    public static string FormatVietnameseString(string input)
    {
        input = Regex.Replace(input, @"\s+", " ").Trim();

        input = Regex.Replace(input,
            @"[^a-zA-Z0-9ăâêôơưđĂÂÊÔƠƯĐàáảãạÀÁẢÃẠèéẻẽẹÈÉẺẼẸìíỉĩịÌÍỈĨỊòóỏõọÒÓỎÕỌùúủũụÙÚỦŨỤỳýỷỹỵỲÝỶỸỴăắằẳẵặĂẮẰẲẴẶâấầẩẫậÂẤẦẨẪẬêếềểễệÊẾỀỂỄỆôốồổỗộÔỐỒỔỖỘơớờởỡợƠỚỜỞỠỢưứừửữựƯỨỪỬỮỰđĐ\s]",
            "");

        TextInfo textInfo = new CultureInfo("vi", false).TextInfo;
        return textInfo.ToTitleCase(input);
    }
}