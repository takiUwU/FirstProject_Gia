using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FirstProject.Entity;
using Microsoft.EntityFrameworkCore;

namespace FirstProject
{
    /// <summary>
    /// Логика взаимодействия для MaterialCreation.xaml
    /// </summary>
    public partial class MaterialCreation : Window
    {
        int Id = 0;
        private bool EditMode = false;
        public MaterialCreation()
        {
            InitializeComponent();
            ComboBoxType.ItemsSource = ApplicationCore.context.MaterialTypes.ToList();
            ComboBoxUnit.ItemsSource = ApplicationCore.context.Units.ToList();
            ComboBoxType.SelectedIndex = 0;
            ComboBoxUnit.SelectedIndex = 0;
        }

        public MaterialCreation(int matId)
        {
            EditMode = true;
            InitializeComponent();
            Title = "Редактирование материала";
            TopLabel.Content = "Редактирование материала";
            ButtonCreate.Content = "Редактировать";
            Id = matId;
            ComboBoxType.ItemsSource = ApplicationCore.context.MaterialTypes.ToList();
            ComboBoxUnit.ItemsSource = ApplicationCore.context.Units.ToList();
            var mat = ApplicationCore.context.Materials.First(v=> v.MaterialId == matId);
            if (mat == null)
                throw new Exception("Материал не был найден");
            ComboBoxType.SelectedValue = mat.MaterialType;
            NameTextBox.Text = mat.MaterialName;
            PriceTextBox.Text = Convert.ToString(mat.UnitPrice);
            ComboBoxUnit.SelectedValue = mat.Unit;
            QuantityTextBox.Text = Convert.ToString(mat.StockQuantity);
            PackageQuantityTextBox.Text = Convert.ToString(mat.PackageQuantity);
            MinimumQuantityTextBox.Text = Convert.ToString(mat.MinimumQuantity);
            ApplicationCore.context.Entry(mat).State = EntityState.Detached;
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            Material finalMat = new Material();
            decimal temp;
            try
            {

                if (!decimal.TryParse(PriceTextBox.Text, out temp))
                    throw new Exception("Цена написана неправильно. Измените поле \"Цена единицы материала\"");
                if (!decimal.TryParse(PackageQuantityTextBox.Text, out temp))
                    throw new Exception("Количество в упаковке написано неправильно. Измените поле \"Количество в упаковке\"");
                if (!decimal.TryParse(QuantityTextBox.Text, out temp))
                    throw new Exception("Количество на складе написано неправильно. Измените поле \"Количество на складе\"");
                if (!decimal.TryParse(MinimumQuantityTextBox.Text, out temp))
                    throw new Exception("Минимальное количество написано неправильно. Измените поле \"Минимальное количество\"");

                if (decimal.Parse(PriceTextBox.Text) < 0)
                    throw new Exception("Цена меньше допустимого. Измените поле \"Цена единицы материала\"");
                if (decimal.Parse(PackageQuantityTextBox.Text) < 0)
                    throw new Exception("Количество в упаковке меньше допустимого. Измените поле \"Количество в упаковке\"");
                if (decimal.Parse(QuantityTextBox.Text) < 0)
                    throw new Exception("Количество на складе меньше допустимого. Измените поле \"Количество на складе\"");
                if (decimal.Parse(MinimumQuantityTextBox.Text) < 0)
                    throw new Exception("Минимальное количество меньше допустимого. Измените поле \"Минимальное количество\"");

                finalMat.MaterialId = Id;
                finalMat.MaterialName = NameTextBox.Text;
                finalMat.MaterialTypeId = ((MaterialType)ComboBoxType.SelectedValue).MaterialTypeId;
                finalMat.UnitId = ((Unit)ComboBoxUnit.SelectedValue).UnitId;
                finalMat.UnitPrice = Convert.ToDecimal(PriceTextBox.Text);
                finalMat.StockQuantity = Convert.ToDecimal(QuantityTextBox.Text);
                finalMat.MinimumQuantity = Convert.ToDecimal(MinimumQuantityTextBox.Text);
                finalMat.PackageQuantity = Convert.ToDecimal(PackageQuantityTextBox.Text);
                finalMat.IsActive = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (EditMode)
                ApplicationCore.context.Update(finalMat);
            else
                ApplicationCore.context.Materials.Add(finalMat);
            ApplicationCore.context.SaveChanges();
            Close();
        }
    }
}
