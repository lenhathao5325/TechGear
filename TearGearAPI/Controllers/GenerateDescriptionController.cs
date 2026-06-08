using Microsoft.AspNetCore.Mvc;

namespace TechGearAPI.Controllers
{
    [Route("api/generate-description")]
    [ApiController]
    public class GenerateDescriptionController : ControllerBase
    {
        public record GenerateRequest(
            string Name,
            string? CategoryName,
            string? BrandName,
            List<string>? Options
        );

        // POST /api/generate-description  — cho sản phẩm
        [HttpPost]
        public IActionResult Generate([FromBody] GenerateRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Tên không được để trống.");

            var description = DescriptionBuilder.Build(req.Name, req.CategoryName, req.BrandName, req.Options);
            return Ok(new { description });
        }

        // POST /api/generate-description/category
        [HttpPost("category")]
        public IActionResult GenerateCategory([FromBody] GenerateRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Tên danh mục không được để trống.");

            var description = DescriptionBuilder.BuildCategory(req.Name);
            return Ok(new { description });
        }

        // POST /api/generate-description/brand
        [HttpPost("brand")]
        public IActionResult GenerateBrand([FromBody] GenerateRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Tên thương hiệu không được để trống.");

            var description = DescriptionBuilder.BuildBrand(req.Name);
            return Ok(new { description });
        }
    }

    internal static class DescriptionBuilder
    {
        public static string Build(string name, string? category, string? brand, List<string>? options)
        {
            var cat = (category ?? "").ToLower();
            var br = brand?.Trim() ?? "";
            var nm = name.Trim();
            var opts = options?.Where(o => !string.IsNullOrWhiteSpace(o)).ToList() ?? new List<string>();

            // Phần hiển thị thương hiệu
            string brandStr = br.Length > 0 ? $"{br} " : "";

            // Phần options
            string optLine = opts.Count > 0
                ? $" Sản phẩm đa dạng tùy chọn: {string.Join(", ", opts)}."
                : "";

            // Chọn template theo danh mục
            if (Contains(cat, "laptop", "máy tính xách tay", "notebook"))
                return Laptop(nm, brandStr, optLine);

            if (Contains(cat, "cpu", "vi xử lý", "processor"))
                return Cpu(nm, brandStr, optLine);

            if (Contains(cat, "gpu", "card", "vga", "đồ họa", "graphics"))
                return Gpu(nm, brandStr, optLine);

            if (Contains(cat, "ram", "bộ nhớ", "memory"))
                return Ram(nm, brandStr, optLine);

            if (Contains(cat, "ssd", "hdd", "ổ cứng", "storage", "nvme"))
                return Storage(nm, brandStr, optLine);

            if (Contains(cat, "màn hình", "monitor", "display"))
                return Monitor(nm, brandStr, optLine);

            if (Contains(cat, "chuột", "mouse"))
                return Mouse(nm, brandStr, optLine);

            if (Contains(cat, "bàn phím", "keyboard"))
                return Keyboard(nm, brandStr, optLine);

            if (Contains(cat, "tai nghe", "headset", "headphone"))
                return Headset(nm, brandStr, optLine);

            if (Contains(cat, "case", "thùng máy", "chassis"))
                return Case(nm, brandStr, optLine);

            if (Contains(cat, "nguồn", "psu", "power supply"))
                return Psu(nm, brandStr, optLine);

            if (Contains(cat, "tản nhiệt", "cooler", "cooling", "fan"))
                return Cooler(nm, brandStr, optLine);

            if (Contains(cat, "mainboard", "bo mạch", "motherboard"))
                return Motherboard(nm, brandStr, optLine);

            if (Contains(cat, "pc", "desktop", "máy tính để bàn", "bộ máy"))
                return Desktop(nm, brandStr, optLine);

            // Fallback chung
            return Generic(nm, brandStr, cat, optLine);
        }

        private static bool Contains(string src, params string[] keywords)
            => keywords.Any(k => src.Contains(k, StringComparison.OrdinalIgnoreCase));

        // ─── Templates ──────────────────────────────────────────────────────────

        private static string Laptop(string nm, string brand, string opts) =>
            $"{brand}{nm} là dòng laptop được thiết kế tối ưu cho nhu cầu làm việc, học tập và giải trí. " +
            $"Sản phẩm nổi bật với thiết kế mỏng nhẹ, màn hình sắc nét và hiệu năng ổn định, " +
            $"đáp ứng tốt các tác vụ văn phòng, đồ họa lẫn gaming. " +
            $"Pin trâu, tản nhiệt hiệu quả, bàn phím êm và kết nối đa dạng giúp mang lại trải nghiệm toàn diện." +
            opts;

        private static string Cpu(string nm, string brand, string opts) =>
            $"{brand}{nm} là bộ vi xử lý hiệu năng cao, phù hợp cho cả người dùng văn phòng lẫn game thủ đòi hỏi sức mạnh xử lý mạnh mẽ. " +
            $"Công nghệ sản xuất tiên tiến giúp tối ưu hóa hiệu suất trên mỗi watt điện năng tiêu thụ, " +
            $"đảm bảo máy tính vận hành mượt mà và ổn định ngay cả khi chạy đa nhiệm nặng." +
            opts;

        private static string Gpu(string nm, string brand, string opts) =>
            $"{brand}{nm} là card đồ họa mạnh mẽ, được thiết kế để xử lý mọi tác vụ đồ họa chuyên nghiệp và gaming đỉnh cao. " +
            $"Với kiến trúc hiện đại, bộ nhớ VRAM dung lượng lớn và tần số cao, sản phẩm cho phép chơi game ở độ phân giải cao " +
            $"hoặc render video, 3D một cách nhanh chóng và trơn tru." +
            opts;

        private static string Ram(string nm, string brand, string opts) =>
            $"{brand}{nm} là thanh RAM hiệu năng cao với tốc độ băng thông vượt trội, giúp hệ thống xử lý đa nhiệm mượt mà hơn. " +
            $"Độ trễ thấp và tương thích rộng với nhiều nền tảng mainboard, " +
            $"đây là lựa chọn nâng cấp lý tưởng cho cả máy tính gaming lẫn máy trạm chuyên dụng." +
            opts;

        private static string Storage(string nm, string brand, string opts) =>
            $"{brand}{nm} là ổ lưu trữ tốc độ cao, giúp rút ngắn thời gian khởi động hệ thống, " +
            $"tải ứng dụng và truyền dữ liệu. Với công nghệ NAND tiên tiến và giao diện kết nối nhanh, " +
            $"sản phẩm đem lại trải nghiệm lưu trữ đáng tin cậy và bền bỉ trong dài hạn." +
            opts;

        private static string Monitor(string nm, string brand, string opts) =>
            $"{brand}{nm} là màn hình với chất lượng hiển thị xuất sắc, tái tạo màu sắc chính xác và độ sáng cao. " +
            $"Tần số quét nhanh kết hợp thời gian phản hồi thấp giúp hình ảnh mượt mà, " +
            $"phù hợp cho cả thiết kế đồ họa, chỉnh sửa ảnh/video lẫn gaming cạnh tranh." +
            opts;

        private static string Mouse(string nm, string brand, string opts) =>
            $"{brand}{nm} là chuột gaming / văn phòng được trang bị cảm biến quang học độ nhạy cao, " +
            $"giúp di chuyển chính xác và nhạy bén. Thiết kế công thái học vừa tay, " +
            $"nút nhấn bền bỉ hàng chục triệu lần, kết nối ổn định và tùy chỉnh DPI linh hoạt." +
            opts;

        private static string Keyboard(string nm, string brand, string opts) =>
            $"{brand}{nm} là bàn phím cơ / membrane với kết cấu chắc chắn, gõ phím đầm tay và phản hồi rõ ràng. " +
            $"Hỗ trợ đèn nền RGB bắt mắt, switch đa dạng lựa chọn theo sở thích, " +
            $"phù hợp cho game thủ lẫn người dùng văn phòng cần gõ phím lâu." +
            opts;

        private static string Headset(string nm, string brand, string opts) =>
            $"{brand}{nm} là tai nghe gaming / âm nhạc với chất âm trung thực, bass sâu và chi tiết âm thanh rõ ràng. " +
            $"Đệm tai mềm mại, băng đầu điều chỉnh linh hoạt giúp đeo thoải mái trong thời gian dài. " +
            $"Microphone khử tiếng ồn hỗ trợ giao tiếp rõ ràng trong game và cuộc họp trực tuyến." +
            opts;

        private static string Case(string nm, string brand, string opts) =>
            $"{brand}{nm} là thùng máy tính được thiết kế thoáng khí tối ưu, hỗ trợ hệ thống tản nhiệt hiệu quả. " +
            $"Panel kính cường lực giúp khoe build một cách ấn tượng; " +
            $"không gian nội thất rộng rãi dễ dàng lắp đặt các linh kiện kích thước lớn." +
            opts;

        private static string Psu(string nm, string brand, string opts) =>
            $"{brand}{nm} là nguồn máy tính chứng nhận 80 PLUS, cung cấp điện năng ổn định và hiệu suất chuyển đổi cao. " +
            $"Bảo vệ đa lớp chống quá áp, quá dòng, ngắn mạch giúp bảo vệ toàn bộ linh kiện, " +
            $"kéo dài tuổi thọ hệ thống và giảm tiếng ồn quạt." +
            opts;

        private static string Cooler(string nm, string brand, string opts) =>
            $"{brand}{nm} là tản nhiệt CPU hiệu suất cao, giúp duy trì nhiệt độ ổn định ngay cả khi ép xung. " +
            $"Thiết kế ống dẫn nhiệt bằng đồng / hệ thống tản nhiệt nước đảm bảo tản nhiệt nhanh, " +
            $"quạt êm ái, lắp đặt dễ dàng trên nhiều socket phổ biến." +
            opts;

        private static string Motherboard(string nm, string brand, string opts) =>
            $"{brand}{nm} là bo mạch chủ trang bị các kết nối mở rộng phong phú và hỗ trợ CPU thế hệ mới nhất. " +
            $"VRM pha nhiều hỗ trợ ép xung bền vững, khe RAM tốc độ cao, " +
            $"cổng PCIe và M.2 đa dạng đáp ứng mọi nhu cầu nâng cấp hệ thống." +
            opts;

        private static string Desktop(string nm, string brand, string opts) =>
            $"{brand}{nm} là bộ máy tính để bàn được tối ưu hóa về hiệu năng và độ ổn định cho nhu cầu làm việc, " +
            $"học tập hoặc gaming. Linh kiện chính hãng được lựa chọn kỹ càng, " +
            $"đảm bảo tương thích hoàn hảo và bảo hành đầy đủ." +
            opts;

        private static string Generic(string nm, string brand, string cat, string opts)
        {
            string catDisplay = cat.Length > 0 ? $" thuộc danh mục {cat}" : "";
            return $"{brand}{nm}{catDisplay} là sản phẩm công nghệ chất lượng cao, " +
                   $"được sản xuất theo tiêu chuẩn nghiêm ngặt để đảm bảo hiệu suất ổn định và độ bền lâu dài. " +
                   $"Sản phẩm phù hợp cho cả người dùng cá nhân lẫn chuyên nghiệp, " +
                   $"mang đến trải nghiệm sử dụng thuận tiện và đáng tin cậy." +
                   opts;
        }

        // ─── Category templates ────────────────────────────────────────────────
        public static string BuildCategory(string name)
        {
            var nm = name.Trim();
            var lower = nm.ToLower();

            if (Contains(lower, "laptop", "máy tính xách tay", "notebook"))
                return $"Danh mục {nm} tập hợp các dòng laptop từ phổ thông đến cao cấp, đáp ứng nhu cầu làm việc, học tập và giải trí. " +
                       $"Đa dạng mẫu mã, cấu hình linh hoạt từ nhiều thương hiệu uy tín, " +
                       $"giúp bạn dễ dàng lựa chọn chiếc laptop phù hợp với ngân sách và nhu cầu sử dụng.";

            if (Contains(lower, "cpu", "vi xử lý", "processor"))
                return $"Danh mục {nm} cung cấp đầy đủ các loại vi xử lý từ Intel và AMD cho mọi nhu cầu, " +
                       $"từ máy tính văn phòng đến máy trạm chuyên dụng và hệ thống gaming hiệu năng cao. " +
                       $"Sản phẩm chính hãng, được bảo hành đầy đủ, đảm bảo hiệu suất và độ ổn định.";

            if (Contains(lower, "gpu", "card đồ họa", "vga"))
                return $"Danh mục {nm} cung cấp đa dạng card đồ họa từ NVIDIA và AMD cho mọi phân khúc. " +
                       $"Từ card phổ thông cho văn phòng đến card cao cấp cho gaming 4K và render đồ họa chuyên nghiệp, " +
                       $"đáp ứng mọi nhu cầu xử lý hình ảnh và video.";

            if (Contains(lower, "ram", "bộ nhớ", "memory"))
                return $"Danh mục {nm} bao gồm các sản phẩm RAM từ các thương hiệu hàng đầu với nhiều dung lượng và tốc độ khác nhau. " +
                       $"Tương thích rộng với nhiều nền tảng mainboard, giúp nâng cao hiệu năng hệ thống " +
                       $"và cải thiện khả năng xử lý đa nhiệm đáng kể.";

            if (Contains(lower, "ssd", "hdd", "ổ cứng", "storage", "nvme"))
                return $"Danh mục {nm} cung cấp các giải pháp lưu trữ đa dạng từ HDD dung lượng lớn đến SSD tốc độ cao. " +
                       $"Sản phẩm từ các thương hiệu uy tín như Samsung, WD, Seagate, Crucial, " +
                       $"đảm bảo an toàn dữ liệu và hiệu suất truyền tải vượt trội.";

            if (Contains(lower, "màn hình", "monitor", "display"))
                return $"Danh mục {nm} tập hợp các màn hình từ FHD đến 4K, tần số quét từ 60Hz đến 360Hz. " +
                       $"Đáp ứng mọi nhu cầu từ văn phòng, thiết kế đồ họa đến gaming cạnh tranh, " +
                       $"với màu sắc chính xác và hình ảnh sắc nét trên nhiều kích cỡ màn hình.";

            if (Contains(lower, "chuột", "mouse"))
                return $"Danh mục {nm} cung cấp đa dạng chuột từ loại văn phòng đơn giản đến chuột gaming chuyên nghiệp. " +
                       $"Cảm biến độ nhạy cao, thiết kế công thái học thoải mái khi sử dụng lâu dài, " +
                       $"kết nối có dây và không dây phù hợp với mọi phong cách làm việc.";

            if (Contains(lower, "bàn phím", "keyboard"))
                return $"Danh mục {nm} bao gồm bàn phím cơ, membrane và hybrid từ các thương hiệu nổi tiếng. " +
                       $"Đa dạng switch, kích thước và layout từ full-size đến 60%, " +
                       $"phù hợp cho cả game thủ chuyên nghiệp lẫn người dùng văn phòng.";

            if (Contains(lower, "tai nghe", "headset", "headphone"))
                return $"Danh mục {nm} cung cấp các sản phẩm âm thanh chất lượng cao từ tai nghe in-ear đến over-ear. " +
                       $"Chất âm trung thực, microphone khử ồn hiệu quả, thiết kế thoải mái cho trải nghiệm nghe nhạc, " +
                       $"chơi game và họp trực tuyến toàn diện.";

            if (Contains(lower, "mainboard", "bo mạch", "motherboard"))
                return $"Danh mục {nm} cung cấp đầy đủ các bo mạch chủ cho Intel và AMD với nhiều chipset khác nhau. " +
                       $"Từ bo mạch phổ thông cho máy tính văn phòng đến bo mạch cao cấp hỗ trợ ép xung, " +
                       $"đảm bảo nền tảng ổn định cho mọi hệ thống.";

            if (Contains(lower, "nguồn", "psu", "power supply"))
                return $"Danh mục {nm} cung cấp các bộ nguồn máy tính đạt chuẩn 80 PLUS với nhiều công suất khác nhau. " +
                       $"Hiệu suất cao, ổn định, có đủ kết nối cho mọi cấu hình, " +
                       $"bảo vệ hệ thống toàn diện với nhiều cơ chế an toàn tích hợp.";

            if (Contains(lower, "tản nhiệt", "cooler", "cooling", "fan"))
                return $"Danh mục {nm} tập hợp các giải pháp tản nhiệt từ quạt khí đến tản nhiệt nước AIO và custom. " +
                       $"Giúp duy trì nhiệt độ CPU/GPU ổn định, tăng tuổi thọ linh kiện, " +
                       $"hoạt động êm ái và hiệu quả ngay cả khi tải nặng.";

            if (Contains(lower, "case", "thùng máy", "chassis"))
                return $"Danh mục {nm} cung cấp đa dạng thùng máy tính từ Mini-ITX đến Full Tower. " +
                       $"Thiết kế thoáng khí, hỗ trợ nhiều cấu hình tản nhiệt, " +
                       $"nhiều mẫu mã hiện đại với panel kính cường lực để khoe build ấn tượng.";

            // Generic
            return $"Danh mục {nm} là nơi tập hợp các sản phẩm công nghệ chất lượng cao, " +
                   $"được tuyển chọn kỹ lưỡng từ các thương hiệu uy tín trên thị trường. " +
                   $"Đa dạng mẫu mã và phân khúc giá, đáp ứng nhu cầu của mọi đối tượng khách hàng " +
                   $"từ người dùng phổ thông đến chuyên nghiệp.";
        }

        // ─── Brand templates ───────────────────────────────────────────────────
        public static string BuildBrand(string name)
        {
            var nm = name.Trim();
            var lower = nm.ToLower();

            var brandProfiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["asus"] = $"{nm} là thương hiệu công nghệ hàng đầu đến từ Đài Loan, nổi tiếng với các sản phẩm laptop gaming ROG, TUF, mainboard và linh kiện máy tính chất lượng cao. Cam kết đổi mới không ngừng, {nm} mang đến hiệu suất vượt trội và độ bền bỉ được kiểm chứng qua hàng triệu người dùng trên toàn thế giới.",
                ["msi"] = $"{nm} (Micro-Star International) là thương hiệu công nghệ gaming danh tiếng từ Đài Loan, chuyên cung cấp laptop, mainboard, card đồ họa và phụ kiện gaming cao cấp. Các dòng sản phẩm {nm} được tin dùng bởi game thủ chuyên nghiệp và eSports toàn cầu nhờ hiệu năng mạnh mẽ và thiết kế đẳng cấp.",
                ["acer"] = $"{nm} là tập đoàn công nghệ đa quốc gia với hơn 40 năm kinh nghiệm, cung cấp đa dạng sản phẩm từ laptop phổ thông đến gaming Predator/Nitro. {nm} luôn hướng đến mục tiêu mang công nghệ chất lượng đến với mọi người dùng với mức giá hợp lý.",
                ["dell"] = $"{nm} là một trong những nhà sản xuất máy tính lớn nhất thế giới, nổi tiếng với độ bền và dịch vụ hậu mãi xuất sắc. Các dòng laptop {nm} XPS, Inspiron, Alienware đáp ứng từ nhu cầu văn phòng đến gaming chuyên nghiệp với chất lượng được đảm bảo.",
                ["hp"] = $"{nm} (Hewlett-Packard) là thương hiệu công nghệ hàng đầu thế giới với danh mục sản phẩm đa dạng từ laptop, máy tính để bàn đến máy in. {nm} mang đến sự tin cậy và hiệu suất ổn định cho cả người dùng cá nhân lẫn doanh nghiệp.",
                ["lenovo"] = $"{nm} là tập đoàn công nghệ đa quốc gia hàng đầu, nổi bật với dòng ThinkPad cho doanh nghiệp và IdeaPad, Legion cho người dùng phổ thông và game thủ. {nm} kết hợp giữa thiết kế tinh tế, hiệu suất cao và giá trị bền vững.",
                ["samsung"] = $"{nm} là tập đoàn điện tử hàng đầu Hàn Quốc, cung cấp đa dạng sản phẩm từ màn hình, SSD đến DRAM chất lượng cao. Công nghệ sản xuất tiên tiến của {nm} đảm bảo hiệu suất vượt trội và tuổi thọ sản phẩm dài hạn.",
                ["logitech"] = $"{nm} là thương hiệu Thụy Sĩ chuyên về thiết bị ngoại vi máy tính, nổi tiếng với chuột, bàn phím, tai nghe và webcam chất lượng cao. Các sản phẩm {nm} được tin dùng bởi cả người dùng phổ thông lẫn game thủ chuyên nghiệp toàn cầu.",
                ["razer"] = $"{nm} là thương hiệu gaming cao cấp đến từ Singapore, chuyên cung cấp laptop, chuột, bàn phím và tai nghe gaming hàng đầu. Với công nghệ Chroma RGB độc quyền và hiệu suất vượt trội, {nm} là lựa chọn hàng đầu của game thủ chuyên nghiệp.",
                ["corsair"] = $"{nm} là thương hiệu Mỹ uy tín chuyên về RAM, SSD, nguồn máy tính, case và phụ kiện gaming. {nm} nổi tiếng với chất lượng ổn định, hiệu suất cao và hệ sinh thái iCUE kiểm soát đèn RGB toàn diện.",
                ["gigabyte"] = $"{nm} là nhà sản xuất linh kiện máy tính lớn từ Đài Loan, cung cấp mainboard, card đồ họa AORUS, laptop và các phụ kiện cao cấp. {nm} nổi bật với thiết kế kỹ lưỡng, tính năng phong phú và độ bền cao cấp.",
                ["intel"] = $"{nm} là tập đoàn bán dẫn hàng đầu thế giới, nhà sản xuất vi xử lý và chip đồ họa Arc. Các thế hệ CPU {nm} Core i3/i5/i7/i9 được tin dùng rộng rãi trong máy tính cá nhân, máy chủ và thiết bị di động trên toàn cầu.",
                ["amd"] = $"{nm} (Advanced Micro Devices) là nhà sản xuất vi xử lý và GPU hàng đầu thế giới. Dòng CPU Ryzen và GPU Radeon của {nm} mang đến hiệu suất cạnh tranh với mức giá hấp dẫn, phù hợp cho cả người dùng phổ thông lẫn chuyên nghiệp.",
                ["nvidia"] = $"{nm} là công ty công nghệ hàng đầu thế giới về GPU và AI. Các dòng card đồ họa GeForce RTX của {nm} dẫn đầu về hiệu năng gaming và xử lý đồ họa chuyên nghiệp, hỗ trợ ray tracing và DLSS công nghệ tiên tiến nhất hiện nay.",
                ["kingston"] = $"{nm} là thương hiệu lưu trữ và bộ nhớ uy tín hàng đầu thế giới từ Mỹ. Các sản phẩm RAM, SSD và USB của {nm} được biết đến với độ tin cậy cao, hiệu suất ổn định và chính sách bảo hành dài hạn.",
                ["seagate"] = $"{nm} là nhà sản xuất ổ cứng lớn nhất thế giới từ Mỹ, cung cấp HDD và SSD cho mọi nhu cầu lưu trữ. {nm} nổi tiếng với dung lượng lưu trữ lớn, độ tin cậy cao và giải pháp backup dữ liệu toàn diện.",
                ["western digital"] = $"Western Digital (WD) là thương hiệu lưu trữ hàng đầu thế giới, cung cấp đa dạng HDD và SSD cho cả người dùng cá nhân lẫn doanh nghiệp. Các sản phẩm WD Blue, Green, Red, Black và Purple đáp ứng từng nhu cầu lưu trữ cụ thể.",
                ["wd"] = $"Western Digital (WD) là thương hiệu lưu trữ hàng đầu thế giới, cung cấp đa dạng HDD và SSD cho cả người dùng cá nhân lẫn doanh nghiệp. Các sản phẩm WD đáp ứng từng nhu cầu lưu trữ từ gia đình đến máy chủ doanh nghiệp.",
                ["steelseries"] = $"{nm} là thương hiệu gaming chuyên nghiệp từ Đan Mạch, cung cấp chuột, bàn phím, tai nghe và phụ kiện gaming cao cấp. Sản phẩm {nm} được lựa chọn bởi các đội eSports chuyên nghiệp nhờ độ chính xác và độ bền vượt trội.",
            };

            if (brandProfiles.TryGetValue(lower, out var profile))
                return profile;

            // Generic brand description
            return $"{nm} là thương hiệu công nghệ uy tín, được biết đến với các sản phẩm chất lượng cao và dịch vụ hậu mãi tốt. " +
                   $"Với nhiều năm kinh nghiệm trong ngành công nghệ, {nm} không ngừng đổi mới và cải tiến " +
                   $"để mang đến những sản phẩm đáp ứng nhu cầu ngày càng cao của người dùng.";
        }
    }
}
