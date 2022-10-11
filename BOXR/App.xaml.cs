using BOXR.DataAccess.Repositories;
using BOXR.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BOXR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            ColorRepository colorRepository = new ColorRepository();
            DogRepository dogRepository = new DogRepository(colorRepository, connectionString);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(dogRepository, colorRepository)
            };

            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
