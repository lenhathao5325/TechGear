namespace TearGearAPI.DTO
{
    /// <summary>
    /// DTO ?? c?p nh?t S? L??NG c?a chi ti?t ??n hàng
    /// Ch? cho phép c?p nh?t Quantity khi Order.Status == Pending
    /// KHÔNG cho phép s?a OrderId, ProductVariantId, ProductName, VariantDescription, UnitPrice
    /// </summary>
    public class UpdateOrderDetailQuantityDTO
    {
        public int Quantity { get; set; }
        // Ch? c?n truy?n s? l??ng m?i
        // T?t c? các tr??ng khác không th? thay ??i sau khi t?o
    }
}
