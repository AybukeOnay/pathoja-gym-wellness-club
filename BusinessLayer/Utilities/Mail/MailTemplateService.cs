using BusinessLayer.DTOs.Contact;
using EntityLayer.Concrete;

namespace BusinessLayer.Utilities.Mail
{
    public static class MailTemplateService
    {
        public static (string Subject, string Body) BuildAdminMail(
            ContactCreateDto dto,
            string? categoryName = null)
        {
            return dto.Type switch
            {
                ContactType.PackageInfoContact => BuildAdminPackageMail(dto, categoryName),
                ContactType.GeneralContact => BuildAdminGeneralMail(dto),
                _ => BuildAdminGeneralMail(dto)
            };
        }

        public static (string Subject, string Body) BuildUserMail(
            ContactCreateDto dto,
            string? categoryName = null)
        {
            return dto.Type switch
            {
                ContactType.PackageInfoContact => BuildUserPackageMail(dto, categoryName),
                ContactType.GeneralContact => BuildUserGeneralMail(dto),
                _ => BuildUserGeneralMail(dto)
            };
        }

        private static (string Subject, string Body) BuildAdminPackageMail(
            ContactCreateDto dto,
            string? categoryName)
        {
            var subject = "Yeni Paket Bilgilendirme Talebi";

            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color:#333; line-height:1.6;'>
                    <h2>Yeni bir paket bilgilendirme talebi oluşturuldu.</h2>

                    <p><strong>Ad Soyad:</strong> {dto.FullName}</p>
                    <p><strong>Telefon:</strong> {dto.Phone}</p>
                    <p><strong>E-posta:</strong> {dto.Email}</p>
                    <p><strong>Kategori:</strong> {categoryName ?? "-"}</p>
                </body>
                </html>";

            return (subject, body);
        }

        private static (string Subject, string Body) BuildAdminGeneralMail(ContactCreateDto dto)
        {
            var subject = "Yeni İletişim Talebi";

            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color:#333; line-height:1.6;'>
                    <h2>Site üzerinden yeni bir iletişim talebi oluşturuldu.</h2>
                    <p><strong>Ad Soyad:</strong> {dto.FullName}</p>
                    <p><strong>Telefon:</strong> {dto.Phone}</p>
                    <p><strong>E-posta:</strong> {dto.Email}</p>
                    <p><strong>Mesaj:</strong><br/>{dto.Message ?? "-"}</p>
                </body>
                </html>";

            return (subject, body);
        }

        private static (string Subject, string Body) BuildUserPackageMail(
            ContactCreateDto dto,
            string? categoryName)
        {
            var subject = "Bilgilendirme Talebiniz Alındı";

            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color:#333; line-height:1.6;'>
                    <p>Merhaba {dto.FullName},</p>

                    <p>Bilgilendirme talebiniz tarafımıza başarıyla ulaşmıştır.</p>
                    <p>Ekibimiz en kısa sürede sizinle iletişime geçecektir.</p>

                    <p>Teşekkür ederiz.<br/>Pathoja GYM & Wellness Club</p>
                </body>
                </html>";

            return (subject, body);
        }

        private static (string Subject, string Body) BuildUserGeneralMail(ContactCreateDto dto)
        {
            var subject = "Pathoja GYM & Wellness Club - Mesajınız Bize Ulaştı";

            var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; color:#333; line-height:1.6;'>
                <p>Merhaba {dto.FullName},</p>
                <p>Mesajınız tarafımıza ulaşmıştır. En kısa sürede sizinle iletişime geçeceğiz.</p>
                <p>Teşekkür ederiz.<br/>Pathoja GYM & Wellness Club</p>
            </body>
            </html>";

            return (subject, body);
        }
    }
}

//namespace BusinessLayer.Utilities.Mail
//{
//    public static class MailTemplateService
//    {
//        public static (string Subject, string Body) BuildAdminMail(ContactCreateDto dto,string? categoryName = null,string? productName = null)
//        {
//            switch (dto.Type)
//            {
//                case ContactType.PackageInfoContact:
//                    return BuildAdminPackageMail(dto, categoryName);

//                case ContactType.GeneralContact:
//                default:
//                    return BuildAdminGeneralMail(dto);
//            }
//        }

//        public static (string Subject, string Body) BuildUserMail(ContactCreateDto dto,string? categoryName = null)
//        {
//            switch (dto.Type)
//            {
//                case ContactType.PackageInfoContact:
//                    return BuildUserInformationMail(dto, categoryName);

//                case ContactType.GeneralContact:
//                default:
//                    return BuildUserGeneralMail(dto);
//            }
//        }

//        private static (string Subject, string Body) BuildAdminPackageMail(ContactCreateDto dto, string? categoryName)
//        {
//            var subject = "Üyelik Hizmeti Bilgilendirme";
//            var body = $@"
//            Yeni bir paket bilgilendirme talebi oluşturuldu.
//            Ad Soyad : {dto.FullName}
//            Telefon  : {dto.Phone}
//            E-posta  : {dto.Email}
//            Kategori : {categoryName ?? "-"}";

//            return (subject, body);
//        }

//        private static (string Subject, string Body) BuildAdminGeneralMail(ContactCreateDto dto)
//        {
//            var subject = "Yeni İletişim Talebi";
//            var body = $@"
//            Site üzerinden yeni bir iletişim talebi oluşturuldu.

//            Ad Soyad : {dto.FullName}
//            Telefon  : {dto.Phone}
//            E-posta  : {dto.Email}

//            Mesaj:
//            {dto.Message ?? "-"}
//            ";

//            return (subject, body);
//        }

//        private static (string Subject, string Body) BuildUserInformationMail(ContactCreateDto dto,string? categoryName)
//        {
//            var subject = "Pathoja GYM & Wellness Club - Talebiniz Alınmıştır";

//            var sb = new StringBuilder();
//            sb.AppendLine($"Merhaba {dto.FullName},");
//            sb.AppendLine();
//            sb.AppendLine("Pathoja GYM & Wellness Club, web sitemiz üzerinden bize ulaştığınız için teşekkür ederiz.");
//            sb.AppendLine("Mesajınız tarafımıza başarıyla ulaşmıştır. En kısa sürede sizinle iletişime geçeceğiz.");
//            sb.AppendLine();
//            sb.AppendLine();
//            sb.AppendLine("Sağlıklı ve keyifli günler dileriz,");
//            sb.AppendLine("Pathoja GYM & Wellness Club");

//            return (subject, sb.ToString());
//        }

//        private static (string Subject, string Body) BuildUserGeneralMail(
//    ContactCreateDto dto)
//        {
//            var subject = "Pathoja GYM & Wellness Club - Paket Bilgilendirme Talebiniz Alındı";

//            var sb = new StringBuilder();
//            sb.AppendLine($"Merhaba {dto.FullName},");
//            sb.AppendLine();
//            sb.AppendLine("Paketlerimiz hakkında bilgilendirme talebiniz tarafımıza ulaşmıştır.");
//            sb.AppendLine("En kısa sürede sizi arayarak paketler, ders içerikleri ve uygun saatler hakkında detaylı bilgi vereceğiz.");
//            sb.AppendLine();



//            sb.AppendLine("İletişim bilgileriniz:");
//            sb.AppendLine($"- Telefon : {dto.Phone}");
//            sb.AppendLine($"- E-posta : {dto.Email}");
//            sb.AppendLine();
//            sb.AppendLine("Pathoja GYM & Wellness Club ailesi olarak sizinle tanışmak için sabırsızlanıyoruz.");
//            sb.AppendLine();
//            sb.AppendLine("Sevgiler,");
//            sb.AppendLine("Pathoja GYM & Wellness Club");

//            return (subject, sb.ToString());
//        }

//    }
//}
