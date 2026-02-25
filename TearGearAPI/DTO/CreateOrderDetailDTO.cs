namespace TechGearAPI.DTO
{
    /// <summary>
    /// DTO ?? t?o chi ti?t ??n hàng
    /// ProductName và VariantDescription ???c tính t? ??ng t? ProductVariant
    /// UnitPrice là snapshot giá s?n ph?m t?i th?i ?i?m mua
    /// </summary>
    public class CreateOrderDetailDTO
    {
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        // ProductName ???c l?y t? ProductVariant.Product.Name
        // VariantDescription ???c l?y t? ProductVariant.SKU
    }
}
