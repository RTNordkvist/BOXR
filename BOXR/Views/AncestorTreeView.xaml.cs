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

        /// <summary>
        /// The AncestorTreeView is made in code-behind in a grid structure
        /// </summary>
        public void GenerateTree(AncestorTreeViewModel context)
        {
            var grid = new Grid();
            grid.ShowGridLines = false;
            // The ancestor tree is loaded in the viewmodel and return in a flat structure. The number of generations are also defined in the viewmodel.
            var dogs = context.LoadAncestorTree();

            // The number of generations is derived from the flat ancester tree.
            var generations = dogs.Max(x => x.GenerationsFromBase) + 1;
            // The dimensions of the grid is calculated based on number of generations
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
            this.Width = Math.Min(columns * 200 + 150, 1200);
            this.Height = Math.Min(rows * 45 + 150, 900);

            // A dictionary is made containing duplicates and the number os times, they appear in the tree 
            var duplicates = dogs
                .Where(x => x.PedigreeNumber != null)
                .GroupBy(x => x.PedigreeNumber)
                .Where(x => x.Count() > 1)
                .ToDictionary(x => x.Key, x => x.Count());
            _inbreedCount = 0;
            _colorsDictionary = new();


            // The nodes are grouped in seperate lists based on their generation and then rendered using PlaceDogInGrid
            foreach (var dogGroup in dogs.GroupBy(x => x.GenerationsFromBase))
            {
                PlaceDogsInGrid(dogGroup.ToList(), grid, dogGroup.Key, context, duplicates);
            }
        }

        /// <summary>
        /// Places the dognodes in the grid structure based their generation
        /// </summary>

        private void PlaceDogsInGrid(List<DogNode> dogs, Grid grid, int generationFromBase, AncestorTreeViewModel context, Dictionary<string, int> duplicates)
        {
            // The base dog has index 0 and is a special case
            if (generationFromBase == 0)
            {
                CreateDogButton(dogs[0], grid, (grid.RowDefinitions.Count + 1) / 2 - 1, generationFromBase, context, null);
            }
            else
            {
                var column = generationFromBase;
                var nodeDistance = (grid.RowDefinitions.Count + 1) / (int)(Math.Pow(2, generationFromBase)); // Vertical distance (given in number of rows) between nodes
                var firstNodeRow = ((grid.RowDefinitions.Count + 1) - (nodeDistance * (dogs.Count - 1) + 1)) / 2;

                for (var i = 0; i < dogs.Count; i++)
                {
                    var isFemale = i % 2 == 0; //kønnet afgø
                    var dogRow = firstNodeRow + i * nodeDistance;
                    // Each noed is rendered as a button. The color depends on the gender.
                    CreateDogButton(dogs[i], grid, dogRow, column, context, isFemale ? "#F8C4B4" : "#B8E8FC");

                    // The mothers are placed above the father and thus a border is created from the mother to the father to create the links between nodes
                    if (isFemale)
                    {
                        for (var j = dogRow; j < dogRow + nodeDistance; j++)
                        {
                            AddLeftBorder(grid, j, column);
                        }
                    }

                    // Determine if an inbreeding border border should be added
                    if (dogs[i].PedigreeNumber != null && duplicates.ContainsKey(dogs[i].PedigreeNumber) && duplicates[dogs[i].PedigreeNumber] > 0)
                    {
                        // The count of parents of a duplcate is reduced by two 
                        if (dogs[i].Mother != null && dogs[i].Mother.PedigreeNumber != null && duplicates.ContainsKey(dogs[i].Mother.PedigreeNumber))
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

        // Declares the colors used to mark inbred dogs
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

        // Dictionary contains the duplicates and the assigned color. Inbreedcount keeps track of the latest assigned color
        private int _inbreedCount = 0;
        private Dictionary<string, Color> _colorsDictionary = new Dictionary<string, Color>();

        /// <summary>
        /// Adds an inbreeding border in a unique color
        /// </summary>
        private void AddInbreedingBorder(Grid grid, int row, int column, string pedigreeNumber)
        {
            // checks in the dog is already added to the color dictionary and adds it if not
            if (!_colorsDictionary.TryGetValue(pedigreeNumber, out var color))
            {
                color = _colors[_inbreedCount];
                _colorsDictionary.Add(pedigreeNumber, color);
                _inbreedCount++;
            }

            // Adds a border
            var border = new Border();
            border.BorderThickness = new Thickness(3);
            border.BorderBrush = new SolidColorBrush(color);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            grid.Children.Add(border);
        }

        /// <summary>
        /// Defines the bordes whcih creates the links between nodes
        /// </summary>
        private void AddLeftBorder(Grid grid, int row, int column)
        {
            var border = new Border();
            border.BorderThickness = new Thickness(1, 0, 0, 0);
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            grid.Children.Add(border);
        }

        // Creates a button for a node containing text for name and pedigreeNumber
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
            //btn.Style = Application.Current.Resources["treeButton"] as Style;

            // If the dog exist in the system a command is bound to the button, which leads to the selected dog profile.
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
