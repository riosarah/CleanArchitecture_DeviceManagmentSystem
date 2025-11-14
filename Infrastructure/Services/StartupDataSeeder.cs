using Application.Contracts;
using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Infrastructure.Services;

/*
dotnet ef migrations add InitialMigration --project ./Infrastructure --startup-project ./Api --output-dir ./Persistence/Migrations
dotnet ef database update --project ./Infrastructure --startup-project ./Api
 */


public sealed class StartupDataSeederOptions
{
    public string CsvPath { get; set; } = string.Empty;
}

/// <summary>
/// Hosted Service, der beim Start Migrationen ausführt und die DB einmalig aus CSV befüllt.
/// </summary>
public class StartupDataSeeder(IOptions<StartupDataSeederOptions> options, IServiceProvider serviceProvider) : IHostedService
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly string _csvPath = options.Value.CsvPath;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //await dbContext.Database.MigrateAsync(cancellationToken);

        // Nur seeden, wenn noch keine Sensoren existieren (idempotent)
        if (await dbContext.Sensors.AnyAsync(cancellationToken)) return;

        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var allMeasurements = await ReadMeasurementsFromCsv(uow, cancellationToken);
        dbContext.Measurements.AddRange(allMeasurements);
        await dbContext.SaveChangesAsync(cancellationToken);


        var Devices = await ReadDevicesFromCsv(uow, cancellationToken);
        var Persons = await ReadPersonsFromCsv(uow, cancellationToken);

        dbContext.Devices.AddRange(Devices);
        dbContext.Persons.AddRange(Persons);
        await dbContext.SaveChangesAsync(cancellationToken);

        var Usage = await ReadUsagesFromCsv(uow, dbContext, cancellationToken);
        dbContext.Usages.AddRange(Usage);

        await dbContext.SaveChangesAsync(cancellationToken);
    }


    /// <summary>
    /// Lädt die CSV nur einmalig (Thread-sicher) und baut Sensor- und Messwertobjekte.
    /// </summary>
    private async Task<IEnumerable<Measurement>> ReadMeasurementsFromCsv(IUnitOfWork uow, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        var uniquenessChecker = new SensorUniquenessChecker(uow);
        try
        {
            if (!File.Exists(_csvPath))
            {
                throw new Exception($"File {_csvPath} doesn't exist");
            }
            var lines = await File.ReadAllLinesAsync(_csvPath, cancellationToken);
            var sensors = new Dictionary<string, Sensor>();
            var measurements = new List<Measurement>(lines.Length);
            for (int i = 1; i < lines.Length; i++) // i=1: Header überspringen
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');
                if (parts.Length < 4) continue;
                var datePart = parts[0].Trim();
                var timePart = parts[1].Trim();
                if (!DateTime.TryParseExact(datePart + " " + timePart, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var ts))
                    continue;
                var sensorRaw = parts[2].Trim();
                if (string.IsNullOrEmpty(sensorRaw)) continue;
                var segs = sensorRaw.Split('_', 2);
                if (segs.Length != 2) continue;
                var location = segs[0];
                var name = segs[1];
                var key = location + "|" + name;
                if (!sensors.TryGetValue(key, out var sensor))
                {
                    sensor = await Sensor.CreateAsync(location, name, uniquenessChecker, cancellationToken);
                    //sensor = new Sensor(location, name);
                    sensors[key] = sensor;
                    await uow.Sensors.AddAsync(sensor, cancellationToken);
                    await uow.SaveChangesAsync(cancellationToken); // Sensor sofort speichern, damit Id gesetzt ist
                }
                var valueStr = parts[^1].Trim();
                if (!double.TryParse(valueStr.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                    continue;
                measurements.Add(Measurement.Create(sensor, value, ts));
            }
            return measurements;
        }
        finally
        {
            _lock.Release();
        }
    }


    /// <summary>
    /// Lädt die CSV nur einmalig (Thread-sicher) und baut Sensor- und Messwertobjekte.
    /// </summary>
    private async Task<IEnumerable<Device>> ReadDevicesFromCsv(IUnitOfWork uow, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        //var uniquenessChecker = new SensorUniquenessChecker(uow);
        try
        {
            if (!File.Exists(_csvPath))
            {
                throw new Exception($"File {_csvPath} doesn't exist");
            }
            var lines = await File.ReadAllLinesAsync(_csvPath, cancellationToken);
            var Devices = new List<Device>();
            for (int i = 1; i < lines.Length; i++) // i=1: Header überspringen
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');
                var device = new Device
                {
                    SerialNumber = parts[0].Trim(),
                    Name = parts[1].Trim(),
                    type = Enum.Parse<Device.DeviceType>(parts[2].Trim())
                };
                Devices.Add(device);
                
            }
            return Devices;
        }
        finally
        {
            _lock.Release();
        }
    }
    private async Task<IEnumerable<Usage>> ReadUsagesFromCsv(IUnitOfWork uow, AppDbContext dbContext, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        //var uniquenessChecker = new SensorUniquenessChecker(uow);
        try
        {
            if (!File.Exists(_csvPath))
            {
                throw new Exception($"File {_csvPath} doesn't exist");
            }
            var lines = await File.ReadAllLinesAsync(_csvPath, cancellationToken);
            var Usages = new List<Usage>();
            for (int i = 1; i < lines.Length; i++) // i=1: Header überspringen
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');

                var person = dbContext.Persons.FirstOrDefault(p => p.LastName == parts[3].Trim() && p.FirstName == parts[4].Trim());
                var device = dbContext.Devices.FirstOrDefault(d => d.SerialNumber == parts[0].Trim());
                var usage = new Usage
                {
                    Person = person,
                    PersonId = person.Id,
                    Device = device,
                    DeviceId = device.Id,
                    From = DateTime.Parse(parts[6]),
                    To = DateTime.Parse(parts[7])
                };
                Usages.Add(usage);

            }
            return Usages;
        }
        finally
        {
            _lock.Release();
        }
    }
    private async Task<IEnumerable<Person>> ReadPersonsFromCsv(IUnitOfWork uow, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        //var uniquenessChecker = new SensorUniquenessChecker(uow);
        try
        {
            if (!File.Exists(_csvPath))
            {
                throw new Exception($"File {_csvPath} doesn't exist");
            }
            var lines = await File.ReadAllLinesAsync(_csvPath, cancellationToken);
            var Persons = new List<Person>();
            for (int i = 1; i < lines.Length; i++) // i=1: Header überspringen
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');
                var person = new Person
                {
                    LastName = parts[3].Trim(),
                    FirstName = parts[4].Trim(),
                    MailAddress = parts[5].Trim()
                };
                Persons.Add(person);

            }
            return Persons;
        }
        finally
        {
            _lock.Release();
        }
    }


    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
