using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TearGearAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "user1", 0, "85a43f12-df70-4b1d-94b7-684bf72311eb", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "demo@gmail.com", true, "Demo User", false, false, null, "DEMO@GMAIL.COM", "DEMO", "AQAAAAIAAYagAAAAED2r5Icm92QPZNB0J/kyEomYDARjYlnvszidOxgqB74kUcEcC14qRZp+bWIvh7MrUA==", null, false, "6e7e51eb-b1ed-45a0-a758-9ee18099678f", false, "demo" });

            migrationBuilder.InsertData(
                table: "brandAPIs",
                columns: new[] { "BrandId", "BrandName", "Description", "IsActive", "LogoUrl" },
                values: new object[] { 1, "ASUS", null, true, null });

            migrationBuilder.InsertData(
                table: "categoryAPIs",
                columns: new[] { "CategoryId", "CategoryName", "CreatedAt", "Description", "IsActive" },
                values: new object[] { 1, "Laptop", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true });

            migrationBuilder.InsertData(
                table: "comboAPIs",
                columns: new[] { "ComboId", "ComboItemId", "ComboName", "CreatedAt", "Description", "DiscountType", "DiscountValue", "FinalPrice", "IsActive", "OriginalPrice" },
                values: new object[] { 1, 0, "Combo Laptop Basic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop + ưu đãi", 1, 10m, 20000000m, true, 22000000m });

            migrationBuilder.InsertData(
                table: "productAPIs",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "ImageUrl", "Name" },
                values: new object[] { 1, 1, 1, null, "laptop.jpg", "Laptop Gaming ASUS" });

            migrationBuilder.InsertData(
                table: "userAddressAPIs",
                columns: new[] { "UserAddressId", "AddressLine", "District", "FullName", "IsDefault", "PhoneNumber", "Province", "UserId", "Ward" },
                values: new object[] { 1, "HCM City", "Q1", "Demo User", true, "0123456789", "HCM", "user1", "Ben Nghe" });

            migrationBuilder.InsertData(
                table: "orderAPIs",
                columns: new[] { "OrderId", "CreatedAt", "DiscountAmount", "OrderDate", "ShippingFee", "Status", "SubTotal", "TotalAmount", "UserAddressId", "UserId" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50000m, 1, 40000000m, 39050000m, 1, "user1" });

            migrationBuilder.InsertData(
                table: "productVariantAPIs",
                columns: new[] { "Id", "Price", "ProductId", "ProductVariantId", "SKU", "Stock" },
                values: new object[] { 1, 20000000m, 1, 0, "ASUS-001", 20 });

            migrationBuilder.InsertData(
                table: "cartItemAPIs",
                columns: new[] { "CartItemId", "ProductVariantId", "Quantity", "UserId" },
                values: new object[] { 1, 1, 2, "user1" });

            migrationBuilder.InsertData(
                table: "comboItems",
                columns: new[] { "ComboItemId", "ComboId", "ProductVariantId", "Quantity" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "orderDetailAPIs",
                columns: new[] { "OrderDetailId", "OrderId", "ProductName", "ProductVariantId", "Quantity", "UnitPrice", "VariantDescription" },
                values: new object[] { 1, 1, "Laptop Gaming ASUS", 1, 2, 20000000m, "Default" });

            migrationBuilder.InsertData(
                table: "paymentAPIs",
                columns: new[] { "PaymentId", "Amount", "Method", "OrderId", "PaymentDate", "Status", "TransactionId" },
                values: new object[] { 1, 39050000m, 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TRANS001" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "cartItemAPIs",
                keyColumn: "CartItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "comboItems",
                keyColumn: "ComboItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "orderDetailAPIs",
                keyColumn: "OrderDetailId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "paymentAPIs",
                keyColumn: "PaymentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "comboAPIs",
                keyColumn: "ComboId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "orderAPIs",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "productVariantAPIs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "productAPIs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "userAddressAPIs",
                keyColumn: "UserAddressId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1");

            migrationBuilder.DeleteData(
                table: "brandAPIs",
                keyColumn: "BrandId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categoryAPIs",
                keyColumn: "CategoryId",
                keyValue: 1);
        }
    }
}
