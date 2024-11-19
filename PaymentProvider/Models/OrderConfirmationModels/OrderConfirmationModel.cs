﻿using System.ComponentModel.DataAnnotations;

namespace PaymentProvider.Models.OrderConfirmationModels
{
    public class OrderConfirmationModel
    {

        [Required]
        public string ReceivingEmail { get; set; } = null!;
        [Required]
        public ShippingInformation Shipping { get; set; } = null!;
        [Required]
        public InvoiceInformation Invoice { get; set; } = null!;
        [Required]
        public List<ProductModel> Products { get; set; } = null!;
        [Required]
        [Range(1, 10_000_000)]
        public decimal OrderTotal { get; set; }

    }
}