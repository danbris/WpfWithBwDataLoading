using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ObservableCollection<Person> Persons { get; } = new ObservableCollection<Person>();
		public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			Loaded += MainWindow_Loaded;
		}

		private BackgroundWorker _bwPersons = new BackgroundWorker();
		private BackgroundWorker _bwProducts = new BackgroundWorker();
		private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Task.Delay(2000);
			
			
			_bwPersons.WorkerReportsProgress = true;
			_bwPersons.DoWork += BwPersons_DoWork;
			_bwPersons.RunWorkerCompleted += BwPersons_RunWorkerCompleted;
			_bwPersons.ProgressChanged += BwPersons_ProgressChanged;
			_bwPersons.RunWorkerAsync();

			_bwProducts.WorkerReportsProgress = true;
			_bwProducts.DoWork += _bwProducts_DoWork;
			_bwProducts.RunWorkerCompleted += _bwProducts_RunWorkerCompleted;
			_bwProducts.ProgressChanged += _bwProducts_ProgressChanged;
			_bwProducts.RunWorkerAsync();

			

			
		}

		private void _bwProducts_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			var newProducts = e.UserState as List<Product>;
			foreach (var product in newProducts)
				Products.Add(product);
		}

		private void _bwProducts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			img2.Visibility = Visibility.Collapsed;
		}

		private void _bwProducts_DoWork(object sender, DoWorkEventArgs e)
		{
			var priceRand = new Random(300);
			var qtyRand = new Random(10);
			var bulkIndex = 0;
			var persons = new List<Product>();
			for (var i = 0; i < 1000000; i++)
			{
				if (bulkIndex == 40)
				{
					_bwProducts.ReportProgress(i * 100 / 1000000, persons.ToList());
					persons.Clear();
					bulkIndex = 0;
				}
				persons.Add(new Product
				{
					Name = $"Nume {i}",
					Price = priceRand.Next(10, 500),
					Quantity = qtyRand.Next(0, 100)
				});

				bulkIndex++;
			}
		}

		private void BwPersons_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			var newPersons  = e.UserState as List<Person>;
			foreach (var person in newPersons)
				Persons.Add(person);
		}

		private void BwPersons_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			img1.Visibility = Visibility.Collapsed;
		}

		private void BwPersons_DoWork(object sender, DoWorkEventArgs e)
		{
			var ageRand = new Random(20);
			var cnpRand = new Random(12343365);
			var bulkIndex = 0;
			var persons = new List<Person>();
			for (var i = 0; i < 1000000; i++)
			{
				if (bulkIndex == 20)
				{
					_bwPersons.ReportProgress(i * 100 / 1000000, persons.ToList());
					persons.Clear();
					bulkIndex = 0;
				}

				persons.Add(new Person
				{
					Name = $"Nume {i}",
					Address = $"Totally random text {i}",
					Age = ageRand.Next(20, 40),
					CNP = cnpRand.Next(),
					PhoneNo = $"{i}(9438458{i++}"
				});
				bulkIndex++;
			}
		}
	}
}
