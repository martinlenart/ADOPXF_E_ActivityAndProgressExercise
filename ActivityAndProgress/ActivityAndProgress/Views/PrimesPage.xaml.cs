using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using ActivityAndProgress.Models;
using ActivityAndProgress.Services;

namespace ActivityAndProgress.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrimesPage : ContentPage
    {
        PrimeNumberService service = new PrimeNumberService();

        public PrimesPage()
        {
            InitializeComponent();
        }
        public PrimesPage(int NrBatches) : this()
        {
            enNrBatches.Text = NrBatches.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Code here will run right before the screen appears
            //You want to set the Title or set the City

            //This is making the first load of data
            MainThread.BeginInvokeOnMainThread(async () => { await LoadPrimes(); });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await LoadPrimes();
        }
        private async Task LoadPrimes()
        {
            if (!int.TryParse(enNrBatches.Text, out int result)) return;

            var progressReporter = new Progress<float>(value =>
            {
                progressBar.Progress = value;
            });


            int nrbatches = result;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            progressBar.IsVisible = true;
            lvPrimes.IsVisible = false;

            lvPrimes.ItemsSource = await service.GetPrimeBatchCountsAsync(nrbatches, progressReporter);

            lvPrimes.IsVisible = true;
            progressBar.IsVisible = false;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;

        }

        private async void lvPrimes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (PrimeBatch)e.Item;
            var answer = await DisplayAlert("Write to disk?", $"Would you like to write the {item.NrPrimes} prime numbers to disk", "Yes", "No");
            if (answer)
            {
                //Write to disk
                string path = await WriteAsync(item, $"Primes_from_{item.BatchStart}_to_{item.BatchEnd}.txt");
                await DisplayAlert("Write Completed", $"Filename: {path}", "OK");
            }
        }
        public async Task<string> WriteAsync(PrimeBatch batch, string filename)
        {
            List<int> primes = await service.GetPrimesAsync(batch.BatchStart, PrimeBatch.BatchSize);
            string path = fname(filename);

            using (FileStream fs = File.Create(path))
            using (TextWriter writer = new StreamWriter(fs))
            {
                await writer.WriteLineAsync(batch.ToString());
                await writer.WriteLineAsync($"First Prime: {primes.First()}  Last Prime: {primes.Last()}");
                int nrPerLine = 50;
                for (int i = 0; i <= batch.NrPrimes; i++)
                {
                    string sPrimes = String.Join<int>(", ", primes.Take(nrPerLine));
                    await writer.WriteLineAsync(sPrimes);

                    if (primes.Count > nrPerLine)
                        primes.RemoveRange(0, nrPerLine);
                }
            }

            return path;
        }
        static string fname(string name)
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentPath = System.IO.Path.Combine(documentPath, "AOOP2", "Examples");
            if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
            return System.IO.Path.Combine(documentPath, name);
        }
    }
}