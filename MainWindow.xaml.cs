using FirstProject.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ProductMaterial> ProdMats = ApplicationCore.context.ProductMaterials.ToList();
        public MainWindow()
        {
            InitializeComponent();
            UpdateData();
        }

        private void UpdateData()
        {
            List<Material> materials = ApplicationCore.context.Materials.Where(v => v.IsActive).ToList();
            MaterialPanel.Children.Clear();
            foreach (Material mat in materials)
                MaterialPanel.Children.Add(CreateMaterialGraphic(mat));
            ListBoxProducts.ItemsSource = ApplicationCore.context.Products.Where(v => v.IsActive).ToList();
            ProductSelectionSearch(ListBoxProducts.SelectedItems.Cast<Product>().ToList());
        }

        private Grid CreateMaterialGraphic(Material mat)
        {
            Grid grid = new Grid() { Height = 150, Margin = new Thickness(1,3,1,3) };

            for (int i = 0; i < 5; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 2; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            Rectangle border = new Rectangle() { StrokeThickness = 2, Stroke = Brushes.Black, Fill = (Brush)new BrushConverter().ConvertFrom("#FFFFFF")! };
            Grid.SetColumnSpan(border, 2);
            Grid.SetRowSpan(border, 5);
            grid.Children.Add(border);
            grid.Children.Add(new Label { Content = $"{mat.MaterialType.TypeName} | {mat.MaterialName}" });
            Label MinQuantityLabel = new Label() { FontFamily = new FontFamily("Georgia") , Content = $"Минимальное количество: {mat.MinimumQuantity} {mat.Unit.Symbol}" };
            Grid.SetRow(MinQuantityLabel, 1);
            grid.Children.Add(MinQuantityLabel);
            Label StorageLabel = new Label() { FontFamily = new FontFamily("Georgia"), Content = $"Количество на складе: {mat.StockQuantity} {mat.Unit.Symbol}" };
            Grid.SetRow(StorageLabel, 2);
            grid.Children.Add(StorageLabel);
            Label PriceLabel = new Label() { FontFamily = new FontFamily("Georgia"), Content = $"Цена: {mat.UnitPrice} р / {mat.Unit.Symbol}" };
            Grid.SetRow(PriceLabel, 3);
            grid.Children.Add(PriceLabel);
            Label NeededLabel = new Label() { FontFamily = new FontFamily("Georgia"), Name = "neededLabel", HorizontalAlignment = HorizontalAlignment.Right ,Content = $"Требуемое количество: 0.00 {mat.Unit.Symbol}" };
            Grid.SetRow(NeededLabel, 4);
            Grid.SetColumn(NeededLabel, 2);
            grid.Children.Add(NeededLabel);
            grid.Tag = Tuple.Create(mat,0.0m);
            Button button = new Button() { Opacity = 0 , FontFamily = new FontFamily("Georgia"), Content = $"Редактировать", Margin = new Thickness(2)};
            button.Click += (source, e) => { try { new MaterialCreation(mat.MaterialId).ShowDialog(); UpdateData(); } catch (Exception ex) { MessageBox.Show(ex.Message); } };
            
            Grid.SetRowSpan(button,10);
            Grid.SetColumnSpan(button,10);
            grid.Children.Add(button);
            return grid;
        }

        private void ListBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductSelectionSearch(ListBoxProducts.SelectedItems.Cast<Product>().ToList());
        }

        private void ProductSelectionSearch(List<Product> SelectedValues)
        {
            var SelectedValuesNeeded = ProdMats.Where(v => SelectedValues.Any(n => n.ProductId == v.ProductId));

            foreach (Grid child in MaterialPanel.Children)
            {
                decimal needed_amount = 0.00m;
                Material child_material = (Material)((Tuple<Material, decimal>)child.Tag).Item1;
                if (SelectedValuesNeeded.Any(v => v.MaterialId == child_material.MaterialId))
                    foreach (ProductMaterial prod in SelectedValuesNeeded.Where(v => v.MaterialId == child_material.MaterialId))
                        needed_amount += child_material.MinimumQuantity * prod.QuantityPerUnit;
                child.Tag = Tuple.Create(child_material, needed_amount);
                child.Children.OfType<Label>().LastOrDefault()!.Content = $"Требуемое количество: {Math.Round(needed_amount, 2)} {child_material.Unit.Symbol}";
            }
            var sorted = MaterialPanel.Children.Cast<FrameworkElement>().OrderByDescending(v => ((Tuple<Material, Decimal>)v.Tag).Item2).ToList();
            MaterialPanel.Children.Clear();
            foreach (var child in sorted)
                MaterialPanel.Children.Add(child);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                new MaterialCreation().ShowDialog();
                UpdateData();
            } 
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message); 
            }
        }
    }
}