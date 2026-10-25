using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Entity
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public int MaterialTypeId { get; set; }
        public int UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public decimal PackageQuantity { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(UnitId))]
        public virtual Unit Unit { get; set; } = null!;
        [ForeignKey(nameof(MaterialTypeId))]
        public virtual MaterialType MaterialType { get; set; } = null!;
    }


    public class Unit
    {
        [Key]
        public int UnitId { get; set; }
        public string Symbol { get; set; } = null!;
        public string UnitName { get; set; } = null!;

        public virtual ICollection<Material> materials { get; set; } = null!;
        public override string ToString()
        {
            return UnitName;
        }
    }

    public class MaterialType
    {
        [Key]
        public int MaterialTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public decimal LossPercent { get; set; }

        public virtual ICollection<Material> materials { get; set; } = null!;

        public override string ToString()
        {
            return TypeName;
        }
    }

    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Article { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int ProductTypeId { get; set; }
        public decimal MinimumPartnerPrice { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(ProductTypeId))]
        public virtual ProductType ProductType { get; set; } = null!;

        public override string ToString()
        {
            return $"({Article}) {ProductName}";
        }
    }

    public class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public decimal TypeCoefficient { get; set; }


        public virtual ICollection<Product> Product { get; set; } = null!;
    }

    [PrimaryKey(nameof(ProductId), nameof(MaterialId))]
    public class ProductMaterial
    {
        public int ProductId { get; set; }
        public int MaterialId {  get; set; }
        public decimal QuantityPerUnit { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;
        [ForeignKey(nameof(MaterialId))]
        public virtual Material Material { get; set; } = null!;

    }
}
