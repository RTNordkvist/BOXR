using BOXR.Domain;
using BOXR.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BOXR.UI.Views
{
    /// <summary>
    /// Interaction logic for AncestorTreeView.xaml
    /// </summary>
    public partial class AncestorTreeView : Window
    {
        public AncestorTreeView()
        {
            InitializeComponent();
        }

        public void GenerateTree(AncestorTreeViewModel context)
        {
            var grid = new Grid();
            grid.ShowGridLines = false;
            var dogs = context.LoadAncestorTree();

            var generations = dogs.Max(x => x.GenerationsFromBase) + 1;
            var rows = Math.Pow(2, generations) - 1;
            var columns = generations;

            for (var i = 0; i < columns; i++) // Columns
            {
                var column = new ColumnDefinition();
                column.Width = new GridLength(200);
                grid.ColumnDefinitions.Add(column);
            }

            for (var i = 0; i < rows; i++) // Rows
            {
                var row = new RowDefinition();
                row.Height = new GridLength(45);
                grid.RowDefinitions.Add(row);
            }
            var scrollViewer = (ScrollViewer)this.FindName("scrollViewer");
            scrollViewer.Content = grid;
            this.Width = Math.Min(columns * 200 + 100, 1200);
            this.Height = Math.Min(rows * 45 + 100, 900);

            var duplicates = dogs
                .Where(x => x.PedigreeNumber != null)
                .GroupBy(x => x.PedigreeNumber)
                .Where(x => x.Count() > 1)
                .ToDictionary(x => x.Key, x => x.Count());
            _inbreedCount = 0;
            _colorsDictionary = new();
            foreach (var dogGroup in dogs.GroupBy(x => x.GenerationsFromBase))
            {
                PlaceDogInGrid(dogGroup.ToList(), grid, dogGroup.Key, context, duplicates);
            }
        }

        private void PlaceDogInGrid(List<DogNode> dogs, Grid grid, int generationFromBase, AncestorTreeViewModel context, Dictionary<string, int> duplicates)
        {
            if (generationFromBase == 0)
            {
                CreateDogButton(dogs[0], grid, (grid.RowDefinitions.Count + 1) / 2 - 1, generationFromBase, context, null);
            }
            else
            {
                var column = generationFromBase;
                var nodeDistance = (grid.RowDefinitions.Count + 1) / (int)(Math.Pow(2, generationFromBase));
                var firstNodeRow = ((grid.RowDefinitions.Count + 1) - (nodeDistance * (dogs.Count - 1) + 1)) / 2;

                for (var i = 0; i < dogs.Count; i++)
                {
                    var isFemale = i % 2 == 0;
                    var dogRow = firstNodeRow + i * nodeDistance;
                    CreateDogButton(dogs[i], grid, dogRow, column, context, isFemale ? "#F8C4B4" : "#B8E8FC");
                    if (isFemale)
                    {
                        for (var j = dogRow; j < dogRow + nodeDistance; j++)
                        {
                            AddLeftBorder(grid, j, column);
                        }
                    }

                    // Determine if we should add a border
                    if (dogs[i].PedigreeNumber != null && duplicates.ContainsKey(dogs[i].PedigreeNumber) && duplicates[dogs[i].PedigreeNumber] > 0)
                    {
                        if(dogs[i].Mother != null && dogs[i].Mother.PedigreeNumber != null && duplicates.ContainsKey(dogs[i].Mother.PedigreeNumber))
                        {
                            duplicates[dogs[i].Mother.PedigreeNumber] -= 2; 
                        }
                        if (dogs[i].Father != null && dogs[i].Father.PedigreeNumber != null && duplicates.ContainsKey(dogs[i].Father.PedigreeNumber))
                        {
                            duplicates[dogs[i].Father.PedigreeNumber] -= 2;
                        }
                        AddInbreedingBorder(grid, dogRow, column, dogs[i].PedigreeNumber);
                    }
                }
            }
        }

        private Color[] _colors = new Color[]
        {
            Colors.Red,
            Colors.Purple,
            Colors.Green,
            Colors.Orange,
            Colors.Blue,
            Colors.Pink,
            Colors.Turquoise,
            Colors.Yellow
        };

        private int _inbreedCount = 0;
        private Dictionary<string, Color> _colorsDictionary = new Dictionary<string, Color>();

        private void AddInbreedingBorder(Grid grid, int row, int column, string pedigreeNumber)
        {
            if (!_colorsDictionary.TryGetValue(pedigreeNumber, out var color))
            {
                color = _colors[_inbreedCount];
                _colorsDictionary.Add(pedigreeNumber, color);
                _inbreedCount++;
            }
            var border = new Border();
            border.BorderThickness = new Thickness(3);
            border.BorderBrush = new SolidColorBrush(color);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            grid.Children.Add(border);
        }

        private void AddLeftBorder(Grid grid, int row, int column)
        {
            var border = new Border();
            border.BorderThickness = new Thickness(1, 0, 0, 0);
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            grid.Children.Add(border);
        }

        private void CreateDogButton(DogNode dog, Grid grid, int row, int column, AncestorTreeViewModel context, string color)
        {
            var btn = new Button();
            var stackPanel = new StackPanel();
            var nameBlock = new TextBlock();
            nameBlock.Text = dog.Name ?? "Unknown";
            var pdBlock = new TextBlock();
            pdBlock.Text = dog.PedigreeNumber;
            stackPanel.Children.Add(nameBlock);
            stackPanel.Children.Add(pdBlock);
            btn.Content = stackPanel;

            if (dog.Name != null && dog.PedigreeNumber != null)
            {
                btn.Command = context.ChangeProfileViewCommand;
                btn.CommandParameter = dog.PedigreeNumber;
            }
            else
            {
                btn.IsEnabled = false;
            }

            if (color != null)
            {
                var bc = new BrushConverter();
                btn.Background = (Brush)bc.ConvertFrom(color);
            }
            Grid.SetRow(btn, row);
            Grid.SetColumn(btn, column);
            grid.Children.Add(btn);
        }
    }
}
