using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TearGearAPI.Migrations
{
    /// <inheritdoc />
    public partial class Seed5Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "Email", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "d4e0542f-2685-4558-a41f-ae590896fe1e", "demo1@gmail.com", "Demo User 1", "DEMO1@GMAIL.COM", "DEMO1", "AQAAAAIAAYagAAAAEDSWb701hsafCOB1ln+/C3yT3LwoqUm7ab0fq8AxQ6P89B6fA04y2rZboHHRUbDzMA==", "ba04c214-0082-4556-9f09-81b37f61014f", "demo1" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user2", 0, "ab317806-653d-4abd-91e4-94be264e3241", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "demo2@gmail.com", true, "Demo User 2", false, false, null, "DEMO2@GMAIL.COM", "DEMO2", "AQAAAAIAAYagAAAAEM/tx5rmcYc4v+BcklvhS9Voc82cc1t1gZ5X6KTVX/7X6BOaIKOceOvToNG9gyFVag==", null, false, "2fae543b-6beb-4e91-b040-fc6423ca2bba", false, "demo2" },
                    { "user3", 0, "26394bfc-d58a-40f1-9f2c-cc99e05650e2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "demo3@gmail.com", true, "Demo User 3", false, false, null, "DEMO3@GMAIL.COM", "DEMO3", "AQAAAAIAAYagAAAAEB/rmpwjhk/1/+hl+OiD3tTKAQhEV/kt99aH2DN4G94EmFdGpfroRn1uGElG83068g==", null, false, "d48c24cd-5664-4ffc-a2c3-2a60e0994ce3", false, "demo3" },
                    { "user4", 0, "7340b77e-d29b-497c-8575-84883b3d3a88", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "demo4@gmail.com", true, "Demo User 4", false, false, null, "DEMO4@GMAIL.COM", "DEMO4", "AQAAAAIAAYagAAAAEDhyf9PQozw6SYc2TMV5rLol++UJBXVXBgTn3ZTrHanvjg6WLsfKeBemEXflOlHYuQ==", null, false, "a4f15736-bf8b-40e5-b3b0-37e3c6cdd4b2", false, "demo4" },
                    { "user5", 0, "d37d306a-c95a-42ec-a43a-105f29a3f688", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "demo5@gmail.com", true, "Demo User 5", false, false, null, "DEMO5@GMAIL.COM", "DEMO5", "AQAAAAIAAYagAAAAEBpxtjDoZBRmOpzTYewp7PhewrEMWJVSG6X7OTQhPgFZ5Z0RhkAhnHFK8C2IjvtKXQ==", null, false, "59bb3e0b-c9f1-430a-b612-af65f40694e2", false, "demo5" }
                });

            migrationBuilder.InsertData(
                table: "brandAPIs",
                columns: new[] { "BrandId", "BrandName", "Description", "IsActive", "LogoUrl" },
                values: new object[,]
                {
                    { 2, "Logitech", null, true, null },
                    { 3, "Razer", null, true, null },
                    { 4, "Dell", null, true, null },
                    { 5, "Sony", null, true, null }
                });

            migrationBuilder.UpdateData(
                table: "cartItemAPIs",
                keyColumn: "CartItemId",
                keyValue: 1,
                column: "Quantity",
                value: 1);

            migrationBuilder.InsertData(
                table: "categoryAPIs",
                columns: new[] { "CategoryId", "CategoryName", "CreatedAt", "Description", "IsActive" },
                values: new object[,]
                {
                    { 2, "Mouse", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true },
                    { 3, "Keyboard", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true },
                    { 4, "Monitor", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true },
                    { 5, "Headphone", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true }
                });

            migrationBuilder.UpdateData(
                table: "comboAPIs",
                keyColumn: "ComboId",
                keyValue: 1,
                columns: new[] { "ComboName", "Description", "DiscountValue", "FinalPrice" },
                values: new object[] { "Gaming Set 1", null, 5m, 21000000m });

            migrationBuilder.InsertData(
                table: "comboAPIs",
                columns: new[] { "ComboId", "ComboItemId", "ComboName", "CreatedAt", "Description", "DiscountType", "DiscountValue", "FinalPrice", "IsActive", "OriginalPrice" },
                values: new object[,]
                {
                    { 2, 0, "Gaming Set 2", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 10m, 2700000m, true, 3000000m },
                    { 3, 0, "Office Set", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 500000m, 5500000m, true, 6000000m },
                    { 4, 0, "Streamer Set", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 500000m, 6500000m, true, 7000000m },
                    { 5, 0, "Audio Set", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 7m, 2800000m, true, 3000000m }
                });

            migrationBuilder.UpdateData(
                table: "orderAPIs",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "DiscountAmount", "SubTotal", "TotalAmount" },
                values: new object[] { 0m, 20000000m, 20050000m });

            migrationBuilder.UpdateData(
                table: "orderDetailAPIs",
                keyColumn: "OrderDetailId",
                keyValue: 1,
                columns: new[] { "ProductName", "Quantity" },
                values: new object[] { "Laptop ASUS ROG", 1 });

            migrationBuilder.UpdateData(
                table: "paymentAPIs",
                keyColumn: "PaymentId",
                keyValue: 1,
                column: "Amount",
                value: 20050000m);

            migrationBuilder.UpdateData(
                table: "productAPIs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "laptop1.jpg", "Laptop ASUS ROG" });

            migrationBuilder.UpdateData(
                table: "productVariantAPIs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SKU", "Stock" },
                values: new object[] { "SKU-001", 10 });

            migrationBuilder.UpdateData(
                table: "userAddressAPIs",
                keyColumn: "UserAddressId",
                keyValue: 1,
                columns: new[] { "AddressLine", "FullName", "PhoneNumber", "Ward" },
                values: new object[] { "HCM", "User 1", "111", "Ward 1" });

            migrationBuilder.InsertData(
                table: "productAPIs",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 2, 2, 2, null, "mouse.jpg", "Mouse Logitech G102" },
                    { 3, 3, 3, null, "keyboard.jpg", "Keyboard Razer" },
                    { 4, 4, 4, null, "monitor.jpg", "Monitor Dell 27\"" },
                    { 5, 5, 5, null, "headphone.jpg", "Sony Headphone" }
                });

            migrationBuilder.InsertData(
                table: "userAddressAPIs",
                columns: new[] { "UserAddressId", "AddressLine", "District", "FullName", "IsDefault", "PhoneNumber", "Province", "UserId", "Ward" },
                values: new object[,]
                {
                    { 2, "HN", "Ba Dinh", "User 2", true, "222", "HN", "user2", "Ward 2" },
                    { 3, "DN", "Hai Chau", "User 3", true, "333", "DN", "user3", "Ward 3" },
                    { 4, "CT", "Ninh Kieu", "User 4", true, "444", "CT", "user4", "Ward 4" },
                    { 5, "HP", "Hong Bang", "User 5", true, "555", "HP", "user5", "Ward 5" }
                });

            migrationBuilder.InsertData(
                table: "orderAPIs",
                columns: new[] { "OrderId", "CreatedAt", "DiscountAmount", "OrderDate", "ShippingFee", "Status", "SubTotal", "TotalAmount", "UserAddressId", "UserId" },
                values: new object[,]
                {
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30000m, 1, 500000m, 530000m, 2, "user2" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30000m, 1, 1500000m, 1530000m, 3, "user3" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50000m, 1, 4000000m, 4050000m, 4, "user4" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50000m, 1, 2500000m, 2550000m, 5, "user5" }
                });

            migrationBuilder.InsertData(
                table: "productVariantAPIs",
                columns: new[] { "Id", "Price", "ProductId", "ProductVariantId", "SKU", "Stock" },
                values: new object[,]
                {
                    { 2, 500000m, 2, 0, "SKU-002", 50 },
                    { 3, 1500000m, 3, 0, "SKU-003", 30 },
                    { 4, 4000000m, 4, 0, "SKU-004", 20 },
                    { 5, 2500000m, 5, 0, "SKU-005", 40 }
                });

            migrationBuilder.InsertData(
                table: "cartItemAPIs",
                columns: new[] { "CartItemId", "ProductVariantId", "Quantity", "UserId" },
                values: new object[,]
                {
                    { 2, 2, 2, "user2" },
                    { 3, 3, 1, "user3" },
                    { 4, 4, 1, "user4" },
                    { 5, 5, 3, "user5" }
                });

            migrationBuilder.InsertData(
                table: "comboItems",
                columns: new[] { "ComboItemId", "ComboId", "ProductVariantId", "Quantity" },
                values: new object[,]
                {
                    { 2, 2, 2, 1 },
                    { 3, 3, 3, 1 },
                    { 4, 4, 4, 1 },
                    { 5, 5, 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "orderDetailAPIs",
                columns: new[] { "OrderDetailId", "OrderId", "ProductName", "ProductVariantId", "Quantity", "UnitPrice", "VariantDescription" },
                values: new object[,]
                {
                    { 2, 2, "Mouse Logitech", 2, 1, 500000m, "Default" },
                    { 3, 3, "Keyboard Razer", 3, 1, 1500000m, "Default" },
                    { 4, 4, "Monitor Dell", 4, 1, 4000000m, "Default" },
                    { 5, 5, "Sony Headphone", 5, 1, 2500000m, "Default" }
                });

            migrationBuilder.InsertData(
                table: "paymentAPIs",
                columns: new[] { "PaymentId", "Amount", "Method", "OrderId", "PaymentDate", "Status", "TransactionId" },
                values: new object[,]
                {
                    { 2, 530000m, 1, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TRANS002" },
                    { 3, 1530000m, 1, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TRANS003" },
                    { 4, 4050000m, 1, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TRANS004" },
                    { 5, 2550000m, 1, 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TRANS005" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "cartItemAPIs",
                keyColumn: "CartItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "cartItemAPIs",
                keyColumn: "CartItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "cartItemAPIs",
                keyColumn: "CartItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "cartItemAPIs",
                keyColumn: "CartItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "comboItems",
                keyColumn: "ComboItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "comboItems",
                keyColumn: "ComboItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "comboItems",
                keyColumn: "ComboItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "comboItems",
                keyColumn: "ComboItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "orderDetailAPIs",
                keyColumn: "OrderDetailId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "orderDetailAPIs",
                keyColumn: "OrderDetailId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "orderDetailAPIs",
                keyColumn: "OrderDetailId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "orderDetailAPIs",
                keyColumn: "OrderDetailId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "paymentAPIs",
                keyColumn: "PaymentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "paymentAPIs",
                keyColumn: "PaymentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "paymentAPIs",
                keyColumn: "PaymentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "paymentAPIs",
                keyColumn: "PaymentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "comboAPIs",
                keyColumn: "ComboId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "comboAPIs",
                keyColumn: "ComboId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "comboAPIs",
                keyColumn: "ComboId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "comboAPIs",
                keyColumn: "ComboId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "orderAPIs",
                keyColumn: "OrderId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "orderAPIs",
                keyColumn: "OrderId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "orderAPIs",
                keyColumn: "OrderId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "orderAPIs",
                keyColumn: "OrderId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "productVariantAPIs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "productVariantAPIs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "productVariantAPIs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "productVariantAPIs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "productAPIs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "productAPIs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "productAPIs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "productAPIs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "userAddressAPIs",
                keyColumn: "UserAddressId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "userAddressAPIs",
                keyColumn: "UserAddressId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "userAddressAPIs",
                keyColumn: "UserAddressId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "userAddressAPIs",
                keyColumn: "UserAddressId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5");

            migrationBuilder.DeleteData(
                table: "brandAPIs",
                keyColumn: "BrandId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "brandAPIs",
                keyColumn: "BrandId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "brandAPIs",
                keyColumn: "BrandId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "brandAPIs",
                keyColumn: "BrandId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "categoryAPIs",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categoryAPIs",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "categoryAPIs",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "categoryAPIs",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "Email", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "85a43f12-df70-4b1d-94b7-684bf72311eb", "demo@gmail.com", "Demo User", "DEMO@GMAIL.COM", "DEMO", "AQAAAAIAAYagAAAAED2r5Icm92QPZNB0J/kyEomYDARjYlnvszidOxgqB74kUcEcC14qRZp+bWIvh7MrUA==", "6e7e51eb-b1ed-45a0-a758-9ee18099678f", "demo" });

            migrationBuilder.UpdateData(
                table: "cartItemAPIs",
                keyColumn: "CartItemId",
                keyValue: 1,
                column: "Quantity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "comboAPIs",
                keyColumn: "ComboId",
                keyValue: 1,
                columns: new[] { "ComboName", "Description", "DiscountValue", "FinalPrice" },
                values: new object[] { "Combo Laptop Basic", "Laptop + ưu đãi", 10m, 20000000m });

            migrationBuilder.UpdateData(
                table: "orderAPIs",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "DiscountAmount", "SubTotal", "TotalAmount" },
                values: new object[] { 1000000m, 40000000m, 39050000m });

            migrationBuilder.UpdateData(
                table: "orderDetailAPIs",
                keyColumn: "OrderDetailId",
                keyValue: 1,
                columns: new[] { "ProductName", "Quantity" },
                values: new object[] { "Laptop Gaming ASUS", 2 });

            migrationBuilder.UpdateData(
                table: "paymentAPIs",
                keyColumn: "PaymentId",
                keyValue: 1,
                column: "Amount",
                value: 39050000m);

            migrationBuilder.UpdateData(
                table: "productAPIs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageUrl", "Name" },
                values: new object[] { "laptop.jpg", "Laptop Gaming ASUS" });

            migrationBuilder.UpdateData(
                table: "productVariantAPIs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SKU", "Stock" },
                values: new object[] { "ASUS-001", 20 });

            migrationBuilder.UpdateData(
                table: "userAddressAPIs",
                keyColumn: "UserAddressId",
                keyValue: 1,
                columns: new[] { "AddressLine", "FullName", "PhoneNumber", "Ward" },
                values: new object[] { "HCM City", "Demo User", "0123456789", "Ben Nghe" });
        }
    }
}
