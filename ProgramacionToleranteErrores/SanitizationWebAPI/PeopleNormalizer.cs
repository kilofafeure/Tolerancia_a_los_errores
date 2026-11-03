using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SanitizationWebAPI
{
    public static partial class PeopleNormalizer
    {
        public static People Normalize(People people)
        {
            return new People
            {
                Name = NormalizeName(people.Name),
                Age = people.Age,
                Email = NormalizeEmail(people.Email),
                Phone = NormalizePhone(people.Phone),
                Description = NormalizeDescription(people.Description),
                Content = NormalizeContent(people.Content)
            };
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhiteSpace();

        [GeneratedRegex(@"\D")]
        private static partial Regex OnlyNumbers();

        [GeneratedRegex(@"[^a-zA-ZñÑüÜáéíóú´àèòÁÉÍÓÚÀÈÒ\s]")]
        private static partial Regex AllowedChars();

        public static string NormalizeName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            name = name.Trim();
            name = WhiteSpace().Replace(name, " ");

            return name;
        }

        public static string NormalizeEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return string.Empty;
            }

            email = email.Trim();
            email = email.ToLowerInvariant();

            return email;
        }

        public static string NormalizePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return string.Empty;
            }

            phone = phone.Trim();
            phone = OnlyNumbers().Replace(phone, "");

            return phone;
        }

        public static string NormalizeDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return string.Empty;
            }

            description = AllowedChars().Replace(description, " ");
            description = WhiteSpace().Replace(description, " ");
            description = description.Trim();

            return description;
        }

        public static string NormalizeContent(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var scripts = doc.DocumentNode.SelectNodes("//script|//style");

            if (scripts != null)
            {
                foreach (var script in scripts)
                {
                    script.Remove();
                }
            }

            return doc.DocumentNode.InnerHtml.Trim();
        }

    }
}
