using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillBoard.Models
{
    [Table("Adverts")]
    public class Advert
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int AdvertID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        public int CategoryID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int TypeID { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите заголовок объявления")]
        [DisplayName("Заголовок")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Пожалуйста, введите описание")]
        [DisplayName("Описание")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительную цену")]
        [DisplayName("Цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите контактную информацию")]
        [DisplayName("Контактная информация")]
        public string ContactInfo { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        public Category Category { get; set; }

        public Type Type { get; set; }
    }

    [Table("Categories")]
    public class Category
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [DisplayName("Категория")]
        public string Name { get; set; }

        public List<Advert> Adverts { get; set; }
    }

    [Table("Type")]
    public class Type
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }

        [DisplayName("Тип")]
        public string Name { get; set; }

        public List<Advert> Adverts { get; set; }
    }
}