using Client.Models.Enums;
using System.Reflection;

namespace Client.Models
{
    public static class CategoryMap
    {
        public static Dictionary<Category, List<SubCategory>> Data = new()
    {
        {
            Category.HouseholdAppliances,
            new List<SubCategory>
            {
                SubCategory.Fridges,
                SubCategory.WashingMachines,
                SubCategory.WaterHeaters,
                SubCategory.Ovens,
                SubCategory.CookerHoods,
                SubCategory.MicrowaveOvens
            }
        },
        {
            Category.ComputerEquipment,
            new List<SubCategory>
            {
                SubCategory.Laptops,
                SubCategory.PC,
                SubCategory.Monitors,
                SubCategory.Printers,
                SubCategory.Scanners
            }
        },
        {
                Category.Smartphones,
                new List<SubCategory>
                {
                    SubCategory.AndroidSmartphones,
                    SubCategory.IosSmartphones
                }
         },
            {
                Category.Other,
                new List<SubCategory>
                {
                    SubCategory.Clothes,
                    SubCategory.Shoes,
                    SubCategory.Accessory,
                    SubCategory.SportEquipment,
                    SubCategory.Toys
                }
            }
    };
    }
}
