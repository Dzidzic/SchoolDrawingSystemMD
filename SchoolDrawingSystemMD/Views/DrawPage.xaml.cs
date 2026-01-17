using SchoolDrawingSystemMD.Services;

namespace SchoolDrawingSystemMD.Views;

public partial class DrawPage : ContentPage
{
    public DrawPage()
    {
        InitializeComponent();

        /*
        var allData = new Models.AllSchoolClasses();
        SeedData(allData);
        BindingContext = allData;

        TxtFileServices txtFileServices = new();
        Task.Run(async () => await txtFileServices.SaveData(allData));
        */
        _ = TestFileCycle();
    }

    private async Task TestFileCycle()
    {
        TxtFileServices txtFileServices = new();

        try
        {
            // 3. Teraz czyœcimy dane w pamiêci i ³adujemy je PONOWNIE z pliku
            // To jest prawdziwy test odczytu!
            var loadedData = await txtFileServices.LoadData();
            System.Diagnostics.Debug.WriteLine($"KROK 2: Dane odczytane. Liczba klas: {loadedData.SchoolClasses.Count}");

            // 4. Przypisujemy odczytane dane do BindingContext
            // Jeœli na ekranie pojawi¹ siê klasy i uczniowie, oznacza to, ¿e LoadData dzia³a!
            BindingContext = loadedData;

            Console.WriteLine(loadedData);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"B£¥D TESTU: {ex.Message}");
        }
    }

    private void SeedData(Models.AllSchoolClasses allData)
    {
        var rnd = new Random();
        string[] classNames = { "1A", "2B", "3C" };

        foreach (var name in classNames)
        {
            // 1. Tworzymy now¹ klasê
            var schoolClass = new Models.SchoolClass
            {
                Id = Guid.NewGuid(),
                Name = name,
                Students = new System.Collections.ObjectModel.ObservableCollection<Models.Student>()
            };

            // 2. Losujemy liczbê uczniów od 8 do 11
            int studentCount = rnd.Next(8, 12); // Next(min, max) -> max jest wy³¹czony, wiêc 12 daje max 11

            for (int i = 1; i <= studentCount; i++)
            {
                var student = new Models.Student
                {
                    Id = Guid.NewGuid(),
                    FirstName = $"Imiê_{name}_{i}",
                    LastName = $"Nazwisko_{i}",
                    IsPresent = true,
                    DrawCooldown = 0
                };

                schoolClass.Students.Add(student);
            }

            // 3. Dodajemy klasê do g³ównej kolekcji
            allData.SchoolClasses.Add(schoolClass);
        }
    }
}